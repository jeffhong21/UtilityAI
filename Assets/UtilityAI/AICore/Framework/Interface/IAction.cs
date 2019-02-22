namespace AtlasAI
{
    
    public interface IAction
    {
        //string name { get; set; }

        void OnExecute(IAIContext context);

        void Execute(IAIContext context);
    }
}