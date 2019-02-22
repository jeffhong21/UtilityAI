namespace AtlasAI.Visualization
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The base class for all custom visualizers.   Provides a single method to override, which is called every time the specified type of AI entity is executed by the AI.
    /// </summary>
    /// <typeparam name="T">T is the type of AI entity to visualize, e.g. a specific or base-class Action.</typeparam>
    /// <typeparam name = "TData" > TData is the type of data to visualize and can be virtually anything. </typeparam>
    public abstract class CustomVisualizerComponent<T, TData> : ContextVisualizerComponent, ICustomVisualizer where T : class
    {
        //
        // Fields
        //
        protected Dictionary<IAIContext, TData> _data;

        public bool drawGizmos;
        public bool drawGUI;



        //
        // Methods
        //
        protected override void Awake()
        {
            base.Awake();
            _data = new Dictionary<IAIContext, TData>();
        }


        protected virtual void OnEnable()
        {
            if (_data == null) _data = new Dictionary<IAIContext, TData>();
            VisualizerManager.RegisterVisualizer<T>(this);
            //Debug.Log(VisualizerManager.DebugLogRegisteredVisualziers());
        }

        protected virtual void OnDisable()
        {
            _data = null;
            VisualizerManager.UnregisterVisualizer<T>();
            //Debug.Log(VisualizerManager.DebugLogRegisteredVisualziers());
        }



        public void EntityUpdate(object aiEntity, IAIContext context, Guid aiId)
        {
            ////  Call event for GetDataForVisualization(activeAction, contextProvider.GetContext())
            //VisualizerManager.UpdateVisualizer(activeAction, contextProvider.GetContext());

            if(_data.ContainsKey(context)){
                _data[context] = GetDataForVisualization((T)aiEntity, context, aiId);
            }
            else{
                TData value = GetDataForVisualization((T)aiEntity, context, aiId);
                _data.Add(context, value);
            }
        }


        /// <summary>
        /// Monobehavior method.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_data != null && drawGizmos){
                foreach (IAIContext key in _data.Keys){
                    DrawGizmoData(key);
                }
            }
        }


        /// <summary>
        /// Monobehavior method.
        /// </summary>
        private void OnGUI()
        {
            if (_data != null && drawGUI){
                foreach (IAIContext key in _data.Keys){
                    DrawGUIData(key);
                }
            }
        }


        private void DrawGizmoData(IAIContext context)
        {
            DrawGizmos(_data[context]);
        }


        private void DrawGUIData(IAIContext context)
        {
            DrawGUI(_data[context]);
        }




        protected abstract TData GetDataForVisualization(T aiEntity, IAIContext context, Guid aiId);

        protected abstract void DrawGizmos(TData data);

        protected abstract void DrawGUI(TData data);
    }
}
