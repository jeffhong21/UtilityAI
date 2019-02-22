using System;
using System.Collections.Generic;

namespace AtlasAI.Visualization
{
    public class SelectorVisualizer : Selector, IVisualizedObject
    {
        //
        // Fields
        //
        private Selector _selector;
        private UtilityAIVisualizer _parent;


        //
        // Properties
        //
        public new Guid id{
            get { return _id; }
        }

        internal IQualifier lastSelectedQualifier{
            get;
            private set;
        }

        public UtilityAIVisualizer parent{
            get { return _parent; }
        }

        public Selector selector{
            get { return _selector; }
        }

        object IVisualizedObject.target{
            get;
        }


        //
        // Constructors
        //
        public SelectorVisualizer(Selector s, UtilityAIVisualizer parent)
        {
            _selector = s;
            _parent = parent;

            RegenerateId();
            Init();
        }


        public void Init()
        {
            for (int index = 0; index < _qualifiers.Count; index++){
                //  Cache the qualifier
                IQualifier qualifier = _qualifiers[index];
                if(qualifier is CompositeQualifier){
                    //  Initialize new CompositieQualiferVisualizer.
                    ICompositeVisualizer compositeVisualizer = new CompositeQualifierVisualizer((CompositeQualifier)qualifier, this);
                    //  Cache CompositeQualifier scorers
                    var scorers = ((CompositeQualifier)qualifier).scorers;
                    //  Loop through each scorer in CompositeQualifier.
                    for (int i = 0; i < scorers.Count; i++){
                        //  Initialize and cache new ScorerVisualizer.
                        var scorerVisualizer = new ScorerVisualizer(scorers[i], (CompositeQualifierVisualizer)compositeVisualizer);
                        //  Add ScorerVisualizer to CompositieQualiferVisualizer list of children.
                        compositeVisualizer.Add(scorerVisualizer);
                    }
                    //  Cast CompositeQualifierVisualizer as IQualifierVisualizer
                    IQualifierVisualizer visualizer = (IQualifierVisualizer)compositeVisualizer;
                    //  Initialize the QualifierVisualizer.
                    visualizer.Init();
                }
                else{
                    IQualifierVisualizer visualizer = new QualifierVisualizer(qualifier, this);
                    visualizer.Init();
                }
            }
        }


        public void Reset()
        {
            lastSelectedQualifier = null;
        }


        public override IQualifier Select(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifier)
        {
            lastSelectedQualifier = Select(context, qualifiers, defaultQualifier);
            return lastSelectedQualifier;
        }
    }
}
