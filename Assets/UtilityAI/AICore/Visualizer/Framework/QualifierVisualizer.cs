using System;


namespace AtlasAI.Visualization
{
    public class QualifierVisualizer : IQualifierVisualizer, IQualifier, IVisualizedObject
    {
        //
        // Fields
        //
        private IQualifier _qualifier;
        private ActionVisualizer _action;
        private SelectorVisualizer _parent;


        //
        // Properties
        //
        public IAction action{
            get{ return qualifier.action; }
            set{ qualifier.action = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the qualifier is the current high scorer.
        /// </summary>
        /// <value><c>true</c> if this instance is high scorer; otherwise, <c>false</c>.</value>
        public bool isHighScorer{
            get { return _parent.lastSelectedQualifier == this; }
        }

        /// <summary>
        /// Gets the last score.
        /// </summary>
        /// <value>The last score.</value>
        public float? lastScore{
            get;
            protected set;
        }

        public SelectorVisualizer parent{
            get{ return _parent; }
        }

        public IQualifier qualifier {
            get { return _qualifier; }
        }


        object IVisualizedObject.target{
            get;
        }


        //
        // Constructors
        //
        public QualifierVisualizer(IQualifier q, SelectorVisualizer parent)
        {
            _qualifier = q;
            _parent = parent;
        }


        //
        // Methods
        //
        void IQualifierVisualizer.Init()
        {
            if(action is CompositeAction){
                //  Initialize new CompositeActionVisualizer.
                ICompositeVisualizer compositeVisualizer = new CompositeActionVisualizer((CompositeAction)action, this);
                //  Cache CompositeAction actions
                var actions = ((CompositeAction)action).actions;
                //  Loop through each action in CompositeAction.
                for (int i = 0; i < actions.Count; i++){
                    //  Initialize and cache new ActionVisualizer.
                    var actionVisualizer = new ActionVisualizer(action, this);
                    //  Add ActionVisualizer to CompositeActionVisualizer list of children.
                    compositeVisualizer.Add(actionVisualizer);
                    //  Initialize the ActionVisualizer.
                    actionVisualizer.Init();
                }
            }
            else{
                var actionVisualizer = new ActionVisualizer(action, this);
                actionVisualizer.Init();
            }

        }


        public virtual void Reset()
        {
            lastScore = null;
        }


        public virtual float Score(IAIContext context)
        {
            lastScore = qualifier.Score(context);
            return (float)lastScore;
        }
    }
}
