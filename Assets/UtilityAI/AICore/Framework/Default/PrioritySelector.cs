namespace AtlasAI
{
    using System;
    using System.Collections.Generic;


    [FriendlyName("First Score Wins", "The first qualifier to score above the score of the Default Qualifier, is selected.")]
    public class PrioritySelector : Selector
    {


        //
        // Constructors
        //
        public PrioritySelector()
            : base()
        {
        }

        //
        // Methods
        //
        public override IQualifier Select(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifier)
        {
            var threshold = defaultQualifier.Score(context);

            //  Get score for all qualifiers
            for (int index = 0; index < qualifiers.Count; index++)
            {
                CompositeQualifier q = qualifiers[index] as CompositeQualifier;
                var score = q.Score(context, q.scorers);

                if(score > threshold)
                {
                    return qualifiers[index];
                }
            }


            if (debugIfDefault) 
                UnityEngine.Debug.LogFormat("{0} did not score higher than default qualifier.", this.GetType().Name);


            return defaultQualifier as IQualifier;
        }
    }
}