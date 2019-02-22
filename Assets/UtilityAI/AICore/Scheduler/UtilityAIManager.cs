namespace AtlasAI.Scheduler
{
    using System.Collections.Generic;
    using UnityEngine;


    public class UtilityAIManager : SingleInstanceComponent<UtilityAIManager>
    {
        //
        // Fields
        //
        [SerializeField]
        private SchedulerConfig _configurations;
        private SchedulerQueue _scheduler;

        [ReadOnly, SerializeField]
        private int scheduledItemCount;
        //
        // Properties
        //
        public SchedulerConfig configurations{
            get { return _configurations; }
        }

        public SchedulerQueue scheduler{
            get { return _scheduler; }
        }

        [Header("Misc Info")]
        [HideInInspector]
        public string timeSinceLevelLoaded;
        [HideInInspector]
        public SchedulerQueue.SchedulerItem[] heap;

        //
        // Methods
        //
		protected override void OnAwake()
        {
            base.OnAwake();

            //  Load all AIs
            AIManager.EagerLoadAll();


            _configurations = new SchedulerConfig();
            _scheduler = new SchedulerQueue(16, 
                                            configurations.updateInterval, 
                                            configurations.maxUpdatesPerFrame, 
                                            configurations.maxUpdateTimeInMillisecondsPerUpdate);
            
            //  Set the configurations scheduler.
            configurations.ApplyTo(scheduler);

            heap = _scheduler._queue.heap;
		}


        private void Update()
		{
            scheduledItemCount = scheduler.itemCount;
            scheduler.Update(Time.timeSinceLevelLoad);

            timeSinceLevelLoaded = Time.timeSinceLevelLoad.ToString();
		}




        //public IEnumerable<SchedulerQueue> __Scheduler
        //{
        //    get
        //    {
        //        var clients = AIManager.allClients;
        //        var clientCount = 0;
        //        foreach (var item in clients)
        //        {
        //            //Debug.Log(item.ai.name + " | " + clientCount);
        //            clientCount++;
        //        }
        //        int numSchedulers = (clientCount % configurations.maxUpdatesPerFrame > 0) ?
        //            clientCount / configurations.maxUpdatesPerFrame + 1 :
        //            clientCount / configurations.maxUpdatesPerFrame;
        //        //Debug.LogFormat("Num of Schedulers: {0}\nClient Count : {1}\nMax Updates Per Frame: {2}\nRemainder: {3} ", numSchedulers, clientCount, configurations.maxUpdatesPerFrame);
        //        _scheduler = new SchedulerQueue[numSchedulers];
        //        for (int i = 0; i < _scheduler.Length; i++)
        //        {
        //            yield return new SchedulerQueue(configurations.maxUpdatesPerFrame, configurations.updateInterval);
        //        }
        //    }
        //}
	
    }

}

