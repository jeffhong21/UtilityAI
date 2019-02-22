using System;

namespace AtlasAI.Visualization
{
    public class ScorerVisualizer : IContextualScorer
    {
        //
        // Fields
        //
        private IContextualScorer _scorer;

        private CompositeQualifierVisualizer _parent;

        //
        // Properties
        //
        public string lastScore
        {
            get;
            private set;
        }

        public CompositeQualifierVisualizer parent{
            get { return _parent; }
        }

        public IContextualScorer scorer{
            get { return _scorer; }
        }


        //
        // Constructors
        //
        public ScorerVisualizer(IContextualScorer scorer, CompositeQualifierVisualizer parent)
        {
            _scorer = scorer;
            _parent = parent;
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }


        public float Score(IAIContext context)
        {
            throw new NotImplementedException();
        }
    }
}
