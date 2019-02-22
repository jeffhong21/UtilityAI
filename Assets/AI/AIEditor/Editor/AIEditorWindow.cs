namespace UtilAI.AIEditor
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public class AIEditorWindow : EditorWindow
    {
        private static readonly Color majorGridColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        private static readonly Color minorGridColor = new Color(0.5f, 0.5f, 0.5f, 0.4f);
        private static readonly Color gridBackground = Color.gray;
        public static AIEditorWindow activeInstance;


        private AIUI aiui;
        private MouseState mouseState;
        private Vector2 dragPos;


        [HideInInspector, SerializeField]
        private string lastOpenAI;
        private float _topPadding = 28;
        private float _footerPadding = 28;


        private GUIContent headerTooltip = new GUIContent();
        private Rect toolbarRect = new Rect();
        private Rect footbarRect = new Rect();


        public float topPadding{
            get{ return _topPadding; }
        }
        public float footerPadding{
            get { return _footerPadding; }
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
            activeInstance.titleContent = new GUIContent("AI Editor");
            //  Show window
            activeInstance.Show();
            return activeInstance;
        }



		private void OnEnable(){
            mouseState = new MouseState();


            if (string.IsNullOrEmpty(lastOpenAI) == false)
                Load(lastOpenAI);
            else
                CreateDefault("DefaultAI");
		}


		private void OnDisable()
        {

		}


        private GUIContent GetAITitle()
        {
            if (headerTooltip == null)
                headerTooltip = new GUIContent();
            
            headerTooltip.text = aiui == null ? "</null>" : aiui.name;
            headerTooltip.tooltip = aiui == null ? "</No AI selected >" : "The selected AI is " + aiui.name;
            return headerTooltip;
        }


		private void OnGUI()
		{
            Matrix4x4 m = GUI.matrix;

            DrawBackgroundGrid();
            toolbarRect.Set(0, 0, position.width, topPadding);
            DrawToolBar(toolbarRect, GetAITitle());
            footbarRect.Set(0, position.height - footerPadding + 10, position.width, footerPadding);
            DrawFootbar(footbarRect);


            if(aiui != null)
            {
                DrawNodes(GetViewport());
                DrawUI();
            }


            GUI.matrix = m;
            if (GUI.changed) Repaint();
		}


        #region -- Draw GUI Elements --

        private void DrawBackgroundGrid()
        {
            Color _oldColor = Handles.color;
            Handles.color = majorGridColor;
            DrawGridLines(position.width, position.height, 100);
            Handles.color = minorGridColor;
            DrawGridLines(position.width, position.height, 20);
            Handles.color = _oldColor;
        }


        private void DrawGridLines(float width, float height, float size)
        {
            int widthDivs = Mathf.CeilToInt(width / size);
            int heightDivs = Mathf.CeilToInt(height - topPadding - footerPadding / size);

            Handles.BeginGUI();

            Vector2 panOffset = aiui.canvas == null ? Vector2.zero : (aiui.canvas.offset + dragPos) * 0.5f;
            Vector3 newOffset = new Vector3(panOffset.x % size, panOffset.y % size, 0);

            // Draws the vertical lines.
            for (int i = 0; i < widthDivs; i++)
            {
                Vector3 p1 = new Vector3(size * i, topPadding, 0) + newOffset;                          // (100 * 1, 18)
                Vector3 p2 = new Vector3(size * i, height - footerPadding, 0f) + newOffset;    //  (100 * i, 400
                Handles.DrawLine(p1, p2);
            }
            //  Draws the horizontal lines.
            for (int i = 0; i < heightDivs; i++)
            {
                Vector3 p1 = new Vector3(-size, size * i, 0) + newOffset;                   //  (-100, 100 * 1)
                Vector3 p2 = new Vector3(width, size * i, 0f) + newOffset;        //  (350, 100 * 1)
                Handles.DrawLine(p1, p2);
            }
            Handles.EndGUI();
        }


        private void DrawToolBar(Rect rect, GUIContent headerTitle)
        {
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope()){
                if (GUILayout.Button(EditorStyling.newTooltip, EditorStyles.toolbarButton, GUILayout.Width(48))){
                    Debug.Log("Creating new AI");
                }
                if (GUILayout.Button(EditorStyling.loadTooltip, EditorStyles.toolbarButton, GUILayout.Width(48))){
                    Debug.Log("Loading AI");
                }

                //  Draw current loaded AI.
                GUILayout.FlexibleSpace();
                GUILayout.Label(headerTitle, EditorStyling.Skinned.boldTitle);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(48))){
                    Reload();
                }
            }
            GUILayout.EndArea();
        }


        private void DrawFootbar(Rect rect)
        {
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(string.Format("{0}", Event.current.mousePosition));
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("aiui: {0} | canvas: {1} | node count: {2}", 
                                              aiui == null ? "</null>" : aiui.name, 
                                              aiui.canvas == null ? "</null>" : "true", 
                                              aiui.canvas == null ? "</null>" : aiui.canvas.nodes.Count.ToString()), 
                                              EditorStyling.Skinned.boldTitle);
                GUILayout.FlexibleSpace();


                if (GUILayout.Button("Close Window", EditorStyles.toolbarButton, GUILayout.Width(88))){
                    activeInstance.Close();
                }
            }
            GUILayout.EndArea();
        }


        #endregion




        private void DrawNodes(Rect viewPort)
        {
            using (new GUILayout.AreaScope(viewPort))
            {
                foreach (SelectorNode node in aiui.canvas.selectorNodes)
                {
                    node.DrawNode();
                }
                //for (int i = 0; i < aiui.canvas.nodes.Count; i++)
                //{
                //    SelectorNode node = (SelectorNode)aiui.canvas.nodes[i];
                //    if(node != null)node.DrawNode();
                //}
            }
        }





        private bool DrawUI()
        {
            dragPos = Vector2.zero;

            Event evt = Event.current;
            mouseState.Update(evt);

            //  Mouse button is down.
            if (mouseState.isMouseDown)
            {

            }
            //  LMB button is dragging.
            if (mouseState.isMouseDrag && mouseState.isLeftButton)
            {
                dragPos = evt.mousePosition;
                evt.Use();
                GUI.changed = true;
            }
            //  Mouse button is up.
            if (mouseState.isMouseUp)
            {
                //  Select node
                if (mouseState.isLeftButton)
                {

                    //GUI.changed = true;
                }
                else if (mouseState.isRightButton)
                {
                    ShowContextMenu(evt.mousePosition);
                    evt.Use();
                    GUI.changed = true;
                }
                else
                {
                    //  mode is set to none.
                }
            }


            return true;
        }


        private void CreateDefault(string aiId)
        {
            aiui = AIUI.Create(aiId);
            Selection.activeObject = aiui;
        }


        private void Load(string aiId)
        {
            aiui = AIUI.Load(aiId);
            lastOpenAI = aiId;
            Selection.activeObject = aiui;
        }


        private Rect GetViewport()
        {
            Rect rect = position;
            rect.height = position.height - topPadding - footerPadding;
            rect.x = 0;
            rect.y = topPadding;

            return rect;
        }




        private void Reload()
        {
            Repaint();
        }













        public void ShowContextMenu(Vector2 mousePos)
        {
            if (Application.isPlaying) return;

            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Add Selector"), false, () => aiui.AddSelector(mousePos));

            menu.DropDown(new Rect(mousePos.x, mousePos.y, 0, 0));
        }




        private void _TempContextMenuAction()
        {
            
        }

	}

}

