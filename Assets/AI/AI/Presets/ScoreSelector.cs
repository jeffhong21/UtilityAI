using System;
using System.Collections.Generic;
using UnityEngine;


namespace UtilAI
{
    public class ScoreSelector : Selector
    {
        public override QualifierBase Select(IAIContext context, List<QualifierBase> qualifiers, DefaultQualifier defaultQualifier)
        {
            float score;
            float highestScore = 0f;
            float defaultScore = defaultQualifier.GetScore(context);
            QualifierBase qualifier = null;

            //  Get score for all qualifiers
            for (int index = 0; index < qualifiers.Count; index++)
            {
                CompositeQualifier compositeQualifier = qualifiers[index] as CompositeQualifier;
                score = compositeQualifier.GetScore(context, compositeQualifier.Scorers);
                if (score > highestScore){
                    highestScore = score;
                    qualifier = compositeQualifier as QualifierBase;
                }
            }


            if (defaultScore > highestScore){
                highestScore = defaultScore;
                qualifier = defaultQualifier as QualifierBase;
            }

            return qualifier;
        }
    }
}
