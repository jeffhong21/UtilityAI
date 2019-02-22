namespace AtlasAI
{
    using UnityEngine;
    using System;

    [Serializable]
    public abstract class ContextualScorerBase : IContextualScorer
    {
        public float score;


        protected ContextualScorerBase(){

        }

        public abstract float Score(IAIContext context);

    }
}