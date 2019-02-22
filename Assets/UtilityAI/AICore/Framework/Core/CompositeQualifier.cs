namespace AtlasAI
{

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A base class for Qualifiers that base their score on a number of child.
    /// </summary>
    public abstract class CompositeQualifier : IQualifier, ICompositeScorer, ICanBeDisabled
    {
        //
        // Fields
        //
        protected List<IContextualScorer> _scorers;

        //
        // Properties
        //
        public IAction action { 
            get; 
            set; 
        }

        public bool isDisabled { 
            get; 
            set; 
        }

        public List<IContextualScorer> scorers 
        {
            get { return _scorers; }
            set { _scorers = value; }  //  Do not need?
        }


        //
        // Constructors
        //
        protected CompositeQualifier()
        {
            _scorers = new List<IContextualScorer>();
        }


        //
        // Methods
        //


        public float Score(IAIContext context)
        {
            return Score(context, scorers);
        }


        public abstract float Score(IAIContext context, List<IContextualScorer> scorers);
    }







}

