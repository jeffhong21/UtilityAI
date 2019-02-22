namespace AtlasAI
{
    using System;
    using System.Collections.Generic;


    [AICategory("Composite Qualifiers"), FriendlyName("Sum while above threshold", "Scores by summing child scores, until a child scores below the threshold")]
    public class CompositePriorityQualifier : CompositeQualifier
    {
        //
        // Fields
        //
        public float threshold;



        //
        // Constructors
        //
        public CompositePriorityQualifier()
            : base()
        {
        }

        //
        // Methods
        //
        public sealed override float Score(IAIContext context, List<IContextualScorer> scorers)
        {
            float score = 0f;
            float scorerScore = 0f;


            for (int i = 0; i < scorers.Count; i ++)
            {
                var scorer = scorers[i];
                scorerScore = scorer.Score(context);
                if (scorerScore > threshold)
                {
                    score += scorerScore;
                }
                else
                {
                    return score;
                }
            }


            return score;


            //foreach (IContextualScorer scorer in scorers)
            //{
            //    scorerScore = scorer.Score(context);
            //    if (scorerScore > threshold)
            //    {
            //        score += scorerScore;
            //        _score += scorerScore;  //  For CompareTo()
            //    }
            //    else
            //    {
            //        return score;
            //    }
            //}

        }
    }
}