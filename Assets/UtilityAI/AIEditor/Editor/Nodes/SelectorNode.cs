using System;
using System.Collections.Generic;
using UnityEngine;


namespace AtlasAI.AIEditor
{
    [Serializable]
    public class SelectorNode : TopLevelNode
    {
        //
        // Fields
        //
        private Selector _selector;
        private Type _selectorType;

        public List<QualifierNode> qualifierNodes;
        public QualifierNode defaultQualifierNode;

        //
        // Properties
        //
        public IEnumerable<QualifierNode> AllQualifierNodes{
            get{
                foreach (var node in qualifierNodes){
                    yield return node;
                }
            }
        }

        public string friendlyName{
            get{
                return name;
            }
        }

        public bool isRoot{
            get{
                //if (parentUI.rootSelector == _selector) return true;
                return false;
            }
        }

        public Selector selector
        {
            get { return _selector; }
            set { _selector = value; }
        }



        //
        // Static Methods
        //
        public static SelectorNode Create(Type selectorType, AIUI parent, Rect viewArea)
        {
            //var node = new SelectorNode(viewArea);
            var node = CreateInstance<SelectorNode>();
            node._selector = Activator.CreateInstance(selectorType) as Selector;
            node._selectorType = selectorType;
            node.name = selectorType.Name;
            node.parent = parent;
            node.parentUI = parent;
            node.viewArea = viewArea;
            node.defaultQualifierNode = QualifierNode.Create(typeof(DefaultQualifier));
            node.qualifierNodes = new List<QualifierNode>();
            return node;
        }


        //
        // Methods
        //
        public void AddElement(QualifierNode element){
            element.parent = this;
            qualifierNodes.Add(element);
        }


        public void RemoveElement(QualifierNode element){
            if(qualifierNodes.Contains(element)){
                qualifierNodes.Remove(element);
                element.parent = null;
            } else {
                throw new InvalidOperationException(string.Format("{0} is not a child of {1}", element, this));
            }
        }


        public Vector3 ConnectorAnchorOut(int qualifierIdx, NodeSettings settings)
        {
            throw new NotImplementedException();
        }


        public override void RecalcHeight(NodeSettings settings)
        {
            var elementHeight = settings.qualifierHeight + settings.actionHeight;
            var qualifierHeight = qualifierNodes.Count * elementHeight;
            var defaultQualifierHeight = elementHeight;
            viewArea.height = settings.titleHeight + qualifierHeight + defaultQualifierHeight;
            //viewArea.y = viewArea.y - qualifierHeight - defaultQualifierHeight;
        }

        public bool Reconnect(Selector s)
        {
            throw new NotImplementedException();
        }


    }
}
