using UnityEngine;
using System.Collections.Generic;


namespace UtilAI.AIEditor
{
    public class AICanvas
    {

        public List<TopLevelNode> nodes;

        public Vector2 offset;

        public float zoom = 1;



        public IEnumerable<SelectorNode> selectorNodes{
            get{
                foreach (var node in nodes){
                    if (node is SelectorNode)
                        yield return node as SelectorNode;
                }
            }
        }




        public AICanvas()
        {
            nodes = new List<TopLevelNode>();

        }




        public TopLevelNode NodeAtPosition(Vector2 position)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].viewArea.Contains(position))
                {
                    return nodes[i];
                }
            }
            return null;
        }
    }
}
