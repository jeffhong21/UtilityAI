namespace AtlasAI
{
    using System;


    public class AILinkAction : IAction
    {
        private Guid _aiId;

        private ISelect _linkedAI;

        //
        // Properties
        //
        public string name{
            get;
            set;
        }

        public Guid aiId{
            get { return _aiId; }
            set { _aiId = value; }
        }


        public IUtilityAI linkedAI{
            get { return (IUtilityAI)_linkedAI; }
        }


        //
        // Constructors
        //
        public AILinkAction(Guid aiId){
            _aiId = aiId;
        }

        public void Execute(IAIContext context)
        {
            OnExecute(context);
        }

        public void OnExecute(IAIContext context)
        {
            _linkedAI.Select(context);
        }

    }


}