using System;
using System.Collections;
using System.Collections.Generic;

namespace AtlasAI.Visualization
{
    public class CompositeActionVisualizer : ActionVisualizer, ICompositeVisualizer
    {
        //
        // Fields
        //
        private List<ActionVisualizer> _actions;

        //
        // Properties
        //

        /// <summary>
        /// Gets all ActionVisualizer's.
        /// </summary>
        /// <value>The children.</value>
        public IList children { get; } = new ArrayList();

        //IList ICompositeVisualizer.children { get; } = new ArrayList();


        //
        // Constructors
        //
        public CompositeActionVisualizer(CompositeAction action, IQualifierVisualizer parent) : base(action, parent)
        {
            //children = new List<ICompositeVisualizer>();
            base.Init();
        }


        //
        // Methods
        //
        void ICompositeVisualizer.Add(object item)
        {
            children.Add(item);
        }


        public override void Execute(IAIContext context, bool doCallback)
        {
            
        }


    }
}
