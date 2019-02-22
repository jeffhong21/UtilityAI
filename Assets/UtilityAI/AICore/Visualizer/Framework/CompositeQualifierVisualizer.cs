using System;
using System.Collections;
using System.Collections.Generic;

namespace AtlasAI.Visualization
{
    public class CompositeQualifierVisualizer : QualifierVisualizer, ICompositeVisualizer
    {
        //
        // Fields
        //
        private List<IContextualScorer> _scorers;


        //
        // Properties
        //

        /// <summary>
        /// Gets all ScorerVisualizer's.
        /// </summary>
        /// <value>The children.</value>
        public IList children { get; } = new ArrayList();

        //IList ICompositeVisualizer.children { get; } = new ArrayList();


        //
        // Constructors
        //
        public CompositeQualifierVisualizer(ICompositeScorer q, SelectorVisualizer parent) : base((CompositeQualifier)q, parent)
        {
            _scorers = q.scorers;
        }


        //
        // Methods
        //
        void ICompositeVisualizer.Add(object item)
        {
            children.Add(item);
        }


        public override void Reset()
        {
            lastScore = null;
            children.Clear();
        }


        public override float Score(IAIContext context)
        {
            lastScore = qualifier.Score(context);
            return (float)lastScore;
        }
    }
}
