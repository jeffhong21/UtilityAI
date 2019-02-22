namespace AtlasAI.AIEditor
{
    using System;


    public sealed class NodeSettings
    {
        public float nodeWidth = 256;
        public float nodeMinWidth = 128f;

        public float dragHandleWidth = 18f;
        public float scoreAreaWidth = 18f;
        public float toggleAreaWidth = 18f;
        public float anchorAreaWidth = 16f;
        public float connectorLineWidth = 2f;


        public float actionHeight
        {
            get { return 24f; }     // 25
        }

        public float qualifierHeight
        {
            get { return 28; }      //  35
        }

        public float scoreHeight
        {
            get { return qualifierHeight; }     // 25
        }

        public float titleHeight
        {
            get { return 36; }  // 40
        }

        public float scale
        {
            get { return 1f; }
        }




    }
}
