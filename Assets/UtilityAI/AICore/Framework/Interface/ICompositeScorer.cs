using System;
using System.Collections.Generic;

namespace AtlasAI
{
    public interface ICompositeScorer
    {
        //
        // Properties
        //
        List<IContextualScorer> scorers
        {
            get;
        }

        //
        // Methods
        //
        float Score(IAIContext context, List<IContextualScorer> scorers);
    }
}
