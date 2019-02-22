using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using AtlasAI.Visualization;

namespace AtlasAI
{
    [CustomEditor(typeof(ContextVisualizerComponent), true)]
    public class ContextGizmoVisualizerComponentEditor : Editor
    {
        //
        // Static Fields
        //
        private static readonly GUIContent _relevantAILabel;

        private static readonly GUIContent _relevantAIValue;

        private static readonly GUIContent _modeLabel;

        //
        // Fields
        //
        private ContextVisualizerComponent _target;

        private string _relevantAiName;

        private List<SerializedProperty> _props;

        //
        // Constructors
        //
        public ContextGizmoVisualizerComponentEditor()
        {
            
        }

        //
        // Methods
        //
        private void DrawAISelector()
        {
            
        }

        private void OnEnable()
        {
            //if (Selection.gameObjects.Length > 0)
            //{
            //    VisualizerManager.UpdateSelectedGameObjects(Selection.gameObjects);

            //}
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();


        }
    }
}