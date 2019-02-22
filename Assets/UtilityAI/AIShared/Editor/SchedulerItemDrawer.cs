namespace AtlasAI.AIEditor
{
    using UnityEditor;
    using UnityEngine;

    using AtlasAI.Scheduler;

    [CustomPropertyDrawer(typeof(SchedulerQueue.SchedulerItem))]
    public class SchedulerItemDrawer : PropertyDrawer
    {
        float labelWidth = 0.2f;
        float width = 0.25f;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            Rect rect = new Rect(position);

            SerializedProperty nextUpdate = property.FindPropertyRelative("nextUpdate");
            SerializedProperty lastUpdate = property.FindPropertyRelative("lastUpdate");
            SerializedProperty interval = property.FindPropertyRelative("interval");



            rect.x += 4;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * labelWidth, rect.height), "Next Update:");
            rect.x += rect.width * labelWidth - 10;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width * width, rect.height), nextUpdate, GUIContent.none);
            rect.x += rect.width * labelWidth + 10;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * labelWidth, rect.height), "Last Update:");
            rect.x += rect.width * labelWidth - 10;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width * labelWidth, rect.height), lastUpdate, GUIContent.none);
            //rect.x += rect.width / 3 - 10;


            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
