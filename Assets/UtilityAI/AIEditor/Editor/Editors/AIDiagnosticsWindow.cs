using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AtlasAI.Visualization;

namespace AtlasAI.AIEditor
{
    public class AIDiagnosticsWindow : EditorWindow
    {
        //
        // Static Fields
        //
        private static GameObject _lastSelectedGameObject;
        public static AIDiagnosticsWindow activeInstance;

        //
        // Fields
        //
        private GameObject _visualizedEntity;
        private IUtilityAI _visualizedAI;
        private IUtilityAI _ai;
        private GUIContent _title = new GUIContent("AI Diagnostics");
        private float _topPadding = 20;
        private AIUI _ui;
        [HideInInspector, SerializeField]
        private string _lastOpenAI;



        //
        // Properties
        //
        public IUtilityAI ai{
            get { return _ai; }
        }

        public UtilityAIVisualizer visualizedAI{
            get { return _visualizedAI as UtilityAIVisualizer; }
        }

        public Selector rootSelector{
            get { return ai.rootSelector; }
        }

        private float topPadding{
            get { return _topPadding + 2f; }
        }



        //
        // Static Methods
        //
        public static void UpdateVisualizedEntities()
        {
            
        }


        //[MenuItem("UtilityAI/AI Diagnostics Window")]
        public static void ShowWindow()
        {
            //  Initialize window
            activeInstance = GetWindow<AIDiagnosticsWindow>();
            activeInstance.minSize = new Vector2(330f, 360f);
            activeInstance.maxSize = new Vector2(600f, 4000f);
            activeInstance.titleContent = activeInstance._title;
            //  Show window
            activeInstance.Show();
        }



