using System;

namespace AtlasAI.Visualization
{
    public class ActionVisualizer : IAction, IVisualizedObject
    {
        //
        // Fields
        //
        private IAction _action;

        private IQualifierVisualizer _parent;


        //
        // Properties
        //
        public IAction action{
            get{ return _action; }
        }

        public IQualifierVisualizer parent{
            get { return _parent; }
        }




        public object target{
            get;
        }


        //
        // Constructors
        //
        public ActionVisualizer(IAction action, IQualifierVisualizer parent)
        {
            _action = action;
            _parent = parent;
        }


        //
        // Methods
        //
        public virtual void Execute(IAIContext context, bool doCallback)
        {
            OnExecute(context);
        }


        public void Execute(IAIContext context)
        {
            OnExecute(context);
        }

        public void OnExecute(IAIContext context)
        {
            UnityEngine.Debug.LogFormat("Action Visualizer Executing action");
        }


        public virtual void Init()
        {
            UnityEngine.Debug.LogFormat("Initializing Action Visualizer.");
        }
    }
}
