using System;
using UnityEngine;
using UnityEditor;

namespace UtilAI.AIEditor
{



    public class AIUI : ScriptableObject
    {
        public AICanvas canvas;
        public TopLevelNode selectedNode;


        private SelectorNode currentSelector;
        private AIStorage aiStorage;






        public SelectorNode CurrentSelector{
            get { return currentSelector; }
        }



        public static AIUI Create(string name)
        {
            AIUI aiui = CreateInstance<AIUI>();
            aiui.name = name;

            aiui.InitNew(name);
            aiui.InitAI();

            return aiui;
        }


        public static AIUI Load(string aiId)
        {
            AIUI aiui = CreateInstance<AIUI>();
            aiui.InitAI();
            return aiui;
        }


        public void InitAI()
        {
            if (canvas == null) 
                canvas = new AICanvas();

        }


        private void InitNew(string aiName)
        {
            string aiId = aiName;
            //aiStorage = AIStorage.CreateAsset(aiId, aiName);
        }




        public void MarkDirty()
        {
            if (AIEditorWindow.activeInstance != null)
                AIEditorWindow.activeInstance.Repaint();
            if (aiStorage != null)
            {
                //_aiStorage.editorConfiguration = GuiSerializer.Serialize(canvas);
                EditorUtility.SetDirty(aiStorage);
            }

        }




        public SelectorNode AddSelector(Vector2 position)
        {
            SelectorNode selectorNode = SelectorNode.Create(this, typeof(ScoreSelector), position);
            canvas.nodes.Add(selectedNode);
            //selectedNode.name += " " + canvas.nodes.Count;

            //MarkDirty();
            return selectorNode;
        }
    }
}
