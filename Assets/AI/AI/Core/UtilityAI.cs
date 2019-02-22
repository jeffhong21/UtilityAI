namespace UtilAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;


    public class UtilityAI : ScriptableObject
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _aiName;
        [SerializeField]
        private List<Selector> _selectors;
        [SerializeField]
        private Selector _rootSelector;



        public string id
        {
            get { return _id; }
        }

        public string aiName
        {
            get { return _aiName; }
            set { _aiName = value; }
        }

        public Selector this[int idx]
        {
            get { return _selectors[idx]; }
        }

        public Selector rootSelector
        {
            get { return _rootSelector; }
            set { _rootSelector = value; }
        }

        public int selectorCount
        {
            get { return _selectors.Count; }
        }




        public void AddSelector(Selector s)
        {
            _selectors.Add(s);
        }


        public Selector FindSelector(string id)
        {
            for (int i = 0; i < _selectors.Count; i++){
                if (_selectors[i].id == id){
                    return _selectors[i];
                }
            }
            return null;
        }


        public void RemoveSelector(Selector s)
        {
            for (int index = 0; index < _selectors.Count; index++){
                //  If selector id matches given selector id, remove the selector.
                if (_selectors[index].id == s.id){
                    //  If the root selector is to be removed, replace the rootSelector field with the next selector.
                    if (index == 0){
                        rootSelector = _selectors[1];
                    }
                    _selectors.RemoveAt(index);
                    return;
                }
            }
        }


        public bool ReplaceSelector(Selector current, Selector replacement)
        {
            throw new NotImplementedException();
        }


        public ActionBase Select(IAIContext context)
        {
            return rootSelector.Select(context);
        }
    }





    [Serializable]
    public abstract class Selector : ScriptableObject
    {
        [SerializeField]
        protected string _id;
        [SerializeField]
        protected List<QualifierBase> _qualifiers;
        [SerializeField]
        protected DefaultQualifier _defaultQualifier;


        public string id{
            get { return _id; }
        }

        //  Gets the qualifiers of this selector.
        public List<QualifierBase> qualifiers
        {
            get { return _qualifiers; }
            //protected set { _qualifiers = value; }
        }

        //  Gets or sets the default qualifier.
        public DefaultQualifier defaultQualifier
        {
            get { return _defaultQualifier; }
            set { _defaultQualifier = value; }
        }



        public virtual ActionBase Select(IAIContext context){
            return Select(context, qualifiers, defaultQualifier).Action;
        }

        public abstract QualifierBase Select(IAIContext context, List<QualifierBase> qualifiers, DefaultQualifier defaultQualifier);

    }



    #region -- Qualifiers --

    [Serializable]
    public abstract class QualifierBase
    {
        [SerializeField]
        protected ActionBase m_Action;

        public ActionBase Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        public abstract float GetScore(IAIContext context);
    }


    [Serializable]
    public abstract class CompositeQualifier : QualifierBase
    {
        [SerializeField]
        protected List<ContextualScorerBase> m_Scorers;


        public List<ContextualScorerBase> Scorers{
            get { return m_Scorers; }
            set { m_Scorers = value; }
        }


        public override float GetScore(IAIContext context)
        {
            return GetScore(context, m_Scorers);
        }


        public abstract float GetScore(IAIContext context, List<ContextualScorerBase> scorers);
    }



    [Serializable]
    public class DefaultQualifier : QualifierBase
    {
        [SerializeField]
        protected float m_Score;

        public float Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }


        public override float GetScore(IAIContext context)
        {
            return m_Score;
        }
    }

    #endregion




    #region -- Actions --

    [Serializable]
    public abstract class ActionBase
    {
        
        public abstract void Execute(IAIContext context);
    }



    [Serializable]
    public class CompositeAction : ActionBase
    {
        [SerializeField]
        protected List<ActionBase> m_Actions;

        public List<ActionBase> Action
        {
            get { return m_Actions; }
            set { m_Actions = value; }
        }


        public override void Execute(IAIContext context)
        {
            for (int i = 0; i < m_Actions.Count; i++)
            {
                m_Actions[i].Execute(context);

            }
        }
    }






    #endregion




    #region -- Scorers --

    [Serializable]
    public abstract class ContextualScorerBase
    {
        [SerializeField]
        protected float m_Score;

        public float Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }


        public abstract float GetScore(IAIContext context);
    }


    #endregion
}