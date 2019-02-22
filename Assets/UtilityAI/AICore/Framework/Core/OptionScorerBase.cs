namespace AtlasAI
{
    using UnityEngine;

    /// <summary>
    /// Option scorer base.  This generates a score for ActionWithOptions and generates a score for an indivual option.
    /// </summary>
    public abstract class OptionScorerBase<T> : IOptionScorer<T>
    {

        protected OptionScorerBase()
        {
            
        }


        public abstract float Score(IAIContext context, T data);

    }



}