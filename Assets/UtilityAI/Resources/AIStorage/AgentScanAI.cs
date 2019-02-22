namespace Bang
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    using AtlasAI;

    [Serializable]
    public class AgentScanAI : UtilityAIAssetConfig
    {
        
        /*
        private IAction a;
        private IContextualScorer scorer;
        private List<IContextualScorer> scorers;
        private IQualifier q;
        private Selector s;

        private List<IQualifier> qualifiers;
        private List<IContextualScorer[]> allScorers;
        private List<IAction> actions;
        */


        //public AgentScanAI()
        //{
        //    name = "AgentScanAI";
        //}
        public AgentScanAI(string aiId) : base(aiId) { }


        protected override void ConfigureAI(IUtilityAI ai)
        {
            //  Initializes the setup of AI.
            //InitializeAI(ai);



            //// ---- New Action ----
            //a = new CompositeAction()
            //{
            //    name = "Scan",
            //    actions = new List<IAction>()
            //    {
            //        new ScanForEntities(){name = "ScanForEntitites"},
            //        new ScanForPositions(){ name = "ScanForPositions", samplingRange = 20, samplingDensity = 2.5f }
            //    }
            //};
            //actions.Add(a);  // --  Add to Actions Group
            //
            //// ---- New Scorers Group ----
            //scorers = new List<IContextualScorer>();
            ////
            //// ---- New Scorer ----
            //scorer = new FixedScorer() { score = 5 };
            //scorers.Add(scorer);
            ////
            //// ---- Add All Scorers to Scorers Group ----
            //allScorers.Add(scorers.ToArray());
            //
            //// ---- New Qualifier ----
            //q = new CompositeScoreQualifier();
            //qualifiers.Add(q);



            //
            //  Default Qualifier
            //
            rs.defaultQualifier.action = new CompositeAction()
            {
                actions = new List<IAction>()
                {
                    new ScanForEntities(),
                    new ScanForPositions(){samplingRange = 20, samplingDensity = 2.5f }
                }
            };




            //  
            //  Configure AI.  Setup each qualifiers action and scorers.
            //
            //base.ConfigureAI();
        }






    }

}

