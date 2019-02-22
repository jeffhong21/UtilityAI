namespace AtlasAI.AIEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;


    public abstract class SelectorWindow<T> : EditorWindow
    {
        private const float winWidth = 250f;
        private const float winHeight = 350;
        //
        // Fields
        //
        private IEnumerable<T> _itemList;
        private List<T> _listView;
        private bool _allowNoneSelection;
        private Action<T[]> _onSelect;
        private HashSet<int> _selectedIndexes;

        private Func<T, GUIContent> _itemRenderer;
        GUIStyle contentStyle;

        //
        // Static Methods
        //
        protected static TWin GetWindow<TWin>(Vector2 screenPosition, string title) where TWin : EditorWindow
        {
            //  Initialize window
            Rect rect = new Rect(screenPosition, new Vector2(winWidth, winHeight));
            TWin window = GetWindowWithRect<TWin>(rect, true, title);
            return window;
        }



        //
        // Methods
        //

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if(_itemList != null){
                    Render(position, _itemList);
                }

            }
        }


        private void Render(Rect rect, IEnumerable<T> itemList, float elementHeight = 20f, float elementPadding = 1f)
        {
            rect.x += elementHeight;
            rect.width -= elementPadding * 2;
            rect.height = elementHeight + (elementPadding * 2);

            foreach (var item in itemList){
                if (GUILayout.Button(_itemRenderer(item), contentStyle, GUILayout.Height(elementHeight)))
                {
                    T[] asset = { item };
                    _onSelect(asset);
                    SafeClose();
                }
                rect.y += rect.height;
            }
        }


        private void OnLostFocus()
        {
            
        }


        private void SafeClose()
        {
            EditorUtility.UnloadUnusedAssetsImmediate();
            Close();
        }


        //protected void Show(IEnumerable<T> items, Func<T, GUIContent> itemRenderer, Func<T, string, bool> searchPredicate, bool allowNoneSelection, bool allowMultiSelect, Action<T[]> onSelect = null)
        protected void Show(IEnumerable<T> items, Func<T, GUIContent> itemRenderer, Action<T[]> onSelect = null)
        {
            _itemList = items;
            _onSelect = onSelect;
            _itemRenderer = itemRenderer;

            contentStyle = new GUIStyle(GUI.skin.button);
            contentStyle.alignment = TextAnchor.MiddleCenter;
        }
    }


}
