namespace AtlasAI
{
    using System;

    public interface IContextProvider 
    { 
        IAIContext GetContext();

        IAIContext GetContext(Guid aiId);
    }

}