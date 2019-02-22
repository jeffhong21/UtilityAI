using AtlasAI.DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtlasAI.Scheduler
{
    
    public class SchedulerQueue
    {
        //
        // Fields
        //
        public BinaryHeap<SchedulerItem> _queue;
        private Stopwatch _watch;


        //
        // Properties
        //


        public float defaultUpdateInterval{
            get;
            set;
        }

        public int itemCount{
            get { return _queue.count; }
        }

        public int maxUpdatesPerInterval{
            get;
            set;
        }

        public int maxUpdateTimeInMillisecondsPerUpdate{
            get;
            set;
        }

        /// <summary>
        /// The number of items in the queue that were processed during the previous cycle.
        /// </summary>
        public int updatedItemsCount{
            get;
            private set;
        }

        /// <summary>
        /// Total milliseconds used on the last Update().
        /// </summary>
        public long updateMillisecondsUsed{
            get;
            private set;
        }

        /// <summary>
        /// How many items have not ben updated.
        /// </summary>
        public float updatesOverdueByTotal{
            get;
            private set;
        }

        //
        // Constructors
        //
        public SchedulerQueue(int capacity)
        {
            _queue = new BinaryHeap<SchedulerItem>(capacity, new SchedulerComparer());
            _watch = new Stopwatch();
            defaultUpdateInterval = 0.1f;
            maxUpdatesPerInterval = 20;
            maxUpdateTimeInMillisecondsPerUpdate = 4;
        }

        public SchedulerQueue(int capacity, float defaultUpdateInterval, int maxUpdatesPerInterval, int maxUpdateTimeInMillisecondsPerUpdate)
        {
            _queue = new BinaryHeap<SchedulerItem>(capacity, new SchedulerComparer());
            _watch = new Stopwatch();
            this.defaultUpdateInterval = defaultUpdateInterval;
            this.maxUpdatesPerInterval = maxUpdatesPerInterval;
            this.maxUpdateTimeInMillisecondsPerUpdate = maxUpdateTimeInMillisecondsPerUpdate;
        }



        //
        // Methods
        //

        /// <summary>
        /// Create a new SchedulerItem and add the Client to it.
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="item">Item.</param>
        /// <param name="interval">Interval.</param>
        public ISchedulerHandle Add(IClientScheduler item, float interval)
        {
            //  Initialize a SchedulerItem.
            SchedulerItem schedulerItem = new SchedulerItem();
            schedulerItem.parent = this;
            schedulerItem.nextUpdate = interval;
            schedulerItem.interval = interval;
            schedulerItem.item = item;
            //  Add the scheduler item to the binary heap queue.
            _queue.Add(schedulerItem);

            return schedulerItem;
        }


        public ISchedulerHandle Add(IClientScheduler item, float interval, float delayFirstUpdateBy)
        {
            //  Initialize a SchedulerItem and cache it.
            SchedulerItem schedulerItem = (SchedulerItem)Add(item, interval);
            //  Update nextUpdate variable.
            schedulerItem.nextUpdate += delayFirstUpdateBy;
            return schedulerItem;
        }


        public void Remove(IClientScheduler item)
        {

        }


        private void Remove(SchedulerItem item)
        {
            _queue.Remove(item);
        }


        public void Update(float frameBeginTime)
        {
            //  Reset variables.
            updatedItemsCount = 0;
            updatesOverdueByTotal = 0;

            _watch.Reset();
            _watch.Start();

            do{
                //  Check if the heap has a root node.  If not, return.
                if (_queue.hasNext){
                    //UnityEngine.Debug.LogFormat("Heap has a root item | itemCount: {0} | maxUpdate: {1} | {2}", updatedItemsCount, maxUpdatesPerInterval, updatedItemsCount > maxUpdatesPerInterval);
                    if (updatedItemsCount <= maxUpdatesPerInterval || updatedItemsCount >= itemCount)
                    {
                        //UnityEngine.Debug.Log("Updated item count is less than max item count.");
                        //  Get a reference to the root item of the heap.
                        SchedulerItem schedulerItem = _queue.Peek();
                        //UnityEngine.Debug.LogFormat("NextUpdate: {0} | FrameBeginTime: {1}", schedulerItem.nextUpdate, frameBeginTime);
                        //  If the root item's next update is not less than frame begin time, than no more items are ready for an update.
                        if(schedulerItem.nextUpdate <= frameBeginTime){
                            //UnityEngine.Debug.Log("Next update is less than current frame time.");
                            //  Update updatesOverdueByTotal if time is greater than time allowed.
                            if (_watch.Elapsed.TotalMilliseconds > maxUpdateTimeInMillisecondsPerUpdate)
                                updatesOverdueByTotal = maxUpdatesPerInterval - itemCount;
                            //  Execute the ExecuteUpdate method of the client.
                            float? nextUpdate = schedulerItem.item.ExecuteUpdate(schedulerItem.lastUpdate, schedulerItem.nextUpdate);
                            //  Update updatedItemCount.
                            updatedItemsCount++;

                            //  Update schedulerItem
                            schedulerItem.lastUpdate = frameBeginTime;
                            schedulerItem.nextUpdate = nextUpdate == null ? defaultUpdateInterval : frameBeginTime + (float)nextUpdate;
                            //  Reset the item in the heap.
                            _queue.Remove();
                            _queue.Add(schedulerItem);

                            //UnityEngine.Debug.LogFormat("Heap: {0}", _queue.Peek());
                            //UnityEngine.Debug.Break();
                        }
                        else{
                            break;
                        }

                    }
                    else{
                        break;
                    }
                }
                else{
                    break;
                }
            } while (_watch.Elapsed.TotalMilliseconds < maxUpdateTimeInMillisecondsPerUpdate / 100);

            //if(frameBeginTime > 2){
            //    UnityEngine.Debug.LogFormat("UpdatedItemsCount: {0} | ItemCount: {1} | Elapsed Milliseconds: {2}", updatedItemsCount, itemCount, _watch.Elapsed.TotalMilliseconds);
            //    //UnityEngine.Debug.Break();
            //}

            //  Update the total milliseconds used.
            updateMillisecondsUsed = _watch.ElapsedMilliseconds;
        }

        void DebugHeap(){
            string debug = "";
            for (int i = 0; i < _queue.heap.Length; i++)
            {
                debug += string.Format(" {0} |", _queue.heap[i].nextUpdate);
            }
            debug += string.Format("  Count: {0} |  Capacity {1}", _queue.count, _queue.capacity);
            UnityEngine.Debug.Log(debug);
            UnityEngine.Debug.Break();
        }


        //
        // Nested Types
        //
        private class SchedulerComparer : IComparer<SchedulerItem>
        {
            public static readonly IComparer<SchedulerItem> instance;

            public int Compare(SchedulerItem x, SchedulerItem y){
                return x.nextUpdate.CompareTo(y.nextUpdate);
            }

            public SchedulerComparer(){
                //SchedulerComparer.instance = this;
            }
        }

        [Serializable]
        public class SchedulerItem : ISchedulerHandle
        {
            public SchedulerQueue parent;
            //  When the next update will occur.
            public float nextUpdate;
            //  When the last update occured.
            public float lastUpdate;
            //  The time from last update to the next update?
            public float interval;


            //  The UtilityAIClient.
            public IClientScheduler item{
                get;
                set;
            }


            public void Stop(){
                parent._queue.Remove(this);
            }

            public void Pause()
            {
                //  Get the current time.
                float time = float.PositiveInfinity;    //  Need some way to get current time.

                lastUpdate = nextUpdate - time;
                nextUpdate = float.PositiveInfinity;
                //parent.Remove(this);
            }

            public void Resume()
            {
                //  Get the current time.
                float time = float.PositiveInfinity;    //  Need some way to get current time.

                lastUpdate = time;
                nextUpdate = time + interval;
                //parent.Add(item, interval);
            }

            public SchedulerItem(){
                
            }

			public override string ToString()
			{
                return string.Format("Next Update: {0} | Last Update: {1}", nextUpdate, lastUpdate);
			}
		}
    }



}

