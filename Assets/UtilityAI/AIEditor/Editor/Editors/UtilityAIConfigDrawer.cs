namespace AtlasAI
{
    using UnityEngine;
    using UnityEditor;


    [CustomPropertyDrawer(typeof(UtilityAIConfig))]
    public class UtilityAIConfigDrawer : PropertyDrawer
    {
        //  Padding for rect.y
        private readonly int linePadding = 1;

        private readonly int boolSize = 28;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
            EditorGUI.BeginProperty(position, label, property);


            SerializedProperty isActive = property.FindPropertyRelative("isActive");
            SerializedProperty aiId = property.FindPropertyRelative("aiId");
            SerializedProperty intervalMin = property.FindPropertyRelative("intervalMin");
            SerializedProperty intervalMax = property.FindPropertyRelative("intervalMax");
            SerializedProperty startDelayMin = property.FindPropertyRelative("startDelayMin");
            SerializedProperty startDelayMax = property.FindPropertyRelative("startDelayMax");

            Rect rect = new Rect(position)
            {
                height = EditorGUIUtility.singleLineHeight
            };

            //rect.width = boolSize;

            //rect.x += rect.width;
            rect.width = position.width * 0.33f;
            EditorGUI.LabelField(rect, new GUIContent("AI: "));
            rect.x += rect.width;
            EditorGUI.LabelField(rect, new GUIContent(aiId.stringValue));
            //EditorGUI.PropertyField(rect, aiId);

            rect.x = position.x;
            rect.y += EditorGUIUtility.singleLineHeight + linePadding;
            rect.width = position.width * 0.33f;
            EditorGUI.LabelField(rect, new GUIContent("Interval: "));
            rect.x += rect.width;
            rect.width = 35f;
            EditorGUI.PropertyField(rect, intervalMin, GUIContent.none);
            rect.x += rect.width;
            rect.width = 20f;
            EditorGUI.LabelField(rect, new GUIContent("to: "));
            rect.x += rect.width;
            rect.width = 35f;
            EditorGUI.PropertyField(rect, intervalMax, GUIContent.none);

            rect.x = position.x;
            rect.y += EditorGUIUtility.singleLineHeight + linePadding;
            rect.width = position.width * 0.33f;
            EditorGUI.LabelField(rect, new GUIContent("Start Delay: "));
            rect.x += rect.width;
            rect.width = 35f;
            EditorGUI.PropertyField(rect, startDelayMin, GUIContent.none);
            rect.x += rect.width;
            rect.width = 20f;
            EditorGUI.LabelField(rect, new GUIContent("to: "));
            rect.x += rect.width;
            rect.width = 35f;
            EditorGUI.PropertyField(rect, startDelayMax, GUIContent.none);

            EditorGUI.EndProperty();

		}


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
            //return base.GetPropertyHeight(property, label);
            return (EditorGUIUtility.singleLineHeight + linePadding) * 3;
		}



	}

}