using System;
using System.Collections;

namespace AtlasAI.Visualization
{
    public interface ICompositeVisualizer
    {
        //
        // Properties
        //
        IList children
        {
            get;
        }

        //
        // Methods
        //
        void Add(object item);
    }
}
