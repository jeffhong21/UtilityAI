namespace UtilAI.AIEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using System;
    using System.Collections;



    public class SelectorNode : TopLevelNode
    {
        private const string QualifersProperty = "_qualifiers";
        private const string DefaultQualifierProperty = "_defaultQualifier";


        private Selector selector;
        private Type selectorType;
        private NodeSettings nodeSettings;


        private SerializedObject target;
        private SerializedProperty qualifiers;
        private SerializedProperty defaultQualifier;
        private ReorderableList list;


        private Rect headerRect;
        private Rect contentRect;



        public bool isRoot{
            get { return false; }
        }



        public static SelectorNode Create(AIUI parent, Type selectorType, Vector2 position)
        {
            var node = CreateInstance<SelectorNode>();

            node.parent = parent;
            // Create Selector.
            node.selectorType = selectorType;
            node.nodeSettings = new NodeSettings();
            //Debug.LogFormat("x: {0} | y: {1}| width: {2}| height: {3}", position.x, position.y, node.nodeSettings.nodeWidth, node.nodeSettings.defaultHeight);
            node.viewArea = new Rect(position.x, position.y, node.nodeSettings.nodeWidth, node.nodeSettings.defaultHeight);
            node.headerRect = new Rect(node.viewArea.x, node.viewArea.y, node.viewArea.width, node.nodeSettings.titleHeight); ;
            node.contentRect = new Rect(node.viewArea.x, node.viewArea.y + node.nodeSettings.titleHeight,
                                        node.viewArea.width, node.viewArea.height - node.nodeSettings.titleHeight);

            //node.target = new SerializedObject(node.selector);
            //node.qualifiers = node.target.FindProperty(QualifersProperty);
            //node.defaultQualifier = node.target.FindProperty(DefaultQualifierProperty);
            //node.list = new ReorderableList(node.target, node.qualifiers, true, true, false, false);
            return node;
        }


        public void DrawNode()
        {

            //headerRect.Set(viewArea.x, viewArea.y, viewArea.width, nodeSettings.titleHeight);
            //contentRect.Set(viewArea.x, viewArea.y + nodeSettings.titleHeight, viewArea.width, viewArea.height - nodeSettings.titleHeight);

            //if (target == null)
            //{
            //    target = new SerializedObject(selector);
            //    qualifiers = target.FindProperty(QualifersProperty);
            //    defaultQualifier = target.FindProperty(DefaultQualifierProperty);
            //}

            GUI.Box(viewArea, GUIContent.none);
            //using (new GUI.GroupScope(viewArea))
            //{
            //    //GUI.Box(headerRect, GUIContent.none);
            //    //GUI.Label(headerRect, name, parent.CurrentSelector == this ? EditorStyling.NodeStyles.nodeTitleActive : EditorStyling.NodeStyles.nodeTitle);
            //    GUI.Box(contentRect, GUIContent.none);
            //    //DrawListProperty(contentRect, target, qualifiers);
            //}





        }



        private void DrawListProperty(Rect position, SerializedObject so, SerializedProperty listProperty, bool draggable = true)
        {
            if (list == null)
            {
                list = new ReorderableList(so, listProperty, true, draggable, false, false);
                list.elementHeight = nodeSettings.qualifierHeight;
                list.showDefaultBackground = false;
            }


            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;


                GUI.Label(rect, index.ToString(), EditorStyling.NodeStyles.normalBoxText);
            };

            list.onSelectCallback = (ReorderableList l) =>
            {
                Debug.Log(l.index);
            };


            list.DoList(position);
            so.ApplyModifiedProperties();
        }



        private void DrawListElement(SerializedProperty element)
        {
            //GUI.Label(rect, index.ToString(), EditorStyling.NodeStyles.normalBoxText);
        }


        public override void RecalcHeight(NodeSettings settings)
        {
            viewArea.height = settings.titleHeight + settings.qualifierHeight + settings.actionHeight;
        }

    }


}