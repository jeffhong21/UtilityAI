namespace AtlasAI
{

    public interface IContextualScorer
    {
        float Score(IAIContext context);
    }
}