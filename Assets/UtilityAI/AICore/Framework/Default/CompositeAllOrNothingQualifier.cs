namespace AtlasAI
{

    using System;
    using System.Collections.Generic;


    [AICategory("Composite Qualifiers"), FriendlyName("All or Nothing", "Only scores if all child scorers score above the threshold.")]
    public class CompositeAllOrNothingQualifier : CompositeQualifier
    {

        public float threshold;

        //
        // Constructors
        //
        public CompositeAllOrNothingQualifier()
            : base()
        {
        }

        //
        // Methods
        //
        public override float Score(IAIContext context, List<IContextualScorer> scorers)
        {
            var score = 0f;
            if (scorers.Count == 0)
                return score;


            for (int i = 0; i < scorers.Count; i++){
                score += scorers[i].Score(context);
            }


            if(score >= threshold)
            {
                return score;
            }

            return 0f;
        }
    }
}
