namespace AtlasAI.AIEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class AIEntitySelectorWindow : SelectorWindow<AIBuildingBlocks.NamedType>
    {
        //
        // Fields
        //
        private GUIContent _listItemContent;
        private Action<Type> _singleCallback;


        //
        // Static Methods
        //
        public static void Get<T>(Vector2 screenPosition, Action<Type> callback)
        {
            var title = "";
            if(typeof(T) == typeof(Selector)) title = "Selector | AI Object Window";
            else if (typeof(T) == typeof(IQualifier)) title = "Qualifier | AI Object Window";
            else title = "AI Object Window";

            var window = GetWindow<AIEntitySelectorWindow>(screenPosition, title);
            window._singleCallback = callback;


            window.Show(typeof(T), callback);
        }



        //
        // Methods
        //

        private void OnSelect(AIBuildingBlocks.NamedType[] items)
        {
            if(_singleCallback != null)
                _singleCallback(items[0].type);
            else{
                if(items[0].type == typeof(Selector))
                {
                    Debug.LogFormat("Created {0}", items[0].friendlyName );
                }
                else if (items[0].type == typeof(IQualifier))
                {
                    Debug.LogFormat("Created {0}", items[0].friendlyName);
                }
            }
        }

        private GUIContent RenderListItem(AIBuildingBlocks.NamedType entity)
        {
            return new GUIContent(entity.friendlyName, entity.description);
        }


        private void Show(Type baseType, Action<Type> callback)
        {
            Show(AIBuildingBlocks.Instance.GetForType(baseType), RenderListItem, OnSelect);
        }

    }
}
