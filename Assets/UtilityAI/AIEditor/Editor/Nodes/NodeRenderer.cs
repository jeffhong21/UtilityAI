using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


namespace AtlasAI.AIEditor
{
    public class NodeRenderer
    {
        private NodeSettings nodeSettings;
        private NodeLayout nodeLayout;

        ReorderableList list;
        bool useReorderableList = false;



        public NodeRenderer(NodeSettings settings)
        {
            nodeSettings = settings;
        }

        //
        // Methods
        //
        public void DrawNodes(SelectorNode selectorNode, NodeLayout layout)
        {
            DrawSelectorUI(selectorNode, layout);
            //DrawSelectorNode(selectorNode, layout);
        }



        private void DrawSelectorNode(SelectorNode selectorNode, NodeLayout layout)
        {
            Rect nodeRect = new Rect(selectorNode.viewArea);
            Rect headerRect = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, layout.titleHeight);
            Rect contentRect = new Rect(nodeRect.x, nodeRect.y + layout.titleHeight, 
                                        nodeRect.width, nodeRect.height - layout.titleHeight);

            SerializedObject serializedNode = new SerializedObject(selectorNode);


            //  Header.
            GUI.Box(headerRect, GUIContent.none, EditorStyling.Canvas.normalHeader);
            if (selectorNode.isSelected)
                GUI.Label(headerRect, selectorNode.friendlyName, EditorStyling.NodeStyles.nodeTitleActive);
            else
                GUI.Label(headerRect, selectorNode.friendlyName, EditorStyling.NodeStyles.nodeTitle);

            GUI.Box(contentRect, GUIContent.none, EditorStyling.Canvas.normalSelector);
            DrawListElement(serializedNode, contentRect);
            ////  Body.
            //using (new GUI.GroupScope(contentRect))
            //{
            //    // Begin the body frame around the Node
            //    contentRect.position = Vector2.zero;
            //    using (new GUILayout.AreaScope(contentRect))
            //    {
            //        GUI.Box(contentRect, GUIContent.none, EditorStyling.Canvas.normalSelector);
            //        DrawListElement(serializedNode, contentRect);

            //    }
            //}
        }


        private void DrawListElement(SerializedObject so, Rect pos)
        {
            list = new ReorderableList(so, so.FindProperty("qualifierNodes"), true, false, false, false);

            //list.onReorderCallback += new ReorderableList.ReorderCallbackDelegate()
            list.elementHeight = nodeSettings.qualifierHeight;
            list.draggable = true;
            list.showDefaultBackground = false;
            list.headerHeight = 0;
            list.footerHeight = 0;


            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                //SerializedObject serializedElement = 
                rect.y += 2;
                //EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight),
                                        //element.FindPropertyRelative("name"), GUIContent.none);


                //SerializedProperty elementName = element.FindPropertyRelative("name");
                GUI.Box(rect, GUIContent.none, EditorStyling.Canvas.normalQualifier);
                GUI.Label(rect, index.ToString(), EditorStyling.NodeStyles.normalBoxText);
            };


            list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //SerializedProperty list_element = list.serializedProperty.GetArrayElementAtIndex(index);
            };


            list.onSelectCallback = (ReorderableList l) =>
            {
                Debug.Log(l.index);
            };


            list.DoList(pos);
            //list.DoLayoutList();
            so.ApplyModifiedProperties();
        }


















        private void DrawSelectorUI(SelectorNode selectorNode, NodeLayout layout)
        {
            selectorNode.RecalcHeight(nodeSettings);
            Rect nodeRect = selectorNode.viewArea;

            nodeRect.height = layout.titleHeight;
            GUI.Box(nodeRect, GUIContent.none, EditorStyling.Canvas.normalHeader);
            GUI.Label(nodeRect, selectorNode.friendlyName, selectorNode.isSelected ? EditorStyling.NodeStyles.nodeTitleActive : EditorStyling.NodeStyles.nodeTitle);

            nodeRect.y = nodeRect.y + layout.titleHeight;
            nodeRect.height = selectorNode.viewArea.height - layout.titleHeight;
            // Begin the body frame around the Node
            using (new GUI.GroupScope(nodeRect))
            {
                nodeRect.position = Vector2.zero;
                using (new GUILayout.AreaScope(nodeRect))
                {
                    GUI.Box(nodeRect, GUIContent.none, EditorStyling.Canvas.normalSelector);
                    GUI.changed = false;

                    Vector2 pos = new Vector2(nodeRect.x, nodeRect.y);
                    float elementHeight = nodeSettings.qualifierHeight + nodeSettings.actionHeight;

                    if (selectorNode.qualifierNodes.Count > 0)
                    {
                        for (int i = 0; i < selectorNode.qualifierNodes.Count; i++)
                        {
                            pos.y = i * elementHeight;
                            DrawCompleteQualifier(pos, nodeRect.width, selectorNode.qualifierNodes[i], layout);
                        }
                    }

                    pos.y = selectorNode.qualifierNodes.Count * elementHeight;  //  + settings.actionHeight
                    // Draw Node contents
                    DrawCompleteQualifier(pos, nodeRect.width, selectorNode.defaultQualifierNode, layout);
                    selectorNode.RecalcHeight(nodeSettings);
                }
            }
        }


        private void DrawCompleteQualifier(Vector2 pos, float totalWidth, QualifierNode qualifierNode, NodeLayout layout)
        {

            //Rect dragAreaRect = GetDragAreaLocal(pos.x, pos.y);
            Rect qualifierRect = layout.GetContentAreaLocal(pos.x, pos.y, nodeSettings.qualifierHeight);
            //Rect scoreAreaRect;
            //Rect toggleAreaRect;
            Rect actionRect = layout.GetContentAreaLocal(pos.x, pos.y + nodeSettings.actionHeight, nodeSettings.actionHeight);
            Rect anchorRect = layout.GetAnchorAreaLocal(pos.x, pos.y);


            GUI.Box(qualifierRect, GUIContent.none, EditorStyling.Canvas.normalQualifier);
            GUI.Label(qualifierRect, qualifierNode.friendlyName, EditorStyling.NodeStyles.normalBoxText);

            GUI.Box(actionRect, GUIContent.none, EditorStyling.Canvas.normalAction);
            GUI.Label(actionRect, "Action Name", EditorStyling.NodeStyles.normalBoxText);
            GUI.Label(anchorRect, EditorStyling.ConnectorOpen);


        }







        //private void DrawListElement<T>(IList list, Rect pos)
        //{
        //    list = new ReorderableList(list, typeof(T), true, false, false, false);

        //    //list.onReorderCallback += new ReorderableList.ReorderCallbackDelegate()
        //    list.elementHeight = nodeSettings.qualifierHeight;
        //    list.showDefaultBackground = false;
        //    list.headerHeight = 0;
        //    list.footerHeight = 0;


        //    list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        //    {
        //        GUI.Box(rect, GUIContent.none, EditorStyling.Canvas.normalQualifier);
        //        GUI.Label(rect, list.list[index].ToString(), EditorStyling.NodeStyles.normalBoxText);
        //    };


        //    list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        //    {
        //        //SerializedProperty list_element = list.serializedProperty.GetArrayElementAtIndex(index);
        //    };


        //    list.onSelectCallback = (ReorderableList l) =>
        //    {

        //    };


        //    list.DoList(pos);
        //}








    }
}