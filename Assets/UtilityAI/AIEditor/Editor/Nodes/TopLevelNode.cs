namespace AtlasAI.AIEditor
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class Node : ScriptableObject
    {
        public AIUI parentUI{
            get;
            set;
        }

		public void OnEnable()
		{
            hideFlags = HideFlags.HideAndDontSave;
		}
	}




    [Serializable]
    public abstract class TopLevelNode : Node
    {
        public AIUI parent;
        public Rect viewArea;
        //  Guid 
        public string id;


        public bool isSelected{
            get{
                return parent.selectedNode == this;
            }
        }


        public Vector3 ConnectorAnchorIn(NodeSettings settings)
        {
            throw new NotImplementedException();
        }


        public virtual void RecalcHeight(NodeSettings settings)
        {
            viewArea.height = settings.titleHeight + settings.qualifierHeight + settings.actionHeight;
        }



    }


}
