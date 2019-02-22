using System;

namespace AtlasAI.Visualization
{
    public class DefaultQualifierVisualizer
    {
        //
        // Fields
        //
        private IDefaultQualifier _defQualifier;

        //
        // Properties
        //
        public float score
        {
            get;
            set;
        }



        //
        // Constructors
        //
        public DefaultQualifierVisualizer(IDefaultQualifier q, SelectorVisualizer parent)
        {
            _defQualifier = q;
        }
    }
}
