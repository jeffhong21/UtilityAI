namespace AtlasAI
{
    using System.Collections.Generic;

    /// <summary>
    /// Composite action.
    /// </summary>
    public sealed class CompositeAction : ActionBase
    {
        //
        // Fields
        //
        private List<IAction> _actions;


        //
        // Properties
        //
        public List<IAction> actions 
        {
            get { return _actions; }
            set { _actions = value; }
        }


        //
        // Constructors
        //
        public CompositeAction()
        {
            _actions = new List<IAction>();
        }





        //
        // Methods
        //
        public override void OnExecute(IAIContext context)
        {
            for (int i = 0; i < actions.Count; i++){
                actions[i].Execute(context);


            }
        }

    }



}