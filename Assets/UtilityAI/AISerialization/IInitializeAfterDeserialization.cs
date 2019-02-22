namespace AtlasAI.Serialization
{

    public interface IInitializeAfterDeserialization
    {
        
        void InitializeAfterDeserialization(object rootObject);

    }
}