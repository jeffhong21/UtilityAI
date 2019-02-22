namespace AtlasAI
{
    using UnityEngine;
    using UnityEditor;

    using System;
    using System.Collections.Generic;
    using AtlasAI.Scheduler;

    public enum UtilityAIClientState
    {
        Running,  ///   The associated AI is running.
        Stopped,  ///   The associated AI is not running.
        Pause     ///   The associated AI is paused.
    }

    /// <summary>
    /// This is the decision maker.
    /// </summary>
    public class UtilityAIClient : IUtilityAIClient, IClientScheduler
    {
        //  For Debuging.
        private string debugColor;
        public bool debugClient{ get; set; }

        //  ContextProvider from BANG.
        private Bang.AIContextProvider contextProvider;


        private ISchedulerHandle _handle;
        private IUtilityAI _ai;
        private UtilityAIClientState _state;
        private IAction _activeAction;


        public IUtilityAI ai{ 
            get{return _ai;}
            set{ _ai = value;}
        }

        public UtilityAIClientState state{ 
            get{return _state; }
            protected set{_state = value;}
        }

        public IAction activeAction{
            get { return _activeAction; }
            protected set { _activeAction = value; }
        }


        public float intervalMin { 
            get;
            set;
        }

        public float intervalMax{
            get;
            set;
        }

        public float startDelayMin{
            get;
            set;
        }

        public float startDelayMax{
            get;
            set;
        }



        #region Constructors

        public UtilityAIClient(Guid aiId, IContextProvider contextProvider) 
            : this(aiId, contextProvider, 1, 1, 0, 0){
        }


        public UtilityAIClient(IUtilityAI ai, IContextProvider contextProvider)
            : this(ai, contextProvider, 1, 1, 0, 0){
        }


        public UtilityAIClient(Guid aiId, IContextProvider contextProvider, float intervalMin, float intervalMax, float startDelayMin, float startDelayMax)
        {
            ai = AIManager.GetAI(aiId);
            this.contextProvider = contextProvider as Bang.AIContextProvider;

            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.startDelayMin = startDelayMin;
            this.startDelayMax = startDelayMax;
            state = UtilityAIClientState.Stopped;

            //  Add the client to the scheduler.
            var interval = UnityEngine.Random.Range(intervalMin, intervalMax);
            var delay = UnityEngine.Random.Range(startDelayMin, startDelayMax);
            var manager = Utilities.ComponentHelper.FindFirstComponentInScene<UtilityAIManager>();
            _handle = manager.scheduler.Add(this, interval, delay);

            //float _intervalRandom = (float)new System.Random().NextDouble() * (intervalMin - intervalMax) + intervalMin;
            //Debug.LogFormat("Custom Random Range: Min: {0}, Max: {1} | Output: {2}", intervalMin, intervalMax, _intervalRandom);
            if (ai == null) Debug.LogError("Could not find the correct ai with the provided aiId");

            //Debug.Log("Constructing UtilityAIClient with aiId - v2");

            //string path = AssetDatabase.GUIDToAssetPath(aiId.ToString("N"));
            //AIStorage aiStorage = AssetDatabase.LoadMainAssetAtPath(path) as AIStorage;
            //ai = new UtilityAI(){
            //    name = aiStorage.aiId
            //};
            //ai = aiStorage.configuration;
        }


        public UtilityAIClient(IUtilityAI ai, IContextProvider contextProvider, float intervalMin, float intervalMax, float startDelayMin, float startDelayMax)
        {
            this.ai = ai;
            this.contextProvider = contextProvider as Bang.AIContextProvider;

            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.startDelayMin = startDelayMin;
            this.startDelayMax = startDelayMax;
            state = UtilityAIClientState.Stopped;

            //  Add the client to the scheduler.
            var interval = UnityEngine.Random.Range(intervalMin, intervalMax);
            var delay = UnityEngine.Random.Range(startDelayMin, startDelayMax);
            var manager = Utilities.ComponentHelper.FindFirstComponentInScene<UtilityAIManager>();
            _handle = manager.scheduler.Add(this, interval, delay);

            //Debug.Log("Contructor with all variables");

            //var random = new System.Random();
            //debugColor = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
        }

        #endregion


        Visualization.ICustomVisualizer visualizer;
        /// <summary>
        /// Executes the AI. Typically this is called by whatever manager controls the AI execution cycle.
        /// </summary>
        public void Execute()
        {
            //Debug.LogFormat("<color={0}>AI {1}</color> is Executing Update. | LastUpdate: {2} | NextUpdate: {3}", debugColor, ai.name, deltaTime, nextInterval);
            activeAction = ai.Select(contextProvider.GetContext());
            activeAction.Execute(contextProvider.GetContext());




            if(debugClient)
            {
                //Debug.LogFormat("BaseType: {0}   |   {1}", activeAction.GetType().BaseType, activeAction.GetType().BaseType == typeof(ActionWithOptions<Vector3>));
                if(activeAction.GetType().BaseType == typeof(ActionWithOptions<Vector3>)){
                    Debug.LogFormat("BaseType: {0} | Type: {1}", activeAction.GetType().BaseType, activeAction.GetType());
                }

                //if (activeAction.GetType() == typeof(ActionWithOptions<>)){
                //    Debug.Log(activeAction.GetType().Name);
                //}
            }

            visualizer = null;
            if (Visualization.VisualizerManager.TryGetVisualizerFor(activeAction.GetType(), out visualizer))
            {
                //Debug.LogFormat("There is a visualizer for Action {0} |", activeAction.GetType());
                visualizer.EntityUpdate(activeAction, contextProvider.GetContext(), ai.id);
            }

            //if (activeAction is CompositeAction){
            //    for (int i = 0; i < ((CompositeAction)activeAction).actions.Count; i++){
            //        if (Visualization.VisualizerManager.TryGetVisualizerFor(((CompositeAction)activeAction).actions[i].GetType(), out visualizer)){
            //            visualizer.EntityUpdate(activeAction, contextProvider.GetContext());
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    if (Visualization.VisualizerManager.TryGetVisualizerFor(activeAction.GetType(), out visualizer)){
            //        visualizer.EntityUpdate(activeAction, contextProvider.GetContext());
            //    }
            //}
        }




        /// <summary>
        /// Executes the update.
        /// </summary>
        /// <param name="deltaTime">The delta time, i.e. the time passed since the last update.</param>
        /// <param name="nextInterval">The time that will pass until the next update.</param>
        /// <returns>
        /// Can return the next interval by which the update should run.To use the default interval return null.
        /// </returns>
        public float? ExecuteUpdate(float deltaTime, float nextInterval)
        {
            if (state == UtilityAIClientState.Running){
                Execute();
                //if (debugClient) Debug.LogFormat("Executing {0}.  Action is of type {1}", activeAction.name, activeAction.GetType());
                //if(deltaTime < nextInterval)
                //nextInterval = UnityEngine.Random.Range(intervalMin, intervalMax);
                return UnityEngine.Random.Range(intervalMin, intervalMax);
            }

            //Debug.LogFormat("AI: {0} | State: {1}",ai.name, state);
            return null;
        }


        /// <summary>
        /// Called to initialize AIClient
        /// </summary>
        public void Start()
        {
            //  UtilityAIClient trying to start, but it not in the correct state.
            if (state != UtilityAIClientState.Stopped){
                //Console.WriteLine(" ** UtilityAIClient trying to start, but it not in the correct state.");
                return;
            }
            //  Set the correct client state.
            state = UtilityAIClientState.Running;
            //  Register the client to the AIManager.
            AIManager.Register(this);

        }


        public void Stop()
        {
            //  UtilityAIClient trying to stop, but is currently stopped.
            if (state == UtilityAIClientState.Stopped){
                //Console.WriteLine(" ** UtilityAIClient trying to stop, but is currently stopped.");
                return;
            }
            //  Set the correct client state.
            state = UtilityAIClientState.Stopped;
            //  Remove itself from the scheduler.
            _handle.Stop();
            //  Unregister the client to the AIManager.
            AIManager.UnRegister(this);

        }


        public void Resume()
        {
            if (state != UtilityAIClientState.Pause)
                return;
            
            state = UtilityAIClientState.Running;
            //  Resume schedule updates.
            //_handle.Resume();

        }


        public void Pause()
        {
            if (state != UtilityAIClientState.Running)
                return;
            
            state = UtilityAIClientState.Pause;
            //  Pause from scheduled updates.
            //_handle.Pause();

        }








        
        public Dictionary<IQualifier, float> selectorResults;

        /// <summary>
        /// For Debugging
        /// </summary>
        public Dictionary<IQualifier, float> GetSelectorResults(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifer)
        {
            if (selectorResults == null)
                selectorResults = new Dictionary<IQualifier, float>();
            else
                selectorResults.Clear();

            for (int index = 0; index < qualifiers.Count; index++)
            {
                CompositeQualifier qualifier = qualifiers[index] as CompositeQualifier;
                var score = qualifier.Score(context, qualifier.scorers);
                selectorResults.Add(qualifier, score);
            }


            var dq = defaultQualifer as IQualifier;
            selectorResults.Add(dq, defaultQualifer.Score(context));

            return selectorResults;
        }



    }
}
