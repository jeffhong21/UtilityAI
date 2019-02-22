using System;
using System.Collections;
using System.Collections.Generic;

namespace AtlasAI.Utilities
{
    /// <summary>
    /// A buffer pool for C# lists to avoid untimely garbage collection.
    /// </summary>
    public static class ListBufferPool
    {
        //
        // Static Fields
        //
        private static readonly Dictionary<Type, Queue<IList>> _pool = new Dictionary<Type, Queue<IList>>();

        //
        // Static Methods
        //

        //  Get a list allocate from the buffer with a capacity of 5
        //  Example:  var gameObjects = ListBufferPool.GetBuffer<GameObject>(5)
        public static List<T> GetBuffer<T>(int capacityHint)
        {
            //  If Pool does not contain type.
            if (_pool.ContainsKey(typeof(T)) == false)
                PreAllocate<T>(capacityHint);
            
            var pool = _pool[typeof(T)];
            //  If pool has no buffer lists, create a new buffer list.
            if (pool.Count == 0) PreAllocate<T>(capacityHint);

            var bufferList = (List<T>)pool.Dequeue();
            bufferList.Clear();

            //UnityEngine.Debug.LogFormat("**  ListBuffer capacity: {0} | capacityhint : {1}", bufferList.Capacity, capacityHint);
            bufferList.Capacity = capacityHint;
            return bufferList;
        }


        public static void PreAllocate<T>(int capacity, int number = 1)
        {
            if (_pool.ContainsKey(typeof(T)) == false)
                _pool.Add(typeof(T), new Queue<IList>());
            
            IList list = new List<T>(capacity);
            _pool[typeof(T)].Enqueue(list);
        }
            

        //  return the list to the buffer pool after usage, so other parts of the code can reuse the list
        public static void ReturnBuffer<T>(List<T> buffer){
            var pool = _pool[typeof(T)];
            pool.Enqueue(buffer);


            //string info = string.Format("ListBufferPool | type: {0}, count : {1} \n ", typeof(T), pool.Count);
            //var poolArray = pool.ToArray();
            //for (int i = 0; i < poolArray.Length; i++)
            //{
            //    var _poolArray = poolArray[i] as List<T>;
            //    info += string.Format("ListPool({0}) | capacity: {1} \n", i, _poolArray.Capacity);
            //}
            //UnityEngine.Debug.Log(info);
        }


    }
}
