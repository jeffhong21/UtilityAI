//namespace AtlasAI.AIEditor
//{
//    using System;
//    using System.IO;
//    using System.Linq;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using UnityEngine;
//    using UnityEditor;



//    /// <summary>
//    /// Add options window.
//    /// Currently being tested.
//    /// Currently not being used.
//    /// </summary>
//    public class AddClientWindow : EditorWindow
//    {
//        AddClientWindow window;
//        const string filterType = "t:AIStorage";
//        protected UtilityAIComponentEditor editor { get; set; }
//        protected UtilityAIComponent taskNetwork { get; set; }

//        protected int windowMinSize = 250;
//        protected int windowMaxSize = 350;
//        protected string windowTitle = "Add Clients";

//        protected string defaultID = "NewAIAsset";
//        protected string defaultName = "New AI Asset";
//        //  Name for the AI file.
//        string aiName { get; set; }

//        //Dictionary<Type, Type> displayTypes;
//        //string searchStr;
//        //FieldInfo field;
//        GUIStyle contentStyle;


//        //void AddAIAsset(AIStorage aiAsset)
//        //{
//        //    //  Add asset and client to TaskNetwork
//        //    //UtilityAIClient client = new UtilityAIClient(aiAsset.configuration, taskNetwork.contextProvider);
//        //    UtilityAIClient client = new UtilityAIClient(aiAsset.configuration, taskNetwork.GetComponent<IContextProvider>());
//        //    client.ai = aiAsset.configuration;
//        //    taskNetwork.clients.Add(client);
//        //    //taskNetwork.assets.Add(aiAsset);

//        //    EditorUtility.SetDirty(taskNetwork);
//        //}


//        public void Init(AddClientWindow window, UtilityAIComponentEditor editor)
//        {
            
//            //this.taskNetwork = taskNetwork;
//            this.editor = editor;
//            this.window = window;
//            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
//            this.window.titleContent = new GUIContent(windowTitle);

//            this.window.ShowUtility();

//            contentStyle = new GUIStyle(GUI.skin.button)
//            {
//                alignment = TextAnchor.MiddleCenter
//            };
//        }


//        protected virtual void OnGUI()
//        {
//            //  Section for new name
//            CreateClientDrawer();

//            EditorGUILayout.Space();
//            using (new EditorGUILayout.VerticalScope())
//            {
//                foreach (var guid in AssetDatabase.FindAssets(filterType))
//                {
//                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
//                    GUIContent buttonLabel = new GUIContent(Path.GetFileNameWithoutExtension(assetPath));

//                    if (GUILayout.Button(buttonLabel, GUILayout.Height(18)))
//                    {
//                        AIStorage aiAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as AIStorage;

//                        //  Add asset and client to TaskNetwork
//                        //AddAIAsset(aiAsset);
//                        editor.DoAddNew(new AIStorage[] { aiAsset });
//                        // -----------------------------------

//                        //taskNetworkEditor.AddUtilityAIAsset(aiAsset);
//                        CloseWindow();
//                    }
//                    GUILayout.Space(2);
//                }
//            }
//        }

//        void CreateClientDrawer()
//        {


//            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
//            {
//                EditorGUILayout.LabelField("New AI Name", EditorStyling.Styles.TextCenterStyle);
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    GUILayout.Label("Name: ", GUILayout.Width(windowMinSize * 0.18f));
//                    aiName = GUILayout.TextField(aiName);
//                }
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    if (GUILayout.Button("Ok"))
//                    {
//                        AIStorage aiAsset = AIStorage.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultID : aiName,
//                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultName : aiName );

//                        //  Add asset and client to TaskNetwork
//                        //AddAIAsset(aiAsset);
//                        editor.DoAddNew(new AIStorage[] { aiAsset });
//                        // -----------------------------------

//                        //editor.AddUtilityAIAsset(aiAsset);
//                        CloseWindow();
//                    }
//                    if (GUILayout.Button("Cancel"))
//                    {
//                        CloseWindow();
//                    }
//                }
//            }
//        }



//        protected void CloseWindow()
//        {
//            window.Close();
//        }


//    }











//}