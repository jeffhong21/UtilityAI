using System;
using UnityEngine;

namespace AtlasAI.Scheduler
{
    /// <summary>
    /// This class is the configuration for the scheduler.
    /// </summary>
    [Serializable]
    public class SchedulerConfig
    {
        //
        // Fields
        //
        public float updateInterval = 0.1f;
        public int maxUpdatesPerFrame = 5;  //  Should not be higher than client count.
        public int maxUpdateTimeInMillisecondsPerUpdate = 4;
        [ReadOnly, SerializeField]
        public string targetScheduler;


        //
        // Properties
        //
        public SchedulerQueue associatedScheduler{
            get;
            private set;
        }


        //
        // Constructors
        //
        public SchedulerConfig()
        {
            if(string.IsNullOrWhiteSpace(targetScheduler))
                targetScheduler = "default scheduler";
        }

        //
        // Static Methods
        //
        public static SchedulerConfig From(string name, SchedulerQueue q){
            //  Probably used to get the SchedulerConfig given a name and ScheduerQueue.
            throw new NotImplementedException();
        }


        //
        // Methods
        //
        public void ApplyTo(SchedulerQueue q){
            associatedScheduler = q;
        }
    }
}

