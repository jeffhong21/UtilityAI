namespace Bang
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    using AtlasAI;

    [Serializable]
    public class AgentActionAI : UtilityAIAssetConfig
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



        //public AgentActionAI()
        //{
        //    name = "AgentActionAI";
        //}
        public AgentActionAI(string aiId) : base(aiId) { }


        protected override void ConfigureAI(IUtilityAI ai)
        {
            //  Initializes the setup of AI.
            //InitializeAI(ai);

            //rs.debugScores = true;



            // ---- New Action ----
            a = new ReloadGun();

            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorers ----
            scorer = new IsGunLoaded() { score = 20 };
            scorers.Add(scorer);
            // ---- New Scorers ----
            scorer = new AmmoBelowThreshold() { score = 10, threshold = 0.25f };
            scorers.Add(scorer);

            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 15 };

            // ---- Add all to groups ----
            actions.Add(a);
            allScorers.Add(scorers.ToArray());
            qualifiers.Add(q);

            // ---- End Adding Qualifier ----





            // ---- New Action ----
            a = new CompositeAction()
            {
                actions = new List<IAction>()
                {
                    //new StopMovement(){ name = "StopMovement"},
                    new SetBestAttackTarget()
                    {
                        scorers = new List<IOptionScorer<ActorHealth>>()
                        {
                            new IsTargetAlive(){ score = 100f},
                            new CanSeeTarget(){ score = 50f },
                            new EnemyProximityToSelf(){ multiplier = 1f, score = 50f},
                            new IsCurrentTargetScorer()
                        }
                    },
                    new FireAtAttackTarget()
                }
            };

            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorers ----
            scorer = new HasEnemiesInRange() { score = 15, range = 10 };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new IsTargetInSight() { score = 0 };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new IsAttackTargetAlive() { score = 0 };
            scorers.Add(scorer);

            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 10 };

            // ---- Add all to groups ----
            actions.Add(a);
            allScorers.Add(scorers.ToArray());
            qualifiers.Add(q);

            // ---- End Adding Qualifier ----





            //
            //  Default Qualifier
            //
            rs.defaultQualifier.score = 12;
            rs.defaultQualifier.action = new EmptyAction();


            //  
            //  Configure AI.  Setup each qualifiers action and scorers.
            //
            //base.ConfigureAI();
        }





    }

}

