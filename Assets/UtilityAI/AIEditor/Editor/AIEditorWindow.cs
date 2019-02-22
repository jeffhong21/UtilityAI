using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using AtlasAI.Visualization;

namespace AtlasAI.AIEditor
{
    public class AIEditorWindow : EditorWindow
    {
        //
        // Static Fields
        //
        private static GameObject _lastSelectedGameObject;
        private static readonly Color _majorGridColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        private static readonly Color _minorGridColor = new Color(0.5f, 0.5f, 0.5f, 0.4f);
        private static readonly Color _gridBackground = Color.gray;
        private static readonly Color _connectorLineColor = Color.white;
        private static readonly Color _connectorLineActiveColor = Color.green;
        private const float _curveCurvature = 50f;
        private const float _minimumDragDelta = 25f;
        private const float _saveAsWindowHeight = 70f;
        private const float _saveAsWindowWidth = 220f;
        public static AIEditorWindow activeInstance;

        //
        // Fields
        //
        private GameObject visualizedEntity;
        private GUIContent _title = new GUIContent();               //  Used for Current AI in toolbar.
        private NodeSettings nodeSettings;
        private NodeRenderer nodeRenderer;
        private MouseState mouseState;
        private DragData drag;
        private Vector2 dragPos;
        private AIUI aiui;
        private bool focused;

        [HideInInspector, SerializeField]
        private string _lastOpenAI;
        private float _topPadding = 28;
        private float _footerPadding = 28;


        //
        // Properties
        //

        private float topPadding{
            get { 
                return _topPadding; }
        }

        private float footerPadding{
            get{ return _footerPadding; }
        }


        //
        // Static Methods
        //
        [MenuItem("UtilityAI/AI Editor Window")]
        public static AIEditorWindow AIEditor()
        {
            //  Initialize window
            activeInstance = GetWindow<AIEditorWindow>();
            activeInstance.minSize = new Vector2(400, 200);
            Texture2D _icon = Resources.Load<Texture2D>("Icon_Light"); 
            activeInstance.titleContent = new GUIContent("AI Editor", _icon);
            //  Show window
            activeInstance.Show();
            return activeInstance;
        }




        //
        // Methods
        //
        private GUIContent DoLabel(string text){
            return new GUIContent(text);
        }

        private GUIContent DoLabel(string text, string tooltip){
            return new GUIContent(text, tooltip);
        }

        private GUIContent GetAITitle(){
            if (_title == null) _title = new GUIContent();

            if(string.IsNullOrEmpty(_lastOpenAI) == false){
                _title.text = _lastOpenAI;

            }
            return _title;
        }



		private void OnEnable()
		{
            Reload();

            activeInstance = this;
            if (nodeSettings == null) nodeSettings = new NodeSettings();
            if (nodeRenderer == null) nodeRenderer = new NodeRenderer(nodeSettings);
            if (mouseState == null) mouseState = new MouseState();
            if (drag == null) drag = new DragData(this);

            if (string.IsNullOrEmpty(_lastOpenAI) == false) 
                Load(_lastOpenAI, true);
            else{
                aiui = AIUI.Create("DefaultAI");
                Selection.activeObject = aiui;
                Editor newInspector = Editor.CreateEditor(aiui);
                if (newInspector != null)
                {
                    AIInspectorEditor.instance = newInspector as AIInspectorEditor;
                    AIInspectorEditor.instance.Repaint();
                }
            }
		}


		private void OnDisable()
		{

		}

		private void OnDestroy()
		{
            if (aiui != null)
                DestroyImmediate(aiui);
		}


		private void OnGUI()
        {
            Matrix4x4 m = GUI.matrix;
            //  Draw the Editor Window.
            DrawBackgroundGrid();
            DrawToolbar();
            DrawFooter();


            if(aiui != null){
                //  Draw the nodes and connections.
                DrawNodes(GetViewport());
                DrawActionConnections(GetViewport());
                //  Process all the events.
                DrawUI();
            }



            GUI.matrix = m;
            if (GUI.changed) Repaint();
        }


        #region Draw Editor

        private void DrawBackgroundGrid()
        {
            Color _oldColor = Handles.color;
            Handles.color = _majorGridColor;
            DrawGridLines(position.width, position.height, 100);
            Handles.color = _minorGridColor;
            DrawGridLines(position.width, position.height, 20);
            Handles.color = _oldColor;
            //DrawGrid(position, 1, aiui == null ? Vector2.zero : aiui.canvas.offset);
        }


