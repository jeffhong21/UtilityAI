using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;

namespace UtilAI.AIEditor
{
    public class TopLevelNode : ScriptableObject
    {
        public AIUI parent;
        public Rect viewArea;
        public string id;



        public string friendlyName{
            get { return name; }
        }

        public bool isSelected{
            get { return parent.selectedNode == this; }
        }




        public virtual void RecalcHeight(NodeSettings settings)
        {
            viewArea.height = settings.titleHeight + settings.qualifierHeight + settings.actionHeight;
        }

    }





    public class NodeSettings
    {
        public float nodeWidth = 256;
        public float nodeMinWidth = 128;


        public float actionHeight{
            get { return EditorGUIUtility.singleLineHeight; }
        }

        public float qualifierHeight{
            get { return EditorGUIUtility.singleLineHeight; }
        }

        public float scoreHeight{
            get { return EditorGUIUtility.singleLineHeight; }
        }

        public float titleHeight{
            get { return 36; }  // 40
        }

        public float scale{
            get { return 1f; }
        }


        public float defaultHeight
        {
            get { return 128; }  // 40
        }

        //public float defaultHeight
        //{
        //    get { return titleHeight + qualifierHeight + actionHeight; }  // 40
        //}
    }
}
