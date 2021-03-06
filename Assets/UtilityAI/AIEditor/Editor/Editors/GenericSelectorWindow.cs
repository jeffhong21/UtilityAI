using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apex.AI.Editor
{
    public sealed class GenericSelectorWindow : SelectorWindow<object>
    {


        //
        // Static Methods
        //
        public static void Show<T>(Vector2 screenPosition, string title, IEnumerable<T> items, 
                                   Func<T, GUIContent> itemRenderer, Action<T[]> onSelect = null) where T : class
        {
            var window = GetWindow<GenericSelectorWindow>(screenPosition, title);
        }




        
        //  Nested Types
        
        private class Handler<T> where T : class
        {
            public GenericSelectorWindow _parent;

            public IEnumerable<T> _items;

            public Action<T[]> _onSelectCallback;

            public Func<T, GUIContent> _itemRenderer;

            public IEnumerable<object> items
            {
                get;
            }

            public GUIContent RenderListItem(object o)
            {
                throw new NotImplementedException();
            }

            public void OnSelect(object[] items)
            {
                
            }

            public Handler()
            {
                
            }
        }



    }
}
