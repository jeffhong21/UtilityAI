namespace AtlasAI
{
    using System;

    public abstract class ActionBase : IAction
    {
        //public event Action OnExecuteComplete;


        public ActionBase(){
            //Debug.Log(actionStatus);
        }



        public void Execute(IAIContext context)
        {
            OnExecute(context);

            //if(OnExecuteComplete != null){
            //    OnExecuteComplete();
            //}
        }


        public abstract void OnExecute(IAIContext context);

    }


}