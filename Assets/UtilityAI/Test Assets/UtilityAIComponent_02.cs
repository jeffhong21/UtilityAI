//namespace AtlasAI
//{
//    using UnityEngine;
//    using System;
//    using System.Reflection;
//    using System.Collections;
//    using System.Collections.Generic;
//    using Random = UnityEngine.Random;

//    using Bang;
//    /// <summary>
//    /// UtilityAIComponent
//    /// </summary>
//    public class UtilityAIComponent_02 : ExtendedMonoBehaviour
//    {
//        //
//        // Fields
//        //

//        [SerializeField]
//        public UtilityAIConfig[] aiConfigs;

//        [SerializeField]
//        private List<IUtilityAIClient> _clients = new List<IUtilityAIClient>();

//        # region Debug

//        [Header(" -------- Debug -------- ")]
//        [SerializeField] bool debugNextIntervalTime;
//        bool isRunning = true;
//        [HideInInspector] public bool showDefaultInspector, showDeleteAssetOption, selectAiAssetOnCreate;

//        # endregion

//        //
//        // Properties
//        //

//        /// <summary>
//        /// For AddClientWindow
//        /// </summary>
//        /// <value>The clients.</value>
//        public List<IUtilityAIClient> clients
//        {
//            get { return _clients; }
//        }


//        //
//        // Methods
//        //

//        private void OnDisable()
//        {
//            for (int i = 0; i < clients.Count; i++)
//            {
//                clients[i].Stop();
//            }
//        }

//        /// <summary>
//        /// Instantiates and starts the Utility AI Client
//        /// </summary>
//        protected override void OnStartAndEnable()
//        {



//        }


//        public IUtilityAIClient GetClient(Guid aiId)
//        {
//            foreach (IUtilityAIClient client in clients)
//            {
//                if (client.ai.id == aiId)
//                {
//                    Debug.Log("Client:  " + client);
//                    return null;
//                }
//            }

//            Debug.Log("Did not find a client");

//            return null;
//        }


//        public void Pause()
//        {
//            //isRunning = false;
//            for (int i = 0; i < clients.Count; i++)
//            {
//                clients[i].Pause();
//            }
//        }


//        public void Resume()
//        {
//            //isRunning = true;
//            for (int i = 0; i < clients.Count; i++)
//            {
//                clients[i].Resume();
//            }
//        }


//        public void ToggleActive(int idx, bool active)
//        {
//            if (idx < aiConfigs.Length)
//                aiConfigs[idx].isActive = active;
//        }






//        public IEnumerator ExecuteUpdate(UtilityAIClient client)
//        {
//            float nextInterval = 1f;

//            //Debug.Log(" Waiting for startDelay ");
//            //yield return new WaitForSeconds(Random.Range(client.startDelayMin, client.startDelayMax));

//            while (isRunning)
//            {
//                if (Time.timeSinceLevelLoad + 0.25f > nextInterval)
//                {
//                    client.Execute();
//                    nextInterval = Time.timeSinceLevelLoad + Random.Range(client.intervalMin, client.intervalMax);
//                    if (debugNextIntervalTime) Debug.Log("Current Time:  " + Time.timeSinceLevelLoad + " | Next interval in:  " + (nextInterval - Time.timeSinceLevelLoad));

//                }
//                yield return null;
//            }
//        }


//        #region OnStartAndEnable

//        //protected void OnStartAndEnable_1()
//        //{
//        //    if (aiConfigs.Length != clients.Count)
//        //    {
//        //        //AINameMap.RegenerateNameMap();
//        //        _clients.Clear();

//        //        for (int i = 0; i < aiConfigs.Length; i++)
//        //        {
//        //            //  Get Guid of aiID.
//        //            string aiClientName = aiConfigs[i].aiId;
//        //            var field = typeof(AINameMapHelper).GetField(aiClientName, BindingFlags.Public | BindingFlags.Static);
//        //            Guid guid = (Guid)field.GetValue(null);

//        //            //Guid guid = AINameMap.GetGUID(aiConfigs[i].aiId);
//        //            if (guid == Guid.Empty)
//        //            {
//        //                Debug.LogFormat("<color=red> AINameMap does not contain aiId ( {0} ) </color>", aiConfigs[i].aiId);
//        //                continue;
//        //            }

//        //            //  Create a new IUtilityAIClient
//        //            IUtilityAIClient client = new UtilityAIClient(guid, GetComponent<IContextProvider>(),
//        //                                                          aiConfigs[i].intervalMin, aiConfigs[i].intervalMax,
//        //                                                          aiConfigs[i].startDelayMin, aiConfigs[i].startDelayMax);
//        //            client.debugClient = aiConfigs[i].debug;

//        //            //  Add the client to the TaskNetwork.
//        //            clients.Add(client);


//        //            if (aiConfigs[i].isPredefined)
//        //            {
//        //                //  Initialize AIConfig so we can get the name of the predefined config.
//        //                Type type = Type.GetType(aiConfigs[i].type);
//        //                IUtilityAIAssetConfig config = (IUtilityAIAssetConfig)Activator.CreateInstance(type);
//        //                //  Configure the predefined settings.
//        //                config.SetupAI(client.ai);
//        //            }
//        //        }
//        //    }


//        //    for (int i = 0; i < aiConfigs.Length; i++)
//        //    {
//        //        if (aiConfigs[i].isActive)
//        //        {
//        //            //Debug.Log(aiConfigs[i].aiId + " is active and running.\nDebugClient is set to " + aiConfigs[i]._debugClient);
//        //            if (clients[i].state != UtilityAIClientState.Running)
//        //            {
//        //                clients[i].Start();
//        //            }
//        //            StartCoroutine(ExecuteUpdate((UtilityAIClient)clients[i]));
//        //        }
//        //    }
//        //}

//        #endregion

//    }
//}

