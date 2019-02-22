namespace AtlasAI.AIEditor
{
    using System;
    using UnityEngine;


    public sealed class MouseState
    {
        //
        // Fields
        //
        public bool isMouseUp;
        public bool isMouseDown;
        public bool isMouseDrag;
        public bool isLeftButton;
        public bool isRightButton;

        //
        // Constructors
        //
        public MouseState()
        {
            isMouseUp = false;
            isMouseDown = false;
            isMouseDrag = false;
            isLeftButton = false;
            isRightButton = false;
        }

        //
        // Methods
        //

        public void Update(Event evt)
        {
            isMouseUp = evt.type == EventType.MouseUp;
            isMouseDown = evt.type == EventType.MouseDown;
            isMouseDrag = evt.type == EventType.MouseDrag;
            isLeftButton = evt.button == 0 && evt.isMouse;
            isRightButton = evt.button == 1 && evt.isMouse;
        }







    }
}
