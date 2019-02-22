namespace AtlasAI.AIEditor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(AIStorage)) ]
    public class AIStorageEditor : Editor
    {
        private bool _debug;


		public void OnEnable()
		{
            hideFlags = HideFlags.HideAndDontSave;

		}


		public override void OnInspectorGUI()
		{
            AIStorage aiStorage = target as AIStorage;
            //if (AIInspectorEditor.instance)
            //    AIInspectorEditor.instance.OnInspectorGUI();
            //else
            //EditorGUILayout.HelpBox(aiStorage.editorConfiguration, MessageType.None);


            EditorGUILayout.HelpBox(aiStorage.editorConfiguration, MessageType.None);

            if(string.IsNullOrWhiteSpace(aiStorage.editorConfiguration))
            {
                if (GUILayout.Button("Reset EditorConfigurations"))
                {
                    aiStorage.editorConfiguration = "";
                }
            }

		}
	}
}
