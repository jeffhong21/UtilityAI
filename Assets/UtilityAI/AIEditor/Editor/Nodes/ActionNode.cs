using System;
using System.Collections.Generic;
using UnityEngine;


namespace AtlasAI.AIEditor
{
    [Serializable]
    public class ActionNode : Node
    {
        public QualifierNode parent;
        private IAction _action;
        private Type _actionType;

        //
        // Properties
        //
        internal IAction action
        {
            get;
            set;
        }

        public string friendlyName
        {
            get
            {
                if (_actionType != null)
                    return _actionType.Name;
                return name;
            }
        }

        //
        // Static Methods
        //
        public static ActionNode Create(Type actionType)
        {
            ActionNode node = CreateInstance<ActionNode>();
            node._actionType = actionType;
            node.name = actionType.Name;

            return node;
        }
    }
}
