using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


namespace AtlasAI.AIEditor
{

    public class NodeLayout
    {
        private Rect _viewRect;
        private NodeSettings _settings; 
        private SelectorNode _selectorNode;



        public float titleHeight{
            get { return _settings.titleHeight; } 
        }

        public Rect viewRect{
            get { return _viewRect; }
        }

        public SelectorNode selectorNode{
            get { return _selectorNode; }
        }


        private NodeSettings settings{
            get { return _settings; }  
        }


        //
        // Constructors
        //
        public NodeLayout(SelectorNode node, NodeSettings nodeSettings)
        {
            _viewRect = node.viewArea;
            _settings = nodeSettings;  
            _selectorNode = node;
        }



        public Rect GetDragAreaLocal(float x, float ypos)
        {
            return new Rect(x, ypos, settings.dragHandleWidth, settings.qualifierHeight);
        }

        public Rect GetContentAreaLocal(float x, float y, float height)
        {
            return new Rect(x + settings.dragHandleWidth, y, 
                            settings.nodeWidth - (settings.dragHandleWidth + settings.toggleAreaWidth + settings.scoreAreaWidth), height);
        }

        public Rect GetScoreAreaLocal(float x, float y, float height)
        {
            return new Rect(settings.nodeWidth - settings.scoreAreaWidth - settings.toggleAreaWidth + x, y, settings.scoreAreaWidth, height);
        }


        public Rect GetToggleAreaLocal(float x, float ypos)
        {
            return new Rect(x, ypos, settings.toggleAreaWidth, settings.qualifierHeight);
        }

        public Rect GetAnchorAreaLocal(float x, float ypos)
        {
            //var heightOffset = 3;
            return new Rect(x + settings.nodeWidth - (settings.anchorAreaWidth * 1), ypos + settings.actionHeight, settings.anchorAreaWidth, settings.actionHeight);
        }



        public QualifierHit GetQualifierAtPosition(Vector2 position)
        {
            var hitinfo = new QualifierHit(this);

            int result = -1;
            float elementHeight = settings.qualifierHeight + settings.actionHeight;
            var offset = viewRect.size;
            offset.y = viewRect.height - titleHeight - elementHeight;

            int count = selectorNode.qualifierNodes.Count;
            float localY = position.y - offset.y;

            float num = 0f;
            for (int i = 0; i < count; i++)
            {
                float num2 = elementHeight;
                float num3 = num + num2;
                if (localY >= num && localY < num3){
                    result = i;
                }
                num += num2;
            }

            hitinfo.index = result;
            hitinfo.offset = offset;
            return hitinfo;
        }



        public bool InTitleArea(Vector2 position)
        {
            var buffer = 4f;
            var rect = viewRect;
            rect.y = rect.y + titleHeight - buffer;
            rect.height = titleHeight;


            //if(rect.Contains(position) == false){
            //    Debug.LogFormat("Position: {0} {5}| Sides: xMin({1}), xMax({2}), yMin({3}), yMax({4})",
            //     position, rect.xMin, rect.xMax, rect.yMin, rect.yMax, rect.Contains(position)); 
            //}

            return rect.Contains(position);
        }



        //
        // Nested Types
        //
        public class QualifierHit
        {
            private NodeLayout parent;

            public int index;
            public Vector2 offset;


            public int clampedIndex{
                get;
            }

            public QualifierNode qualifier
            {
                get{ 
                    if (index >= 0) return parent.selectorNode.qualifierNodes[index];
                    return null;
                }
            }

            public bool InAnchorArea
            {
                get{
                    float elementHeight = parent.settings.qualifierHeight + parent.settings.actionHeight;
                    float ypos = parent.selectorNode.viewArea.y - (elementHeight * index) - elementHeight ;
                    float xpos = parent.selectorNode.viewArea.x - offset.x;
                    var rect = parent.GetAnchorAreaLocal(xpos, ypos);

                    //if(rect.Contains(Event.current.mousePosition - offset) == false){
                    //    Debug.LogFormat("Position: {0} {5}| Sides: xMin({1}), xMax({2}), yMin({3}), yMax({4})",
                    //                    Event.current.mousePosition - offset, rect.xMin, rect.xMax, 
                    //                    rect.yMin, rect.yMax, rect.Contains(Event.current.mousePosition - offset)); 
                    //}

                    return rect.Contains(Event.current.mousePosition - offset);
                }
            }

            public bool InActionArea
            {
                get;
            }

            public bool InDragArea
            {
                get;
            }




            public QualifierHit(NodeLayout nodeLayout)
            {
                parent = nodeLayout;
            }
        }

    }






}
