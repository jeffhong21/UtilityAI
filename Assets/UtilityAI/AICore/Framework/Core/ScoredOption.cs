namespace AtlasAI
{

    public struct ScoredOption<TOption> 
    {
        //
        // Fields
        //
        private TOption _option;

        private float _score;


        //
        // Properties
        //
        public TOption option { 
            get { return _option; }
        }

        public float score{
            get { return _score; }
        }


        //
        // Constructors
        //
        public ScoredOption(TOption option, float score)
        {
            _option = option;
            _score = score;
        }


    }
}