namespace AtlasAI.Scheduler
{
    /// <summary>
    /// Interface for a SchedulerItem that is used in the SchedulerQueue
    /// </summary>
    public interface ISchedulerHandle
    {
        //
        // Properties
        //
        IClientScheduler item
        {
            get;
        }

        //
        // Methods
        //
        void Pause();

        void Resume();

        void Stop();
    }
}

