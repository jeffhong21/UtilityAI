//namespace AtlasAI.AIEditor
//{
//    using System;
//    using UnityEngine;
//    using UnityEditor;



//    /// <summary>
//    /// Used in AIAssetEditor to create new AIStorage
//    /// </summary>
//    public class CreateNewClientWindow : EditorWindow
//    {
//        CreateNewClientWindow window;
//        const string filterType = "t:AIStorage";

//        protected string windowTitle = "Add Clients";
//        protected string defaultAiID = "NewUtilityAI";
//        protected string defaultAiName = "NewUtilityAI";
//        //  Name for the AI file.
//        string aiName { get; set; }



//        public void Init(CreateNewClientWindow window)
//        {
//            this.window = window;
//            this.window.minSize = this.window.maxSize = new Vector2(250, 100);
//            this.window.titleContent = new GUIContent("Add Clients");
//            this.window.ShowUtility();
//        }


//        protected virtual void OnGUI()
//        {
//            DrawWindowContents();
//        }


//        protected void DrawWindowContents()
//        {
            
//            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
//            {
//                EditorGUILayout.LabelField("New AI Name", EditorStyling.Styles.TextCenterStyle);
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    GUILayout.Label("Name: ", GUILayout.Width(window.minSize.x * 0.18f));
//                    aiName = GUILayout.TextField(aiName);
//                }
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    if (GUILayout.Button("Ok"))
//                    {
//                        AIStorage aiAsset = AIStorage.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiID : aiName,
//                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiName : aiName);

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