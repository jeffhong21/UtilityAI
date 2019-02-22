namespace Bang
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    using AtlasAI;

    [Serializable]
    public class DeathMatchAI : UtilityAIAssetConfig
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


        //public DeathMatchAI()
        //{
        //    name = "DeathMatchAI";
        //}
        public DeathMatchAI(string aiId) : base(aiId) { }


        protected override void ConfigureAI(IUtilityAI ai)
        {
            //  Initializes the setup of AI.
            //InitializeAI(ai);
            rs.debugScores = true;





            #region Damaged


            a = new CompositeAction()
            {
                actions = new List<IAction>()
                {
                    new MoveToCover()
                    {
                        scorers = new List<IOptionScorer<Vector3>>(){
                            new PositionProximityToSelf() { score = 10, factor = 0.01f },      //  How close each point is to agent.
                            new OverRangeToClosestEnemy() { desiredRange = 5f, score = 100f },      //  If point is over a certain range to each enemy.
                        }
                    }
                }
            };


            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorer ----
            scorer = new HasEnemiesInRange() { score = 50, range = 10 };
            scorers.Add(scorer);
            //


            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 100 };
            //q = new CompositeScoreQualifier();

            actions.Add(a);                             // --  Add to Actions Group
            allScorers.Add(scorers.ToArray());          // ---- Add All Scorers to Scorers Group ----
            qualifiers.Add(q);

            // ---- End Adding Qualifier ----

            #endregion



            #region SearchForHealth


            #endregion



            #region CanSee


            #endregion



            #region MoveToBestAttackPosition

            // ---- New Action ----
            a = new CompositeAction()
            {
                actions = new List<IAction>()
                {
                    new ScanForPositions(){samplingRange = 20, samplingDensity = 2.5f },
                    new MoveToBestPosition()
                    {
                        scorers = new List<IOptionScorer<Vector3>>()
                        {
                            new PositionProximityToSelf(),      //  How close each point is to agent.
                            new ProximityToNearestEnemy(),      //  How close each point is to each enemy.
                            new OverRangeToClosestEnemy(),      //  If point is over a certain range to each enemy.
                            new LineOfSightToAnyEnemy(),        //  Does each point have line of sight to each enemy.
                            new LineOfSightToClosestEnemy(),    //  Does each point have line of sight to closest enemy.
                            new OverRangeToAnyEnemy()           //  If point is over range to any enemy.
                        }
                    }
                }
            };
            actions.Add(a);  // --  Add to Actions Group

            // ---- New Scorers Group ----
            scorers = new List<IContextualScorer>();
            //
            // ---- New Scorer ----
            scorer = new HasEnemiesInRange() { score = 22, range = 8 };
            scorers.Add(scorer);
            //
            // ---- Add All Scorers to Scorers Group ----
            allScorers.Add(scorers.ToArray());

            // ---- New Qualifier ----
            q = new CompositeScoreQualifier();
            //q = new CompositeScoreQualifier();
            qualifiers.Add(q);

            #endregion



            #region Attack

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
            scorer = new HasEnemies() { score = 5 };
            scorers.Add(scorer);
            // ---- New Scorer ----
            scorer = new HasEnemiesInRange() { score = 10, range = 10 };
            scorers.Add(scorer);

            // ---- New Qualifier ----
            q = new CompositeAllOrNothingQualifier() { threshold = 10 };

            // ---- Add all to groups ----
            actions.Add(a);
            allScorers.Add(scorers.ToArray());
            qualifiers.Add(q);

            // ---- End Adding Qualifier ----

            #endregion



            #region SearchForLostTarget
            //

            #endregion



            #region IsAmmoLow

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

            #endregion



            #region Default-Scan
            //
            //  Default Qualifier
            //
            rs.defaultQualifier.action = new ScanForEntities();
            #endregion


            //  
            //  Configure AI.  Setup each qualifiers action and scorers.
            //
            //base.ConfigureAI();
        }






    }

}

