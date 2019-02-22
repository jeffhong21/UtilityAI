using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace AtlasAI.AIEditor
{
    [Serializable]
    public class QualifierNode : Node
    {
        //
        // Fields
        //
        public SelectorNode parent;
        public ActionNode actionNode;
        private IQualifier _qualifier;
        private Type _qualifierType;
        private bool _expanded;



        //
        // Properties
        //
        public IQualifier qualifier
        {
            get { return _qualifier; }
            set { _qualifier = value; }
        }

        public string friendlyName{
            get{
                if (Attribute.GetCustomAttribute(_qualifierType, typeof(FriendlyNameAttribute)) != null)
                    return _qualifierType.GetCustomAttribute<FriendlyNameAttribute>().name;
                else if (_qualifierType != null)
                    return _qualifierType.Name;
                return name;
            }
        }

        public bool isDefault{
            get{
                if (parent == null) return false;
                return parent.defaultQualifierNode == this;
            }
        }

        public bool isExpanded{
            get { return _expanded; }
            set { _expanded = value; }
        }

        public bool isHighScorer{
            get;
        }



        //
        // Static Methods
        //
        public static QualifierNode Create(Type qualifierType){
            QualifierNode node = CreateInstance<QualifierNode>();
            node._qualifierType = qualifierType;
            //  TODO: Need to ge tthe correct qualifier.  

            //  TODO: Need to set the selector node..  
            node.name = qualifierType.Name;


            return node;
        }

    }
}