        private void DrawGridLines(float width, float height, float size)
        {
            int widthDivs = Mathf.CeilToInt(width / size);
            int heightDivs = Mathf.CeilToInt(height - topPadding - footerPadding / size);

            Handles.BeginGUI();

            Vector2 panOffset = aiui.canvas == null ? Vector2.zero : (aiui.canvas.offset + dragPos) * 0.5f;
            Vector3 newOffset = new Vector3(panOffset.x % size, panOffset.y % size, 0);

            // Draws the vertical lines.
            for (int i = 0; i < widthDivs; i++){
                Vector3 p1 = new Vector3(size * i, topPadding, 0) + newOffset;                          // (100 * 1, 18)
                Vector3 p2 = new Vector3(size * i, height - footerPadding, 0f) + newOffset;    //  (100 * i, 400
                Handles.DrawLine(p1, p2);
            }
            //  Draws the horizontal lines.
            for (int i = 0; i < heightDivs; i++){
                Vector3 p1 = new Vector3(-size, size * i, 0) + newOffset;                   //  (-100, 100 * 1)
                Vector3 p2 = new Vector3(width, size * i, 0f)  + newOffset;        //  (350, 100 * 1)
                Handles.DrawLine(p1, p2);
            }
            Handles.EndGUI();
        }



