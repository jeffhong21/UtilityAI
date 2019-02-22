namespace AtlasAI.Visualization
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;

    [ExecuteInEditMode]
    public class VisualizerComponent : MonoBehaviour
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
            var ai = new List<IContextProvider>();
            if (ai != null)
            {
                var count = ai.Count;
                for (int i = 0; i < count; i++)
                {
                    contextsBuffer.Add(ai[i].GetContext(relevantAIId));
                }
            }

        }


        private void OnEnable()
        {
            object[] selection = UnityEditor.Selection.GetFiltered<IContextProvider>(UnityEditor.SelectionMode.Editable | UnityEditor.SelectionMode.TopLevel);


            //Debug.Log(selection + " | Count: " + selection.Length);

        }

    }


}