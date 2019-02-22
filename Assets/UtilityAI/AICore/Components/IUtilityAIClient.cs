namespace AtlasAI
{

    public interface IUtilityAIClient
    {

        IUtilityAI ai{
            get;
            set;
        }

        UtilityAIClientState state{
            get;
        }


        IAction activeAction{
            get;
        }

        bool debugClient
        {
            get;
            set;
        }

        //
        // Methods
        //
        void Execute();

        void Pause();

        void Resume();

        void Start();

        void Stop();


    }
}