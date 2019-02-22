namespace AtlasAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Given a Guid, AIManager can look up AIData or a List of IUtilityAIClient.
    /// </summary>
    public static class AIManager
    {
        public delegate IUtilityAIClient AIClientResolver(GameObject host, Guid aiId);

        //
        // Static Fields
        //
        public static string TempStorageFolder = "Assets/Scripts/UtilityAI/Resources/AIStorage";
        public static string StorageFolder = "AIStorage";

        private static readonly bool initLock;
        //  Guid == aiID
        private static Dictionary<Guid, AIData> _aiLookup;  //  Uses the Guid of the aiID since each ai should have one utilityAi associated with it.
        //  Guid == utilityAI since there should be one per aiID.
        private static Dictionary<Guid, List<IUtilityAIClient>> _aiClients;  //  All the clients associated with each aiId.


        public static AIClientResolver GetAIClient = 
            (GameObject host, Guid aiId) => {
            if(host.GetComponent<UtilityAIComponent>())
                    return host.GetComponent<UtilityAIComponent>().GetClient(aiId);
            return null;
            };



        //
        // Static Properties
        //
        public static IEnumerable<IUtilityAIClient> allClients
        {
            get{
                if(_aiClients != null){
                    foreach (var ai in _aiClients){
                        for (int i = 0; i < ai.Value.Count; i++){
                            yield return ai.Value[i];
                        }
                    }
                } 
                else{
                    yield return null;
                }
            }
        }



        //
        // Static Methods
        //


        /// <summary>
        /// Loads and initializes all AIs. This means that calling <see cref="M:Apex.AI.AIManager.GetAI(System.Guid)"/> 
        /// will never load AIs on demand and thus won't allocate memory.
        /// </summary>
        public static void EagerLoadAll()
        {
            if (_aiLookup == null) _aiLookup = new Dictionary<Guid, AIData>();
            if (_aiClients == null) _aiClients = new Dictionary<Guid, List<IUtilityAIClient>>();

            //AIStorage[] storedAIs = Resources.FindObjectsOfTypeAll<AIStorage>();
            AIStorage[] storedAIs = Resources.LoadAll<AIStorage>(StorageFolder);
            for (int i = 0; i < storedAIs.Length; i++)
            {
                //  Cache and Initialize objects.
                IUtilityAI utilityAI = new UtilityAI(storedAIs[i].friendlyName);
                AIStorage storedAI = storedAIs[i];
                storedAI.configuration = utilityAI;
                AIData aiData = new AIData(){
                    ai = utilityAI,
                    storedData = storedAI
                };


                //  Initialize AIConfig so we can get the name of the predefined config.
                Type[] types = Utilities.AIReflection.GetTypeByName(storedAI.aiId);
                if(types.Length > 0){
                    Type type = types[0];
                    IUtilityAIAssetConfig config = Activator.CreateInstance(type, storedAI.aiId) as IUtilityAIAssetConfig;
                    //  Configure the predefined settings.
                    config.CreateAI(utilityAI);
                } else {
                    Debug.LogFormat(" ** Could not find aiId type. ");
                    continue;
                }


                var field = typeof(AINameMap).GetField(storedAI.aiId, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                Guid guid = (Guid)field.GetValue(null);
                //  Add to the ai lookup.
                if(_aiLookup.ContainsKey(guid) == false){
                    _aiLookup.Add(guid, aiData);

                }
                if(_aiClients.ContainsKey(utilityAI.id) == false){
                    _aiClients.Add(utilityAI.id, new List<IUtilityAIClient>());
                }
            }

            //Debug.LogFormat("Finish loading. Loaded {0} AIs.", _aiLookup.Count);
        }


        private static void EnsureLookup(bool init)
        {

        }


        private static void ReadAndInit(AIData data)
        {
            //  Initialize AIConfig so we can get the name of the predefined config.
            Type type = Type.GetType(data.storedData.aiId);
            UtilityAIAssetConfig config = (UtilityAIAssetConfig)Activator.CreateInstance(type, data.storedData.aiId);
            //  Configure the predefined settings.
            config.CreateAI(data.storedData.configuration);

            //  Add to the ai lookup.
            if (_aiLookup.ContainsKey(data.ai.id) == false){
                _aiLookup.Add(data.ai.id, data);
            }
        }



        public static IUtilityAI GetAI(Guid id)
        {
            if (_aiLookup == null){
                _aiLookup = new Dictionary<Guid, AIData>();
                Debug.LogFormat(" No UtilityAIs were registered.");
                return null;
            }

            if(_aiLookup.ContainsKey(id)){
                return _aiLookup[id].ai;
            }

            Debug.LogFormat(" Could not find AI with given id.");
            return null;
        }


        /// <summary>
        /// Gets the list of clients for a given AI. Please note that this is a live list that should not be modified directly.
        /// </summary>
        /// <param name="aiId">Ai identifier.</param>
        ///<returns> The list of clients for the specified AI. </returns>
        public static IList<IUtilityAIClient> GetAllClients(Guid aiId)
        {
            if (_aiClients == null){
                _aiClients = new Dictionary<Guid, List<IUtilityAIClient>>();
                Debug.LogFormat(" No AIs were registered with that aiId.");
                return null;
            }

            if (_aiClients.ContainsKey(aiId)){
                return _aiClients[aiId];
            }

            Debug.LogFormat(" Could not clients with that given aiId.");
            return null;
        }


        public static void Register(IUtilityAIClient client)
        {
            if(_aiClients == null) _aiClients = new Dictionary<Guid, List<IUtilityAIClient>>();

            //  If aiClients do not have utilityAI id associated.
            if(_aiClients.ContainsKey(client.ai.id) == false){
                _aiClients.Add(client.ai.id, new List<IUtilityAIClient>());
            }

            //  If there is already a utilityAI id registered, add the client to the list.
            if(_aiClients.ContainsKey(client.ai.id)){
                _aiClients[client.ai.id].Add(client);
            }
        }


        public static void UnRegister(IUtilityAIClient client)
        {
            //  If aiLookup has a client registered.
            if (_aiClients.ContainsKey(client.ai.id)){
                _aiClients[client.ai.id].Remove(client);
            }
        }



        private class AIData
        {
            public IUtilityAI ai;

            public AIStorage storedData;

            public AIData()
            {
                ai = null;
                storedData = null;
            }
        }
        //private class AIData
        //{
        //    public IUtilityAI ai;

        //    public AIStorage storedData;

        //    public AIData()
        //    {
        //        ai = null;
        //        storedData = null;
        //    }
        //}

    }



}