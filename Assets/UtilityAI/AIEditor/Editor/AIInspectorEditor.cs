using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace AtlasAI.AIEditor
{
    [CustomEditor(typeof(AIUI))]
    public class AIInspectorEditor : Editor
    {
        //
        // Static Fields
        //
        public static AIInspectorEditor instance;

        //
        // Fields
        //
        private AIUI _state;

        private SerializedObject serializedNode;


		public override void OnInspectorGUI()
		{
            _state = (AIUI)serializedObject.targetObject;

            if (_state.selectedNode != null)
            {
                if (_state.currentSelector != null)
                {
                    DrawSelectorUI(_state.currentSelector);
                }
                else if (_state.currentQualifier != null)
                {
                    DrawQualifierUI(_state.currentQualifier);
                }
                else
                {
                    EditorGUILayout.LabelField("UTILITY AI INSPECTOR EDITOR", EditorStyling.Skinned.inspectorTitle);
                }
            }
            else
            {
                DrawAIUI(_state);
            }


            EditorUtility.SetDirty(_state);
            serializedObject.ApplyModifiedProperties();
		}


        private void DrawAIUI(AIUI aiui)
        {
            EditorGUILayout.LabelField("UTILITY AI INSPECTOR EDITOR", EditorStyling.Skinned.inspectorTitle);
            EditorGUILayout.HelpBox("This is an AI Editor Window.", MessageType.None);

        }




        private void DrawSelectorUI(SelectorNode selectorNode)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(selectorNode.name + " | UTILITY AI", EditorStyling.Skinned.inspectorTitle);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Change", EditorStyles.miniButton, GUILayout.Width(45), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    AIEntitySelectorWindow.Get<IQualifier>(Event.current.mousePosition, null);
                }
                if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(45), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {

                }
                GUILayout.Space(12);
            }


            selectorNode.name = EditorGUILayout.TextField("Name: ", selectorNode.name);
            EditorGUILayout.LabelField("Description");
            EditorGUILayout.TextArea("", GUILayout.Height(EditorGUIUtility.singleLineHeight * 3));


            GUILayout.Space(8);
            EditorGUILayout.HelpBox(selectorNode.viewArea.ToString(), MessageType.None);
            GUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Qualifiers:");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(EditorStyling.addTooltip, EditorStyling.Skinned.addButtonSmall,
                                     GUILayout.Width(28f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    //AIEntitySelectorWindow.Get<IQualifier>(Event.current.mousePosition, type => _state.AddQualifier(type, _state.currentSelector));
                    AIEntitySelectorWindow.Get<IQualifier>(Event.current.mousePosition,  type => _state.AddQualifier(type, selectorNode ) );
                }
                GUILayout.Space(12);
            }


            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (int i = 0; i < selectorNode.qualifierNodes.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var qualifier = selectorNode.qualifierNodes[i];
                        EditorGUILayout.LabelField(qualifier.friendlyName);

                        if (GUILayout.Button(EditorStyling.deleteTooltip, EditorStyling.Skinned.deleteButtonSmall,
                                             GUILayout.Width(28f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                        {
                            _state.RemoveQualifier(qualifier);
                        }
                        GUILayout.Space(8);
                    }
                }
                EditorGUILayout.LabelField(selectorNode.defaultQualifierNode.friendlyName);
            }



            //DrawListElement<QualifierNode>(selectorNode.qualifierNodes);

        }





        private void DrawListElement<T>(IList listElement)
        {
            ReorderableList list = new ReorderableList(listElement, typeof(T), true, false, false, false);

            list.elementHeight = 28;
            //list.draggable = true;
            list.showDefaultBackground = true;
            list.headerHeight = 0;
            list.footerHeight = 0;

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //GUI.Box(rect, GUIContent.none);
                rect.y += 2;
                EditorGUI.LabelField(rect, list.list[index].GetType().Name);
            };


            list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);
            };


            //list.onSelectCallback = (ReorderableList l) =>
            //{

            //};




            list.DoLayoutList();
        }






        private void DrawQualifierUI(QualifierNode qualifierNode)
        {

        }

	}
}
