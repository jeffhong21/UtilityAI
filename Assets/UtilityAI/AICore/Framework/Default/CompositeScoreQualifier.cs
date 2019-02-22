namespace AtlasAI
{

    using System.Collections.Generic;



    [AICategory ("Composite Qualifiers"), FriendlyName ("Sum of Children", "Scores by summing the score of all child scorers.")]
    public class CompositeScoreQualifier : CompositeQualifier
    {



        //
        // Constructors
        //
        public CompositeScoreQualifier()
            : base()
        {
        }


        public override float Score(IAIContext context, List<IContextualScorer> scorers)
        {
            var score = 0f;
            if (scorers.Count == 0)
                return score;
            
            //Debug.Log(this.GetType().ToString() + " is scoring");

            foreach (IContextualScorer scorer in scorers){
                score += scorer.Score(context);
            }

            return score;
        }
    }











}

