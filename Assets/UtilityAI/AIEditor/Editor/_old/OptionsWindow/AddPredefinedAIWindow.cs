namespace AtlasAI.AIEditor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;



    /// <summary>
    ///  Adds a predefined aiConfig to a client.
    /// </summary>
    public class AddPredefinedAIWindow : EditorWindow
    {
        AddPredefinedAIWindow window;
        const string filterType = "t:AIStorage";
        protected UtilityAIComponentEditor editor { get; set; }
        protected UtilityAIComponent taskNetwork { get; set; }

        protected int windowMinSize = 250;
        protected int windowMaxSize = 350;
        protected string windowTitle = "Create Predefined Client Window";




        List<Type> displayTypes;

        GUIStyle contentStyle;




        public void Init(AddPredefinedAIWindow window, UtilityAIComponentEditor editor)
        {
            displayTypes = GetAllOptions<UtilityAIAssetConfig>();
            displayTypes.Reverse();

            this.editor = editor;
            this.taskNetwork = editor.target as UtilityAIComponent;
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            this.window.titleContent = new GUIContent(windowTitle);

            this.window.ShowUtility();

            contentStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };
        }


        protected virtual void OnGUI()
        {

            foreach (Type type in displayTypes)
            {
                GUIContent buttonLabel = new GUIContent(type.Name);
                if (GUILayout.Button(buttonLabel, contentStyle, GUILayout.Height(18)))
                {
                    editor.DoAddNew(new AIStorage[]{ AIStorage.CreateAsset(type.Name, type.Name, taskNetwork.selectAiAssetOnCreate) } , type);
                    CloseWindow();
                }
                GUILayout.Space(4);
            }

        }


        ////  AIStorage aiAsset is a premade aiAsset
        //void AddAIAsset(Type type)
        //{


        //    //  Create a new AIStorage instance.
        //    AIStorage aiStorage = new AIStorage();

        //    //  Initialize AIConfig so we can get the name of the predefined config.
        //    IUtilityAIConfig config = (IUtilityAIConfig)Activator.CreateInstance(type);
        //    //  Use the new instance to create a scriptableObject of aiAsset.
        //    AIStorage aiAsset = aiStorage.CreateAsset(config.name, config.name, taskNetwork.selectAiAssetOnCreate);

        //    //  Create a new UtilityAIClient
        //    UtilityAIClient client = new UtilityAIClient(aiAsset.configuration, taskNetwork.GetComponent<IContextProvider>());
        //    client.ai = aiAsset.configuration;      //  Add the AtlasAI to the UtilityAIClient.
        //    taskNetwork.clients.Add(client);        //  Add the client to the TaskNetwork.

        //    //  Configure the predefined settings.
        //    config.SetupAI(client.ai);

        //    ////  Remove the type from the list of predefined clients.
        //    //displayTypes.Remove(type);
        //    editor.DoAddNew(new AIStorage[] { aiAsset });
        //}



        protected void CloseWindow()
        {
            window.Close();
        }


        /// <summary>
        /// Get all available classes of type.
        /// </summary>
        /// <returns> All available options. </returns>
        /// <typeparam name="T"> Is one the AI blocks. </typeparam>
        private List<Type> GetAllOptions<T>() //where T: Selector, IQualifier, IAction, IContextualScorer
        {
            List<Type> availableTypes = new List<Type>();
            Type type = typeof(T);
            //  Gets all custom Types in this assembly and adds it to a list.
            //var optionTypes = Assembly.GetAssembly(type).GetTypes()
            //.Where(t => t.IsClass && t.Namespace == typeof(AIManager).Namespace)
            //.ToList();
            var optionTypes = Assembly.GetAssembly(type).GetTypes()
                                      .Where(t => t.IsClass).ToList();

            //optionTypes.ForEach(t => Debug.Log(t));

            foreach (Type option in optionTypes)
            {
                if (option.BaseType == type)
                    availableTypes.Add(option);
            }
            //availableTypes.ForEach(t => Debug.Log(t));
            return availableTypes;
        }
    }











}