namespace Bang
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    using AtlasAI;

    [Serializable]
    public class AgentMoveAI : UtilityAIAssetConfig
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


        public AgentMoveAI(string aiId) : base(aiId) {}


        protected override void ConfigureAI(IUtilityAI ai)
        {
            //  Initializes the setup of AI.
            //InitializeAI(ai);

            //rs.debugScores = true;
            //rs.debugIfDefault = true;







            #region Move to Cover Positiion

            //// ---- New Action ----
            //a = new CompositeAction()
            //{
            //    name = "Move to Cover Positiion",
            //    actions = new List<IAction>()
            //    {
            //        new GetCoverPositions(){ name = "Get Cover Positions" },
            //        new MoveToCover()
            //        {
            //            name = "Move to Cove Position",
            //            scorers = new List<IOptionScorer<Vector3>>()
            //            {
            //                new PositionProximityToSelf() { score = 10, factor = 0.01f },      //  How close each point is to agent.
            //                new OverRangeToClosestEnemy() { desiredRange = 8f, score = 100f },      //  If point is over a certain range to each enemy.
            //            }
            //        }
            //    }
            //};
            //actions.Add(a);  // --  Add to Actions Group

            //// ---- New Scorers Group ----
            //scorers = new List<IContextualScorer>();
            ////
            //// ---- New Scorer ----
            //scorer = new IsDamaged() { score = 0};
            //scorers.Add(scorer);
            //// ---- New Scorer ----
            //scorer = new HasCoverPosition() { score = 50 };
            //scorers.Add(scorer);
            //// ---- New Scorer ----
            //scorer = new HealthBelowThreshold() { score = 15, threshold = 2 };
            //scorers.Add(scorer);
            //// ---- New Scorer ----
            //scorer = new HasEnemiesInRange() { score = 50, range = 10 };
            //scorers.Add(scorer);
            //// ---- New Scorer ----
            //scorer = new AmmoBelowThreshold() { score = 0, threshold = 0.5f };
            //scorers.Add(scorer);
            //// ---- New Scorer ----
            //scorer = new ShouldFindCover() { score = 15 };
            //scorers.Add(scorer);
            ////
            //// ---- Add All Scorers to Scorers Group ----
            //allScorers.Add(scorers.ToArray());

            //// ---- New Qualifier ----
            //q = new CompositeAllOrNothingQualifier() { threshold = 100 };
            ////q = new CompositeScoreQualifier();
            //qualifiers.Add(q);

            #endregion



            #region MoveToBestAttackPosition

            // ---- New Action ----
            a = new MoveToBestPosition()
            {
                scorers = new List<IOptionScorer<Vector3>>()
                {
                    new PositionProximityToSelf() { score = 10, factor = 0.1f },      //  How close each point is to agent.
                    new ProximityToNearestEnemy() { desiredRange = 14f, score = 10f },      //  How close each point is to each enemy.
                    new OverRangeToClosestEnemy() { desiredRange = 5f, score = 100f },      //  If point is over a certain range to each enemy.
                    new LineOfSightToAnyEnemy() { score = 50f },        //  Does each point have line of sight to each enemy.
                    new LineOfSightToClosestEnemy() { score = 50f },    //  Does each point have line of sight to closest enemy.
                    new OverRangeToAnyEnemy() { desiredRange = 5f, score = 50f }          //  If point is over range to any enemy.
                }
            };
            actions.Add(a);  // --  Add to Actions Group

            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorer ----
            scorer = new HasEnemiesInRange() { score = 150, range = 5 };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new IsTargetInSight() { score = 0, not = true };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new IsAttackTargetAlive() { score = 0 };
            scorers.Add(scorer);
            //
            // ---- Add All Scorers to Scorers Group ----
            allScorers.Add(scorers.ToArray());

            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 100 };
            //q = new CompositeScoreQualifier();
            qualifiers.Add(q);

            #endregion



            #region Move In Cover

            //// ---- New Action ----
            //a = new MoveToPeekPosition()
            //{
            //    name = "Move To Peek Position",
            //};
            //actions.Add(a);  // --  Add to Actions Group

            //// ---- New Scorers Group ----
            //scorers = new List<IContextualScorer>();
            ////
            //// ---- New Scorer ----
            //scorer = new IsInCover() { score = 125 };
            //scorers.Add(scorer);
            ////
            //// ---- Add All Scorers to Scorers Group ----
            //allScorers.Add(scorers.ToArray());

            //// ---- New Qualifier ----
            //q = new CompositeAllOrNothingQualifier() { threshold = 100 };
            //qualifiers.Add(q);


            #endregion



            #region SearchForTarget

            // ---- New Action ----
            a = new SearchForTargets();
            actions.Add(a);  // --  Add to Actions Group

            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorer ----
            scorer = new IsSearchingForTargets() { score = 15, not = true };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new HasEnemies() { score = 200, not = true};  //  If agent has no hostiles ,than it scores.
            scorers.Add(scorer);
            //
            // ---- Add All Scorers to Scorers Group ----
            allScorers.Add(scorers.ToArray());

            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 100 };
            //q = new CompositeScoreQualifier();
            qualifiers.Add(q);

            #endregion







            //
            //  Default Qualifier
            //
            rs.defaultQualifier.score = 75;
            rs.defaultQualifier.action = new EmptyAction();




            //  
            //  Configure AI.  Setup each qualifiers action and scorers.
            //
            //base.ConfigureAI();
        }





    }

}

