using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;


namespace AtlasAI.AIEditor
{
    [CustomEditor(typeof(UtilityAIComponent))]
    public class UtilityAIComponentEditor : Editor
    {
        UtilityAIComponent _target;
        IContextProvider contextProvider;


        bool showDefaultInspector;
        bool showDeleteAssetOption;
        bool debugEditorFoldout = true;

        bool displayContextProviderName = false;


        void OnEnable()
        {
            _target = target as UtilityAIComponent;
            contextProvider = _target.GetComponent<IContextProvider>();
        }



        public void AddNew()
        {
            //AddClientWindow window = new AddClientWindow();
            //window.Init(window, this);

            var window = AISelectorWindow.Get(Event.current.mousePosition, DoAddNew);
        }


        public void Delete(int idx)
        {

            _target.aiConfigs = ShrinkArray(_target.aiConfigs, idx);

            EditorUtility.SetDirty(target);
            Repaint();
        }


        public void DoAddNew(AIStorage[] aiAsset)
        {
            //  Use the aiID to be able to grab the correct AIStorage;
            //serializedObject.Update();

            _target.aiConfigs = GrowArray(_target.aiConfigs, aiAsset.Length);

            for (int idx = 0; idx < aiAsset.Length; idx ++)
            {
                _target.aiConfigs[ (_target.aiConfigs.Length - aiAsset.Length) + idx] = new UtilityAIConfig()
                {
                    aiId = aiAsset[idx].aiId
                };
            }

            EditorUtility.SetDirty(_target);
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            AINameMapGenerator.WriteNameMapFile();
            //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }


        //  This is for adding predefined type.
        public void DoAddNew(AIStorage[] aiAsset, Type type)
        {
            _target.aiConfigs = GrowArray(_target.aiConfigs, aiAsset.Length);

            for (int idx = 0; idx < aiAsset.Length; idx++)
            {
                _target.aiConfigs[(_target.aiConfigs.Length - aiAsset.Length) + idx] = new UtilityAIConfig()
                {
                    aiId = aiAsset[idx].aiId,
                    isPredefined = true,
                    type = type.ToString()
                };
            }

            EditorUtility.SetDirty(_target);
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            AINameMapGenerator.WriteNameMapFile();
            //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }



        private T[] GrowArray<T>(T[] array, int increase)
        {
            T[] newArray = array;
            Array.Resize(ref newArray, array.Length + increase);
            return newArray;
        }

        private T[] ShrinkArray<T>(T[] array, int idx)
        {
            T[] newArray = new T[array.Length - 1];
            if (idx > 0)
                Array.Copy(array, 0, newArray, 0, idx);

            if (idx < array.Length - 1)
                Array.Copy(array, idx + 1, newArray, idx, array.Length - idx - 1);

            return newArray;
        }



        #region AI Client Inspector

        float propertyHeight;
        /// <summary>
        /// Client Inspector
        /// </summary>
        protected virtual void DrawTaskNetworkInspector()
        {
            
            //  Displaying header options.
            using (new EditorGUILayout.HorizontalScope()){
                EditorGUILayout.LabelField("AIs", EditorStyles.boldLabel);


                //  Button to add predefined mocks.
                if (GUILayout.Button("Add Predefined", EditorStyles.miniButton, GUILayout.Width(130f)))
                {
                    AddPredefinedAIWindow window = new AddPredefinedAIWindow();
                    window.Init(window, this);
                }

                //  Add a new UtilityAIClient
                if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.Width(65f))){
                    AddNew();
                }
            }


            //  Displaying the AI Clients

            //var aiClientsDisplayBox = new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Height(propertyHeight));
            var aiClientsDisplayBox = new EditorGUILayout.VerticalScope(EditorStyles.helpBox);
            using (aiClientsDisplayBox)
            {
                if (_target.aiConfigs.Length == 0){
                    EditorGUILayout.HelpBox("There are no AI's attached to this UtilityAIComponent.", MessageType.Info);
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    for (int i = 0; i < _target.aiConfigs.Length; i++)
                    {
                        if (_target.aiConfigs[i] != null)
                        {
                            UtilityAIConfig aiConfig = _target.aiConfigs[i];
                            SerializedProperty property = serializedObject.FindProperty("aiConfigs").GetArrayElementAtIndex(i);
                            SerializedProperty isActive = property.FindPropertyRelative("isActive");
                            SerializedProperty debug = property.FindPropertyRelative("debug");

                            //  For Client Options
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                //  aiConfig isActive toggle.
                                EditorGUILayout.PropertyField(isActive, GUIContent.none, GUILayout.Width(Screen.width * 0.1f) );
                                //EditorGUILayout.PropertyField(isActive, GUIContent.none, GUILayout.Width(28f));


                                //  Debug Client toggle
                                //GUILayout.FlexibleSpace();
                                GUILayout.Space(5);
                                EditorGUILayout.LabelField("Debug Client", GUILayout.Width(75f));
                                EditorGUILayout.PropertyField(debug, GUIContent.none, GUILayout.Width(16));
                                GUILayout.Space(5);
                                //GUILayout.FlexibleSpace();
    
                                ////  Debug button
                                //if (GUILayout.Button("Debug", EditorStyles.miniButton, GUILayout.Width(48f)))//  GUILayout.Width(Screen.width * 0.15f)
                                //{
                                //    //Debug.Log("Currently cannot debug at the moment.");
                                //    if(_target.clients != null)
                                //    {
                                //        if(_target.clients.Length == _target.aiConfigs.Length)
                                //        {
                                //            var qualifiers = _target.clients[i].ai.rootSelector.qualifiers.Count;
                                //            Debug.LogFormat("{0} Qualifier count:  {1}", aiConfig.aiId, qualifiers);
                                //        }
                                //    }
                                //    else{
                                //        string aiClientName = aiConfig.aiId;
                                //        var field = typeof(AINameMap).GetField(aiClientName, BindingFlags.Public | BindingFlags.Static);
                                //        Guid aiId = (Guid)field.GetValue(null);
                                //        Debug.LogFormat("{0} Guid:  {1}", aiConfig.aiId, aiId);
                                //    }
                                //}


                                //  Add or Switch button
                                if (GUILayout.Button(EditorStyling.addTooltip, EditorStyling.Skinned.addButtonSmall, 
                                                     GUILayout.Width(28f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                                {
                                    Debug.Log("TODO:  Add functionality for changing AIs.");
                                }

                                //  Delete button
                                if (GUILayout.Button(EditorStyling.deleteTooltip, EditorStyling.Skinned.deleteButtonSmall,
                                                     GUILayout.Width(28f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                                {
                                    Delete(i);
                                }

                                ////  Add or Switch button
                                //if (InspectorUtility.OptionsPopupButton(InspectorUtility.AddContent))
                                //{
                                //    Debug.Log("TODO:  Add functionality for changing AIs.");
                                //}

                                ////  Delete button
                                //if (InspectorUtility.OptionsPopupButton(InspectorUtility.DeleteContent))
                                //{
                                //    Delete(i);
                                //}
                            }

                            //  Draw the aiconfigs.
                            EditorGUILayout.PropertyField(property);


                            EditorGUILayout.Space();
                        }
                    }
                }


            }  // The group is now ended





            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Editor", EditorStyles.miniButton, GUILayout.Width(65f))){
                    //AIAssetEditor.Init();
                }
            }
            GUILayout.Space(8);






        }


        #endregion



        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            using (new EditorGUILayout.HorizontalScope())
            {
                _target.showDefaultInspector = EditorGUILayout.ToggleLeft("Show Default Inspector", _target.showDefaultInspector);

                if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.Width(65f)))
                {
                    for (int index = _target.aiConfigs.Length - 1; index >= 0; index--)
                    {
                        Delete(index);
                    }
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                _target.showDeleteAssetOption = EditorGUILayout.ToggleLeft("Show Delete Asset Btn", _target.showDeleteAssetOption);
                if(_target.showDeleteAssetOption){
                    if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(65f)))
                    {
                        for (int index = _target.aiConfigs.Length -1 ; index >= 0 ; index--)
                        {
                            Delete(index);
                        }
                        var results = AssetDatabase.FindAssets("t:AIStorage", new string[]{AIManager.TempStorageFolder} );
                        foreach(string guid in results)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));

                        }