        private void DrawToolbar()
        {
            Rect rect = new Rect(0, 0, position.width, topPadding);
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(DoLabel("New", "Creates a new AI"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    Debug.Log("Creates a new AI");
                }
                if (GUILayout.Button(DoLabel("Load", "Loads a saved AI"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    StoredAIs.Refresh();
                    if (StoredAIs.AIs != null)
                    {
                        GenericMenu storedAIs = new GenericMenu();
                        for (int i = 0; i < StoredAIs.AIs.Count; i++)
                        {
                            string aiId = StoredAIs.AIs[i].aiId;
                            storedAIs.AddItem(new GUIContent(aiId), false, () => Load(aiId, true));
                        }
                        storedAIs.ShowAsContext();
                    }
                }
                if (GUILayout.Button(DoLabel("Save", "Saves current AI"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    Save(_lastOpenAI);
                }



                //  Draw current loaded AI.
                GUILayout.FlexibleSpace();
                GUILayout.Label(GetAITitle(), EditorStyling.Skinned.boldTitle);
                GUILayout.FlexibleSpace();

                //if (aiui != null)
                //{
                //    //  Window Rect
                //    GUILayout.Space(5);
                //    GUILayout.FlexibleSpace();
                //    Rect windowRect = GetViewport();
                //    GUILayout.Label(string.Format("Canvas offset: {0}", aiui.canvas.offset));
                //}


            }
            GUILayout.EndArea();
        }

        bool showBuildingblocksOption;
        private void DrawFooter()
        {
            Rect rect = new Rect(0, position.height - footerPadding + 10, position.width, footerPadding);
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(DoLabel("Refresh", "Refreshes the ai editor.  Also refreshes all StoredAIs"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    Debug.Log(StoredAIs.GetById(_lastOpenAI));
                    //Repaint();
                    //Debug.Log(AIBuildingBlocks.Instance.GetForType(typeof(QualifierBase)));
                }

                if (GUILayout.Button(DoLabel("Clear", "Clear all nodes from the graph."), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    aiui.canvas.nodes.Clear();
                }

                showBuildingblocksOption = GUILayout.Toggle(showBuildingblocksOption, GUIContent.none);
                if(showBuildingblocksOption)
                {
                    if (GUILayout.Button(DoLabel("Selector"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                        AIEntitySelectorWindow.Get<Selector>(Event.current.mousePosition, null);
                    if (GUILayout.Button(DoLabel("IQualifier"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                        AIEntitySelectorWindow.Get<IQualifier>(Event.current.mousePosition, null );
                }



                //  Number of Nodes.
                if (aiui != null)
                {
                    GUILayout.Space(5);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(string.Format("| Selected Node: {0} ", aiui.selectedNode == null ? "< None >" : aiui.selectedNode.name));
                    GUILayout.Label(string.Format(" {0} ", aiui.currentSelector == null ? "" : aiui.currentSelector.viewArea.position.ToString()));
                }


                GUILayout.FlexibleSpace();
                if (GUILayout.Button(DoLabel("Close", "Close window"), EditorStyles.toolbarButton, GUILayout.Width(88)))
                {
                    activeInstance.Close();
                }
            }
            GUILayout.EndArea();
        }


        #endregion



        #region Draw Nodes


        private void DrawConnectionCurve(Vector3 startPos, Vector3 endPos, bool visualizedSelected)
        {
            Vector3 startTan = startPos + Vector3.left * _curveCurvature;
            Vector3 endTan = endPos - Vector3.left * _curveCurvature;
            Handles.DrawBezier( startPos, endPos, startTan, endTan, _connectorLineColor, null, 2f );

            GUI.changed = true;
        }





        private void DrawNodes(Rect viewPort)
        {
            using (new GUILayout.AreaScope(viewPort))
            {
                foreach (SelectorNode node in aiui.canvas.selectorNodes)
                {
                    //DrawSelectorUI(node, nodeSettings);
                    nodeRenderer.DrawNodes(node, new NodeLayout(node, nodeSettings));
                }
            }
        }

        private void DrawActionConnections(Rect viewPort)
        {

        }

        #endregion


        private void DrawUI()
        {
            if (aiui == null) return;

            Event evt = Event.current;
            //  Update MosueState.
            mouseState.Update(evt);
            Vector2 offset = new Vector2(evt.mousePosition.x, evt.mousePosition.y - nodeSettings.titleHeight + 4);
            TopLevelNode node = aiui.canvas.NodeAtPosition(offset);
            //  Mouse button is down.
            if (mouseState.isMouseDown)
            {
                
                if (node != null)
                {
                    var layout = new NodeLayout((SelectorNode)node, nodeSettings);
                    if(layout.InTitleArea(evt.mousePosition))
                    {
                        drag.StartNodeDrag(node, evt.mousePosition);
                    }
                    else
                    {
                        var hitInfo = layout.GetQualifierAtPosition(evt.mousePosition);
                        if(hitInfo.InAnchorArea){
                            Debug.Log("In anchor area.");

                        }
                        else if (hitInfo.qualifier != null){
                            Debug.Log(hitInfo.qualifier.friendlyName);
                        }

                    }
                    //  If anchor is selected, start Connection.

                    //  If DragHandle is selected.

                    //  
                    //drag.StartNodeDrag(node, evt.mousePosition);
                    GUI.changed = true;
                }
                //  Mouse down on the canvas background enables panning.
                else {
                    drag.StartPan(evt.mousePosition);
                }
            }
            //  LMB button is dragging.
            if (mouseState.isMouseDrag && mouseState.isLeftButton)
            {
                if(drag.isDragging){
                    drag.DoDrag(evt.delta);
                    GUI.changed = true;
                }
            }
            //  Mouse button is up.
            if (mouseState.isMouseUp)
            {
                //  Select node
                if (mouseState.isLeftButton)
                {
                    if(drag.isDragging){
                        drag.EndDrag(evt.mousePosition);
                    }

                    if(node != null){
                        aiui.selectedNode = node;
                        aiui.Select((SelectorNode)node, null, null);
                    } else {
                        aiui.selectedNode = null;
                        aiui.Select(null, null, null);
                    }

                    GUI.changed = true;
                }
                else if (mouseState.isRightButton)
                {
                    ShowLoadMenu(evt.mousePosition);
                    evt.Use();
                    GUI.changed = true;
                }

                //// Resize canvas after a drag
                //else if(){
                //    // GUI.changed = true;
                //}

                else
                {
                    //  mode is set to none.
                }

                //if (mouseState.isLeftButton) Debug.LogFormat("Mouse Position : {0}", evt.mousePosition);
            }



        }




        private Rect GetViewport()
        {
            Rect rect = position;
            rect.height = position.height - topPadding - footerPadding;
            rect.x = 0;
            rect.y = topPadding;

            return rect;
        }


        #region Load and Save

        private void Load(string aiId, bool forceRefresh)
        {
            //  Do stuff...
            aiui = AIUI.Load(aiId, forceRefresh);
            _lastOpenAI = aiId;

            Selection.activeObject = aiui;
            Editor newInspector = Editor.CreateEditor(aiui);
            if(newInspector != null){
                AIInspectorEditor.instance = newInspector as AIInspectorEditor;
                AIInspectorEditor.instance.Repaint();
            }
        }


        private void Save(string newName)
        {
            aiui.Save(newName);
            EditorUtility.SetDirty(aiui);
            AssetDatabase.SaveAssets();
        }

        #endregion

        #region Misc functions

        private void OnAIExecute()
        {
            
        }

		private void OnFocus()
		{

		}


		private void OnSelectionChange()
        {
            SetVisualizedEntity(Selection.activeGameObject);
            Repaint();
        }


        private void SetVisualizedEntity(GameObject entity)
        {
            //  If Selection returns a GameObject.
            if (entity != null)
            {
                //  If GameObject contains UtilityAIComponent.
                if (entity.GetComponent<UtilityAIComponent>())
                {
                    visualizedEntity = entity;
                    GUI.changed = true;
                }
                //  If GameObject does not contains UtilityAIComponent.
                else
                {
                    _lastSelectedGameObject = visualizedEntity;
                    visualizedEntity = null;
                    GUI.changed = true;
                }
            }
            //  Nothing was selected.
            else
            {
                if (visualizedEntity != null)
                    _lastSelectedGameObject = visualizedEntity;
                visualizedEntity = null;
            }
        }


		private void UpdateVisualizedEntity(bool forceUpdate)
        {
            
        }


        private void Reload()
        {
            StoredAIs.Refresh();
        }


        #endregion





        public void ShowLoadMenu(Vector2 mousePos)
        {
            if (Application.isPlaying) return;

            var menu = new GenericMenu();

            if(aiui.selectedNode == null)
            {
                //  Add Selectors
                menu.AddItem(new GUIContent("Add Selector"), false, () => aiui.AddSelector(mousePos, typeof(ScoreSelector)));
            }

            if(aiui.selectedNode != null)
            {
                
                if(aiui.currentSelector != null)
                {
                    menu.AddItem(new GUIContent("Add Qualifier"), false, () => aiui.AddQualifier(typeof(CompositeScoreQualifier), aiui.currentSelector));
                    ///menu.AddItem(new GUIContent("Add Qualifier"), false, () => ShowSelectorWindow<IQualifier>(null) );

                    menu.AddDisabledItem(new GUIContent("Connect to Selector"));
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Remove Selector"), false, () => aiui.RemoveNode(aiui.currentSelector));
                }
            }

            menu.AddSeparator("");
            menu.AddDisabledItem(new GUIContent("Save"));

            menu.DropDown(new Rect(mousePos.x, mousePos.y, 0, 0));
        }


        public void ShowSelectorWindow<T>(Action<Type> callback)
        {
            AIEntitySelectorWindow.Get<T>(Event.current.mousePosition, callback);
        }



        public class DragData
        {
            public enum DragType
            {
                None,
                Node,
                Qualifier,
                Connector,
                Resize,
                MassSelect,
                Pan
            }

            public bool lastMouseDownOnCanvas;

            private AIEditorWindow _parent;
            private DragType _dragType;

            private TopLevelNode _node;

            private Vector2 _offset;
            private Vector2 _dragStart;
            private Vector2 _dragLast;
            private Vector3 _dragAnchor;


            public bool isDragging{
                get { return isDraggingType(_dragType); }
            }

            public Vector2 dragStart { get { return _dragStart; } }

            public Vector2 offset { get { return _offset; } }
           
            public Vector3 anchor { get { return _dragAnchor; } }



            public bool isDraggingType(DragType type)
            {
                switch(type)
                {
                    case DragType.None:
                        return false;
                    case DragType.Node:
                        return true;
                    case DragType.Qualifier:
                        return true;
                    case DragType.Connector:
                        return true;
                    case DragType.Pan:
                        return true;
                }
                return false;
            }


            public void StartNodeDrag(TopLevelNode target, Vector2 mousePos)
            {
                _node = target;
                _dragLast = mousePos;
                _dragStart = mousePos;
                //_offset = mousePos - target.viewArea.position;
                _dragType = DragType.Node;
            }

            public void StartConnectorDrag(Vector2 mousePos)
            {
                _parent.wantsMouseMove = false;
                _dragLast = mousePos;
                _dragStart = mousePos;
                _dragType = DragType.Connector;
            }

            public void StartPan(Vector2 mousePos)
            {
                _dragLast = mousePos;
                _dragStart = mousePos;
                _dragType = DragType.Pan;
            }




            public void DoDrag(Vector2 delta)
            {
                switch (_dragType)
                {
                    case DragType.None:
                        break;
                    case DragType.Node:
                        DoNodeDrag(delta);
                        break;
                    case DragType.Qualifier:
                        break;
                    case DragType.Connector:
                        DoConnectorDrag(delta);
                        break;
                    case DragType.Pan:
                        DoPan(delta);
                        break;
                }
            }

            private void DoNodeDrag(Vector2 delta)
            {
                //Vector2 newPosition = delta - _offset;
                _node.viewArea.position += delta;
                if (Application.isPlaying) 
                    return;
            }


            private void DoConnectorDrag(Vector2 delta)
            {

            }

            private void DoPan(Vector2 delta)
            {
                _parent.aiui.canvas.offset += delta;
                if (_parent.aiui.canvas.nodes != null){
                    for (int i = 0; i < _parent.aiui.canvas.nodes.Count; i++){
                        _parent.aiui.canvas.nodes[i].viewArea.position += delta;
                    }
                }
            }




            public void EndDrag(Vector2 mousePos)
            {
                switch (_dragType)
                {
                    case DragType.None:
                        break;
                    case DragType.Node:
                        EndNodeDrag(mousePos);
                        break;
                    case DragType.Qualifier:
                        break;
                    case DragType.Connector:
                        EndConnectorDrag(mousePos);
                        break;
                    case DragType.Pan:
                        EndPan(mousePos);
                        break;
                }
                _dragType = DragType.None;
            }

            public void CancelDrag()
            {
                _parent.wantsMouseMove = false;
                _dragType = DragType.None;
            }


            private void EndNodeDrag(Vector2 mousePos)
            {
                _node = null;
                //_offset = Vector2.zero; ;
                _dragLast = Vector2.zero;
                _dragStart = Vector2.zero;
            }

            private void EndConnectorDrag(Vector2 mousePos)
            {

            }

            private void EndPan(Vector2 mousePos)
            {

                _dragLast = Vector2.zero;
                _dragStart = Vector2.zero;
            }




            public DragData(AIEditorWindow parent)
            {
                _parent = parent;
            }




        }
    }
}





#region DrawUI

//private void DrawUI()
//{
//    if (aiui == null) return;
//    Event evt = Event.current;
//    //  Update MosueState.
//    mouseState.Update(evt);
//    TopLevelNode node = aiui.canvas.NodeAtPosition(evt.mousePosition);
//    if (node != null)
//    {
//        if (mouseState.isLeftButton)
//        {
//            if (mouseState.isMouseDown)
//            {
//                aiui.selectedNode = node;
//                GUI.changed = true;
//            }
//            if (mouseState.isMouseDrag)
//            {
//                //  If node is selected than, than update position of selected node.
//                if (node.isSelected)
//                {
//                    node.viewArea.position += evt.delta;
//                    evt.Use();
//                    GUI.changed = true;
//                }
//            }
//            if (mouseState.isMouseUp)
//            {
//                //  Not being dragged anymore.
//            }
//        }
//        //  If RMB is clicked down.
//        if (mouseState.isRightButton)
//        {
//            if (mouseState.isMouseDown)
//            {
//                //  If node is selected, show context menu.
//                if (node.isSelected)
//                {
//                    GenericMenu menu = new GenericMenu();
//                    if (node.InTitleArea(evt.mousePosition, nodeSettings))
//                    {
//                        menu.AddItem(new GUIContent("Add Qualifier"), false, () => aiui.AddQualifier(typeof(CompositeScoreQualifier), (SelectorNode)node));
//                    }
//                    else
//                    {
//                        menu.AddItem(new GUIContent("Remove Selector"), false, () => aiui.RemoveNode(node));
//                    }
//                    menu.ShowAsContext();
//                    GUI.changed = true;
//                    evt.Use();
//                }
//            }
//        }
//    }
//    //  Process all canvas events.  Only process events if no node is seelcted.
//    mouseState.Update(evt);
//    dragPos = Vector2.zero;
//    //  If LMB is clicked down.
//    if (mouseState.isLeftButton)
//    {
//        if (mouseState.isMouseDown)
//        {
//            //  If no nodes were selected.
//            if (node == null)
//            {
//                aiui.selectedNode = null;
//                GUI.changed = true;
//            }
//        }
//        if (mouseState.isMouseDrag)
//        {
//            if (GetViewport().Contains(evt.mousePosition))
//            {
//                //  If mouse click is not on a node, than can start drag.
//                dragPos = evt.delta;
//                if (aiui.canvas.nodes != null)
//                {
//                    for (int i = 0; i < aiui.canvas.nodes.Count; i++)
//                    {
//                        aiui.canvas.nodes[i].viewArea.position += dragPos;
//                    }
//                }
//                GUI.changed = true;
//            }
//        }
//        if (mouseState.isMouseUp)
//        {
//            //  Stop dragging.  use Event?
//            //aiui.selectedNode = null;
//            //GUI.changed = true;
//            //Debug.Log(aiui.selectedNode);
//        }
//    }
//    //  If RMB is clicked down.
//    if (mouseState.isRightButton)
//    {
//        if (mouseState.isMouseDown)
//        {
//            //  If node is selected, show context menu.
//            GenericMenu menu = new GenericMenu();
//            menu.AddItem(new GUIContent("Add Selector"), false, () => aiui.AddSelector(evt.mousePosition, typeof(ScoreSelector)));
//            menu.ShowAsContext();
//            evt.Use();
//        }
//    }
//}

#endregion