		//
		// Methods
		//
        private void OnEnable()
        {
            //EditorStyling.Canvas.Init();
            //EditorStyling.Skinned.Init();

            activeInstance = this;
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;


        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnDestroy()
        {
            EditorApplication.update -= OnEditorUpdate;
        }


		private void OnGUI()
        {
            //  Draw Window GUI.
            DrawToolbar();
            DrawBody();

            //  Update variables.
            SetVisualizedEntity(Selection.activeGameObject);


            if (GUI.changed) Repaint();
        }


		private void DrawToolbar()
        {
            Rect rect = new Rect(0, 2, position.width, topPadding);
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(new GUIContent("New"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {

                }
                GUILayout.Space(5);
                if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    if (_visualizedEntity != null)
                    {
                        GenericMenu storedAIs = new GenericMenu();
                        for (int i = 0; i < _visualizedEntity.GetComponent<UtilityAIComponent>().aiConfigs.Length; i++)
                        {
                            var aiConfig = _visualizedEntity.GetComponent<UtilityAIComponent>().aiConfigs[i];
                            storedAIs.AddItem(new GUIContent(aiConfig.aiId), false, SetActiveClient, aiConfig.aiId);
                        }
                        storedAIs.ShowAsContext();
                    }
                }
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();

                GUILayout.Label(_lastOpenAI);


                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
                if (GUILayout.Button(new GUIContent("Delegate"), EditorStyles.toolbarButton, GUILayout.Width(75)))
                {
                    if(_visualizedEntity != null && !string.IsNullOrEmpty(_lastOpenAI)){
                        var field = typeof(AINameMap).GetField(_lastOpenAI, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        Guid guid = (Guid)field.GetValue(null);

                        var client = AIManager.GetAIClient(_visualizedEntity, guid);
                        Debug.Log(client);
                    }
                }

                if (GUILayout.Button(new GUIContent("State"), EditorStyles.toolbarButton, GUILayout.Width(48)))
                {
                    if(_visualizedEntity != null){
                        var cmpt = _visualizedEntity.GetComponent<UtilityAIComponent>();
                        for (int i = 0; i < cmpt.clients.Length; i++){
                            Debug.LogFormat("{0} | State: {1}", cmpt.name, cmpt.clients[i].state);
                        }
                    }
                }
                GUILayout.Space(5);


            }
            GUILayout.EndArea();
        }


        private void SetActiveClient(object asset){
            //_ui = AIUI.Create((string)asset);
            _lastOpenAI = (string)asset;
            //Reload();
            Repaint();
        }


        private void DrawBody()
        {
            Rect rect = new Rect(4, 2 + topPadding, position.width - 4, position.height);
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.VerticalScope())
            {
                //  Draw Current Selection
                GUILayout.Label(string.Format("Current Selection: {0} | Last Selected: {1}",
                              _visualizedEntity == null ? "<None>" : _visualizedEntity.name,
                              _lastSelectedGameObject == null ? "<None>" : _lastSelectedGameObject.name));

                //  Draw Selection Clients.
                if(_visualizedEntity != null){
                    var component = _visualizedEntity.GetComponent<UtilityAIComponent>();
                    //GUILayout.Label(component.aiConfigs.Length.ToString());
                    for (int i = 0; i < component.aiConfigs.Length; i++){
                        GUILayout.Label(component.aiConfigs[i].aiId);
                        //GUILayout.Space(5);
                    }
                }


                GUILayout.Space(5);
                //  Draw Client Configuration.
                if (_visualizedEntity != null && _ui != null){
                    if (_visualizedEntity.GetComponent<UtilityAIComponent>().clients != null){
                        for (int i = 0; i < _visualizedEntity.GetComponent<UtilityAIComponent>().clients.Length; i++){
                            DrawConfiguration(_visualizedEntity.GetComponent<UtilityAIComponent>().clients[i]);
                        }
                    }
                }


            }
            GUILayout.EndArea();
        }


        private void DrawConfiguration(IUtilityAIClient client)
        {
            string config = string.Format("AI Name:: {0}\n", client.ai.name);
            config += string.Format("Selector: {0}\n", client.ai.rootSelector.GetType().Name);
            for (int i = 0; i < client.ai.rootSelector.qualifiers.Count; i++)
            {
                config += string.Format("  Qualifiers: {0}\n", client.ai.rootSelector.qualifiers[i].GetType().Name);
                config += string.Format("     * Action: {0}\n", client.ai.rootSelector.qualifiers[i].action.GetType().Name);
            }
            GUILayout.Label(config);
            GUILayout.Space(8);
        }





        private void DrawSelectorUI()
        {
            
        }




        private void Reload()
        {
            //if(string.IsNullOrEmpty(_lastOpenAI) == false){
            //    _ui = AIUI.Load(_lastOpenAI, false);
            //    Repaint();
            //}
        }


        private Rect GetViewport()
        {
            Rect rect = position;
            rect.height = position.height - topPadding;
            rect.x = (position.width / 2) - (position.width / 2);
            rect.y = ((position.height / 2) - (position.height / 2)) + topPadding;
            return rect;
        }



        private void SetVisualizedEntity(GameObject visualizedEntity)
        {
            //  If Selection returns a GameObject.
            if(visualizedEntity != null){
                //  If GameObject contains UtilityAIComponent.
                if(visualizedEntity.GetComponent<UtilityAIComponent>()){
                    _visualizedEntity = visualizedEntity;
                    GUI.changed = true;
                }
                //  If GameObject does not contains UtilityAIComponent.
                else{
                    _lastSelectedGameObject = _visualizedEntity;
                    _visualizedEntity = null;
                    GUI.changed = true;
                }
            }
            //  Nothing was selected.
            else{
                if(visualizedEntity != null)
                    _lastSelectedGameObject = visualizedEntity;
                _visualizedEntity = null;
            }
        }



		private void OnSelectionChange()
		{
            SetVisualizedEntity(Selection.activeGameObject);
            Repaint();
		}


		private void OnAIExecute()
        {
            
        }


        private void OnEditorUpdate()
        {

        }


        private void ShowLoadMenu()
        {
            Event evt = Event.current;
            Rect rect = new Rect(evt.mousePosition.x, evt.mousePosition.y, 200f, 100f);

        }

    }
}