                        AINameMapGenerator.WriteNameMapFile();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                }
            }
            //using (new EditorGUILayout.HorizontalScope())
            //{
            //    _target.selectAiAssetOnCreate = EditorGUILayout.ToggleLeft("Select Asset On Create", _target.selectAiAssetOnCreate);
            //}

            GUILayout.Space(8);

            if (_target.showDefaultInspector)
            {
                DrawDefaultInspector();
                GUILayout.Space(8);
            } 


            //  Draw the TaskNetworkInspector
            DrawTaskNetworkInspector();


            //  Display a message box if there is not ContextProvider.
            if (contextProvider == null)
            {
                var contextMsg = string.Format("{0} has no context provider.", target.name);
                EditorGUILayout.HelpBox(contextMsg, MessageType.Error);
                EditorGUILayout.Space();
            }
            else if (contextProvider != null && displayContextProviderName)
            {
                var contextMsg = string.Format("ContextProvider name:  {0}", contextProvider.GetType());
                EditorGUILayout.HelpBox(contextMsg, MessageType.Info);
            }


            ////  Name Map
            //if (GUILayout.Button("AINameMap", EditorStyles.miniButton, GUILayout.Width(65f)))
            //{
            //    if(AINameMap.aiNameMap == null){
            //        Debug.Log("aiNameMap is not initialized");
            //    }
            //    else if (AINameMap.aiNameMap.Count == 0)
            //    {
            //        Debug.Log("Nothing registered to aiNameMap. Reregistering AiNameMap.");
            //        AINameMap.RegenerateNameMap();

            //    }
            //    else{
            //        foreach (KeyValuePair<Guid, string> keyValue in AINameMap.aiNameMap)
            //        {
            //            string path = AssetDatabase.GUIDToAssetPath(keyValue.Key.ToString("N"));
            //            Debug.LogFormat("AIID:  {0}  | Guid:  {1}\nPath:  {2}", keyValue.Value, keyValue.Key.ToString("N"), path);


            //            AIStorage aiStorage = AssetDatabase.LoadMainAssetAtPath(path) as AIStorage;

            //            Debug.LogFormat("Path:  {0}\nAIStorage:  {1}", path, aiStorage);
            //        }

            //    }
            //}

            GUILayout.Space(5);

            if (GUILayout.Button("Generate Name Map", EditorStyles.miniButton, GUILayout.Width(120f)))
            {
                AINameMapGenerator.WriteNameMapFile();

                //Debug.Log(AINameMapHelper.AgentActionAI);
                //Debug.Log(AINameMapHelper.AgentMoveAI);
                //Debug.Log(AINameMapHelper.AgentScanAI);
            }


            serializedObject.ApplyModifiedProperties();
        }











        string ClientDiagnosticsInfo(int index)
        {
            string clientInfo = "";

            IUtilityAI utilityAI = _target.clients[index].ai;
            Selector rootSelector = utilityAI.rootSelector;
            bool isSelected = false;

            clientInfo += string.Format("<b>AI Name:  {0}</b>\n", utilityAI.name);
            foreach (IQualifier qualifier in rootSelector.qualifiers)
            {
                isSelected = _target.clients[index].activeAction == qualifier.action;
                clientInfo += QualifierNetworkInfo(qualifier, isSelected);
            }

            isSelected = _target.clients[index].activeAction == rootSelector.defaultQualifier.action;
            clientInfo += QualifierNetworkInfo((DefaultQualifier)rootSelector.defaultQualifier, isSelected);

            return clientInfo;
        }



        string QualifierNetworkInfo(IQualifier qualifier, bool isSelected)
        {
            //string networkInfo = " <b>Qualifier:</b> {0} | <b>Score:</b>: <color=lime>{1}</color>\n <b>Action:</b>:  <color=lime>{2}</color>\n";
            string networkInfo = "";

            string qualiferName = "";
            //string qualifierScore;
            string[] scorers;
            //string[] scorersScores;
            string actionName = "";
            string[] optionScorers;

            if (qualifier is DefaultQualifier)
                qualiferName = string.Format("<b>{0}</b>{1}", "DefaultQualifier:  ", qualifier.GetType().Name);
            else
                qualiferName = string.Format("<b>{0}</b>{1}", "Qualifier:  ", qualifier.GetType().Name);


            networkInfo += string.Format(" {0}\n", qualiferName);


            if (qualifier is CompositeQualifier)
            {
                var _scorers = ((CompositeQualifier)qualifier).scorers;
                int count = _scorers.Count;
                scorers = new string[count];
                //scorersScores = new string[count];
                for (int i = 0; i < count; i++)
                {
                    scorers[i] = string.Format("<b>{0}</b>{1} ", "Scorer: ", _scorers[i].GetType().Name);
                    networkInfo += string.Format(" -------- {0}\n", scorers[i]);
                }
            }

            if (qualifier is DefaultQualifier)
            {
                string _actionName = qualifier.action == null ? "<None>" : qualifier.action.GetType().Name;
                actionName = string.Format("<b>{0}</b>{1}", "Action: ", _actionName);
                networkInfo += string.Format(" -- {0}\n", actionName);
                return networkInfo;
            }

            var action = qualifier.action;
            actionName = string.Format("<b>{0}</b>{1}", "Action: ", action.GetType().Name);
            //networkInfo += string.Format(" -- {0} <{1}>\n", actionName, action.GetType());
            networkInfo += string.Format(" -- {0}\n", actionName);

            //if (action.GetType() == typeof(ActionWithOptions<>))
            if (action is ActionWithOptions<Vector3>)
            {
                var _actionWithOption = action as ActionWithOptions<Vector3>;
                var _optionScorers = _actionWithOption.scorers;
                int count = _optionScorers.Count;

                optionScorers = new string[count];
                for (int i = 0; i < count; i++)
                {
                    optionScorers[i] = string.Format("<b>{0}</b>{1} ", "ScoredOption: ", _optionScorers[i].GetType().Name);
                    networkInfo += string.Format("     ----- {0}\n", optionScorers[i]);
                }
            }
            else if(action is ActionWithOptions<Bang.ActorHealth>)
            {
                var _actionWithOption = action as ActionWithOptions<Bang.ActorHealth>;
                var _optionScorers = _actionWithOption.scorers;
                int count = _optionScorers.Count;

                optionScorers = new string[count];
                for (int i = 0; i < count; i++)
                {
                    optionScorers[i] = string.Format("<b>{0}</b>{1} ", "ScoredOption: ", _optionScorers[i].GetType().Name);
                    networkInfo += string.Format("     ----- {0}\n", optionScorers[i]);
                }
            }

            return networkInfo;
        }







    }






}