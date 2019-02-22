namespace AtlasAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// What this does is assign the created AI entities to the correct location.
    /// </summary>
    public abstract class UtilityAIAssetConfig : IUtilityAIAssetConfig
    {
        [HideInInspector]
        public bool debug;

        public string aiId{
            get;
            private set;
        }

        protected Selector rs;
        protected IAction a;
        protected IContextualScorer scorer;
        protected List<IContextualScorer> scorers;
        protected IQualifier q;
        protected Selector s;

        protected List<IQualifier> qualifiers;
        protected List<IContextualScorer[]> allScorers;
        protected List<IAction> actions;



        protected UtilityAIAssetConfig(string aiId)
        {
            this.aiId = aiId;
            qualifiers = new List<IQualifier>();
            allScorers = new List<IContextualScorer[]>();
            actions = new List<IAction>();
        }



        public void CreateAI(IUtilityAI aI)
        {
            if (qualifiers == null) qualifiers = new List<IQualifier>();
            if (allScorers == null) allScorers = new List<IContextualScorer[]>();
            if (actions == null) actions = new List<IAction>();
            //  Set the root selector.
            rs = aI.rootSelector;
            rs.debugScores = debug;
            rs.debugIfDefault = debug;

            //  Setup up the AI entites based on the derived classes configurations.
            ConfigureAI(aI);
            //Setup each qualifiers action and scorers.
            SetupAI();
        }



        /// <summary>
        /// Setup each qualifiers action and scorers.  Called by SetupAI method.
        /// </summary>
        /// <param name="debug">If set to <c>true</c> debug finish.</param>
        protected void SetupAI()
        {
            //if (debug) Debug.Log("ConfigureAI qualifier count: " + qualifiers.Count);
            for (int index = 0; index < qualifiers.Count; index++)
            {
                //  Add qualifier to rootSelector.
                rs.qualifiers.Add(qualifiers[index]);
                var qualifier = rs.qualifiers[index];
                //  Set qualifier's action.
                qualifier.action = actions[index];
                //  Add scorers to qualifier.
                foreach (IContextualScorer contextualScorer in allScorers[index])
                {
                    if (qualifier is CompositeQualifier)
                    {
                        var q = qualifier as CompositeQualifier;
                        q.scorers.Add(contextualScorer);
                    }
                }
            }

            if (debug) Debug.Log("Finish Initializing " + this.GetType().Name);
        }



        protected abstract void ConfigureAI(IUtilityAI ai);


    }

}

