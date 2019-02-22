namespace AtlasAI
{
    
    /// <summary>
    ///   The Qualifiers job is to get just a score.  
    /// </summary>
    public abstract class QualifierBase : IQualifier, ICanBeDisabled
    {
        public bool isDisabled 
        {
			get;
			set;
		}
        
        public IAction action
        { 
            get;
            set;
        }


        //  Generates a score from all Scorers.
        public abstract float Score(IAIContext context);

    }





}