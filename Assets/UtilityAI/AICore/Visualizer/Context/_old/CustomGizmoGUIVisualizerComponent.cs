//namespace AtlasAI.Visualization
//{
//    using System;
//    using System.Collections.Generic;


//    /// <summary>
//    /// The next step in custom visualization. This base class builds on the previous one, and adds abstract 
//    /// methods for Gizmos and OnGUI visualization, as well as options for toggling each type of visualization. 
//    /// Use this base class if providing visualizations for a qualifier.
//    /// </summary>
//    public abstract class CustomGizmoGUIVisualizerComponent<T, TData> : CustomVisualizerComponent<T, TData> where T : class
//    {
//        //
//        // Fields
//        //
//        public bool drawGizmos;
//        public bool drawGUI;

//        //
//        // Constructors
//        //
//        protected CustomGizmoGUIVisualizerComponent()
//        {

//        }

//        //
//        // Methods
//        //

//        private void OnDrawGizmos()
//        {

//            if(_data != null && drawGizmos)  // && VisualizerManager.isVisualizing
//            {
//                //UnityEngine.Debug.Log(_data.Count);
//                foreach (IAIContext key in _data.Keys){
//                    UnityEngine.Debug.Log(key);
//                    DrawGizmoData(key);
//                }
//            }

//        }


//        private void OnGUI()
//        {
//            if (_data != null && drawGUI)  // && VisualizerManager.isVisualizing
//            {
//                foreach (IAIContext key in _data.Keys){
//                    DrawGUIData(key);
//                }
//            }
//        }


//        private void DrawGizmoData(IAIContext context)
//        {
//            DrawGizmos(_data[context]);
//        }


//        private void DrawGUIData(IAIContext context)
//        {
//            DrawGUI(_data[context]);
//        }


//        protected abstract void DrawGizmos(TData data);

//        protected abstract void DrawGUI(TData data);
//    }
//}