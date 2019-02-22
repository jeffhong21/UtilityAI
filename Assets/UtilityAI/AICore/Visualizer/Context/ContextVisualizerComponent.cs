namespace AtlasAI.Visualization
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Context visualization through a non-GUI and non-Gizmo approach, e.g. writing 
    /// data to a file or using Handles to draw, rather than Gizmos
    /// </summary>
    public class ContextVisualizerComponent : MonoBehaviour
    {
        
        [HideInInspector, SerializeField]
        public string relevantAIId;

        //[HideInInspector, SerializeField]
        public SceneVisualizationMode mode;

        private Guid _relevantGuid;



		protected virtual void Awake()
		{
			
		}


        protected void DoDraw(Action<IAIContext> drawer)
        {

        }


        //  Used to visualize Custom SceneVisualizationMode
        protected virtual void GetContextsToVisualize(List<IAIContext> contextsBuffer, Guid relevantAIId)
        {
        }


		private void OnEnable()
		{
            //object[] selection = UnityEditor.Selection.GetFiltered<IContextProvider>(UnityEditor.SelectionMode.Editable | UnityEditor.SelectionMode.TopLevel);
            //Debug.Log(selection + " | Count: " + selection.Length);

		}

	}



    public enum SceneVisualizationMode
    {
        SingleSelectedGameObject,
        AllSelectedGameObjects,
        Custom
    }
}


