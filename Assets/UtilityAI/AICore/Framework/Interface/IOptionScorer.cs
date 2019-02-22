namespace AtlasAI
{
    public interface IOptionScorer <T>
    {
        //IQualifier Qualifier { get; }

        //IQualifierCollection Collection { get;  }
        float Score(IAIContext context, T data);
    }
}