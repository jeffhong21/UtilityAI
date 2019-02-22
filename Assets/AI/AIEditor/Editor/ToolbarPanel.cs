namespace UtilAI.AIEditor
{
    using System;
    using UnityEngine;
    using UnityEditor;

    public class ToolbarPanel
    {
        public delegate void OnNewAI();
        public event OnNewAI OnNewAIHandle = delegate { };


        private AIEditorWindow editor;
        private Rect rect;




        public ToolbarPanel(AIEditorWindow aiEditor, float height)
        {
            editor = aiEditor;
            rect = new Rect(0, 0, editor.position.width, height);

        }


        public void Draw(GUIContent headerTitle)
        {


            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(EditorStyling.newTooltip, EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    NewAction();
                }
                if (GUILayout.Button(EditorStyling.loadTooltip, EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    LoadAction();
                }

                //  Draw current loaded AI.
                //  Draw current loaded AI.
                GUILayout.FlexibleSpace();
                GUILayout.Label(headerTitle, EditorStyling.Skinned.boldTitle);
                GUILayout.FlexibleSpace();


            }
            GUILayout.EndArea();
        }





        private void NewAction()
        {




            OnNewAIHandle();
        }


        private void LoadAction()
        {
            
        }

    }
}
