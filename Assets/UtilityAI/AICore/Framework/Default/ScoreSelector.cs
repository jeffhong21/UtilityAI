namespace AtlasAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    //using System.Diagnostics;


    /// <summary>
    ///   Selector Gets the Highest Score from the list of qualifiers.
    /// </summary>

    [FriendlyName("Highest Score Wins", "The qualifier with the highest score is selected")]
    public class ScoreSelector : Selector
    {
        public string info;
        private string[] scoresInfo;

        public ScoreSelector()
            :base()
        {
        }


        public override IQualifier Select(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifier)
        {
            //  Debug Scorers
            scoresInfo = new string[qualifiers.Count + 1];


            float thisScore;
            float score = 0f;
            IQualifier best = null;

            //  Get score for all qualifiers
            for (int index = 0; index < qualifiers.Count; index++)
            {
                CompositeQualifier q = qualifiers[index] as CompositeQualifier;
                thisScore = q.Score(context, q.scorers);
                if(thisScore > score)
                {
                    score = thisScore;
                    best = q as IQualifier;
                }

                //  Debug Scorers
                if (debugScores)
                    scoresInfo[index] = string.Format("{0} | {1} (<color=#800080ff>{2}</color>)\n", q.GetType().Name, q.action, score);
                
            }

            thisScore = defaultQualifier.Score(context);
            if(thisScore > score)
            {
                score = thisScore;
                best = defaultQualifier as IQualifier;

                if(debugIfDefault) 
                    info = string.Format(" ** Default Action Picked: {0} (<color=#00ff00ff>{1}</color>)\n", best.action, score);
            }
            else
            {
                if (debugScores) 
                    info = string.Format(" - Best Qualifier: {0} (<color=#00ff00ff>{1}</color>)\n", best.action, score);
            }


            //  Debug Scorers
            if (debugScores)
            {
                //  Get Default Qualifiers scorers info.
                scoresInfo[qualifiers.Count] = string.Format("{0} | {1} (<color=#800080ff>{2}</color>)\n", defaultQualifier.GetType().Name, defaultQualifier.action, defaultQualifier.Score(context));


                for (int i = 0; i < scoresInfo.Length; i++){
                    info += scoresInfo[i];
                }
                Debug.Log(info);
            }




            return best;

        }



    }


}