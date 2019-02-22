namespace AtlasAI.AIEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class AISelectorWindow : SelectorWindow<AIStorage>
    {
        //
        // Fields
        //
        private GUIContent _listItemContent;
        private Action<AIStorage[]> _multiCallback;

        //
        // Static Methods
        //
        public static AISelectorWindow Get(Vector2 screenPosition, Action<AIStorage[]> callback)
        {
            var window = GetWindow<AISelectorWindow>(screenPosition, "AI Selector Window");
            window._multiCallback = callback;
            IEnumerable<AIStorage> items = Resources.FindObjectsOfTypeAll<AIStorage>();
            window.Show(items, window.RenderListItem, window.OnSelect);
            return window;
        }



        //
        // Methods
        //

        private void OnSelect(AIStorage[] items)
        {
            _multiCallback(items);
        }

        private GUIContent RenderListItem(AIStorage ai)
        {
            return new GUIContent(ai.aiId);
        }


    }
}
