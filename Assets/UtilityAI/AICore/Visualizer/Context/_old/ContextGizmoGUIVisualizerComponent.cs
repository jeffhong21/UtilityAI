//namespace AtlasAI.Visualization
//{
//    using UnityEngine;
//    using UnityEditor;

//    using System.Collections.Generic;

//    /// <summary>
//    /// Context gizmo GUIV isualizer component.
//    /// </summary>
//    /// <typeparam name="T">The context type.</typeparam>
//    public abstract class ContextGizmoGUIVisualizerComponent : MonoBehaviour
//    {
//        public bool drawGUI = true;

//        public bool drawGizmo = true;

//        [SerializeField]
//        //[SerializeField, HideInInspector]
//        protected IContextProvider contextProvider;



//        protected virtual void Awake()
//		{
//            if (contextProvider == null)
//                contextProvider = gameObject.GetComponent<IContextProvider>();
//		}

//        protected void OnEnable()
//        {
//            if (contextProvider == null)
//                contextProvider = gameObject.GetComponent<IContextProvider>();
//        }

//        protected void OnDisable()
//		{
			
//		}




//        protected virtual void OnGUI()
//        {
//            if (contextProvider != null && drawGUI == true && Application.isEditor){
//                DrawGUI(contextProvider.GetContext());
//                //if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
//                //    DrawGUI(contextProvider.GetContext());
//                //}
//            }
//        }

//        protected virtual void OnDrawGizmos()
//        {
//            if (contextProvider != null && drawGizmo == true && Application.isEditor){
//                DrawGizmos(contextProvider.GetContext());
//                //if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
//                //    DrawGizmos(contextProvider.GetContext());
//                //}
//            }
//        }


//        private void DrawGizmoData(IAIContext context){
//            DrawGizmos(context);
//        }


//        private void DrawGUIData(IAIContext context){
//            DrawGUI(context);
//        }


//        protected abstract void DrawGUI(IAIContext context);

//        protected abstract void DrawGizmos(IAIContext context);




//    }


//}

