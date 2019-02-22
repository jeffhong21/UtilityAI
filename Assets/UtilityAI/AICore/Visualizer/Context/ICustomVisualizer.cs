namespace AtlasAI.Visualization
{
    using System;

    public interface ICustomVisualizer
    {
        void EntityUpdate(object aiEntity, IAIContext context, Guid aiId);

        //void EntityUpdate(System.Object aiEntity, IAIContext context, System.Guid aiID);

    }
}

