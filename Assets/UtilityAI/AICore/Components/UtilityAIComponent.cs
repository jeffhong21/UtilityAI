namespace AtlasAI
{
    using UnityEngine;
    using System;
    using System.Collections;


    using Bang;
    /// <summary>
    /// UtilityAIComponent
    /// </summary>
    public class UtilityAIComponent : ExtendedMonoBehaviour
    {
        //
        // Fields
        //

        [SerializeField]
        private string serializedGuid;
        public UtilityAIConfig[] aiConfigs;
        private IUtilityAIClient[] _clients;



        # region Debug
        [Header(" -------- Debug -------- ")]
        [SerializeField] bool debugNextIntervalTime;
        bool isRunning = true;
        [HideInInspector] public bool showDefaultInspector, showDeleteAssetOption, selectAiAssetOnCreate;
        # endregion


        //
        // Properties
        //

        /// <summary>
        /// For AddClientWindow
        /// </summary>
        /// <value>The clients.</value>
        public IUtilityAIClient[] clients{
            get { return _clients; }
        }


        //
        // Methods
        //

        private void OnDisable()
        {
            if(clients != null){
                for (int i = 0; i < clients.Length; i++){
                    clients[i].Stop();
                }
            }
        }


        /// <summary>
        /// Instantiates and starts the Utility AI Client
        /// </summary>
		protected override void OnStartAndEnable()
        {
            _clients = new IUtilityAIClient[aiConfigs.Length];

            for (int i = 0; i < aiConfigs.Length; i++){
                //  Get Guid of the aiConfig.
                string aiClientName = aiConfigs[i].aiId;
                var field = typeof(AINameMap).GetField(aiClientName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                Guid guid = (Guid)field.GetValue(null);
                clients[i] = new UtilityAIClient(guid, GetComponent<IContextProvider>(),
                                 aiConfigs[i].intervalMin, aiConfigs[i].intervalMax,
                                 aiConfigs[i].startDelayMin, aiConfigs[i].startDelayMax);

                //  Start UtilityAIClient
                clients[i].Start();
                //  If aiConfig is not active, pauce client.
                if(aiConfigs[i].isActive == false){
                    clients[i].Pause();
                    continue;
                }

                clients[i].debugClient = aiConfigs[i].debug;
            }
        }


        public IUtilityAIClient GetClient(Guid aiId)
        {
            if(clients != null){
                for (int i = 0; i < clients.Length; i++){
                    if (clients[i].ai.id == aiId){
                        Debug.Log("Client:  " + clients[i]);
                        return null;
                    }
                }
            }

            Debug.Log("Did not find a client");
            return null;
        }


        public void Pause()
        {
            //isRunning = false;
            for (int i = 0; i < clients.Length; i++){
                clients[i].Pause();
            }
        }


        public void Resume()
        {
            //isRunning = true;
            for (int i = 0; i < clients.Length; i++){
                clients[i].Resume();
            }
        }


        public void ToggleActive(int idx, bool active)
        {
            if (idx < aiConfigs.Length){
                aiConfigs[idx].isActive = active;
                if (active && clients[idx].state == UtilityAIClientState.Pause)
                    clients[idx].Resume();
                else
                    clients[idx].Pause();
            }                  
        }






    }
}

