namespace AtlasAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;



    /// <summary>
    ///   Selectors select the best Qualifier from the qualifiers attached to the Selector.
    ///   Selector Gets the Highest Score from the list of qualifiers.
    ///   It needs to know all qualifiers attached to it.
    /// </summary>
    public abstract class Selector : ISelect
    {
        public bool debugScores;
        public bool debugIfDefault;


        [SerializeField, HideInInspector]
        protected Guid _id;

        [SerializeField, HideInInspector]
        protected List<IQualifier> _qualifiers;

        [SerializeField, HideInInspector]
        protected IDefaultQualifier _defaultQualifier;


        //  Gets the id of this selector.
        public Guid id{
            get { return _id; }
        }

        //  Gets the qualifiers of this selector.
        public List<IQualifier> qualifiers 
        {
            get{ return _qualifiers; }
            //protected set { _qualifiers = value; }
        }

        //  Gets or sets the default qualifier.
        public IDefaultQualifier defaultQualifier  
        {
            get { return _defaultQualifier; }
            set { _defaultQualifier = value; }
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="T:AtlasAI.Selector"/> class.
        /// </summary>
        public Selector()
        {
            //Debug.Log(string.Format("Selector:  {0} : Abstract Constructor Message", this.GetType().Name));
            _qualifiers = new List<IQualifier>();
            _defaultQualifier = new DefaultQualifier();
            RegenerateId();
        }


        public virtual void CloneFrom(object other)
        {
            var cloneFrom = other as Selector;
            _id = cloneFrom.id;
            _qualifiers = new List<IQualifier>(cloneFrom.qualifiers);
            _defaultQualifier = cloneFrom.defaultQualifier;
        }


        public void RegenerateId()
        {
            _id = Guid.NewGuid();
        }


        /// <summary>
        /// Selects the action for execution.
        /// </summary>
        /// <returns>The action to execute.</returns>
        /// <param name="context">Context.</param>
        public virtual IAction Select(IAIContext context)
        {
            //  Return the action of the selected IQualifier.
            return Select(context, qualifiers, defaultQualifier).action;
        }




        /// <summary>
        ///   This function selects the best score from a list of qualifiers.
        /// </summary>
        public abstract IQualifier Select(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifier);
        //public abstract IQualifier Select(IAIContext context, List<IQualifier> qualifiers, IDefaultQualifier defaultQualifier);











    }






}