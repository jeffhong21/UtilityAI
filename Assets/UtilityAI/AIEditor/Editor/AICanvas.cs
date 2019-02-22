using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AtlasAI.AIEditor
{
    
    public class AICanvas
    {
        //
        // Fields
        //
        public List<TopLevelNode> nodes;
        public Vector2 offset;
        public float zoom = 1f;

        //
        // Properties
        //
        public IEnumerable<SelectorNode> selectorNodes{
            get{
                foreach (var node in nodes){
                    if(node is SelectorNode)
                        yield return node as SelectorNode;
                }
            }
        }


        //
        // Constructors
        //
        public AICanvas()
        {
            nodes = new List<TopLevelNode>();

        }


        //
        // Methods
        //

        public int SnapAllToGrid()
        {
            throw new NotImplementedException();
        }


        public Vector2 SnapPositionToGrid(Vector2 value)
        {
            throw new NotImplementedException();
        }


        public Rect SnapToGrid(Rect value)
        {
            throw new NotImplementedException();
        }


        public TopLevelNode NodeAtPosition(Vector2 position)
        {
            for (int i = nodes.Count - 1; i >= 0; i--){
                if(nodes[i].viewArea.Contains(position)){
                    return nodes[i];
                }
            }
            return null;
        }




        public void DrawGrid(Rect rect, float zoom, Vector2 panOffset)
        {
            rect.position = Vector2.zero;

            Vector2 center = rect.size / 2f;
            Texture2D gridTex = EditorStyling.GraphBackground; ;
            //Texture2D crossTex = graphEditor.GetSecondaryGridTexture();

            // Offset from origin in tile units
            float xOffset = -(center.x * zoom + panOffset.x) / gridTex.width;
            float yOffset = ((center.y - rect.size.y) * zoom + panOffset.y) / gridTex.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTex.width;
            float tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTex.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));
            //GUI.DrawTextureWithTexCoords(rect, crossTex, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }



        private void DrawGrid(Rect viewPort)
        {
            //Vector2 size = _window.Size.size;
            var center = viewPort.size / 2f;


            // Offset from origin in tile units
            float xOffset = -(center.x * zoom + offset.x) / EditorStyling.GraphBackground.width;
            float yOffset = ((center.y - viewPort.size.y) * zoom + offset.y) / EditorStyling.GraphBackground.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(viewPort.size.x * zoom) / EditorStyling.GraphBackground.width;
            float tileAmountY = Mathf.Round(viewPort.size.y * zoom) / EditorStyling.GraphBackground.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(viewPort, EditorStyling.GraphBackground, new Rect(tileOffset, tileAmount));
        }



        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            var handleColor = Handles.color;
            Handles.color = color;

            Handles.DrawLine(start, end);
            Handles.color = handleColor;
        }
    }
}
