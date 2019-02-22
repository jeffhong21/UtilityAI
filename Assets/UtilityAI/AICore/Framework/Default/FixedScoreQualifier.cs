namespace AtlasAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Simply has a fixed score.  So whatever score is set, it will always return that score.
    /// </summary>
    [FriendlyName("Fixed Score", "Always scores a fixed score.")]
    public class FixedScoreQualifier : QualifierBase
    {
        [SerializeField]
        public float score = 2f;



        public override float Score(IAIContext context)
        {
            return score;
        }
    }





}