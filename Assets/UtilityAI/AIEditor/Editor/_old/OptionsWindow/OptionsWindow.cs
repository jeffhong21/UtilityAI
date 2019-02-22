namespace AtlasAI.AIEditor
{
    using System;
    using UnityEditor;


    public abstract class OptionsWindow<T> : EditorWindow
    {
        //protected T window;
        protected int windowMinSize = 250;
        protected int windowMaxSize = 350;


        //  Need to know so we can add UtilityAIAssets
        protected UtilityAIComponent taskNetwork { get; set; }


        public virtual void Init(T window, UtilityAIComponent taskNetwork)
        {
            
        }
        public virtual void Init(T window, UtilityAIComponent taskNetwork, Type type)
        {
            Init(window, taskNetwork);
        }


        protected abstract void CloseWindow();
        protected abstract void DrawWindowContents();


        protected virtual void OnGUI(){
            DrawWindowContents();
        }

    
    }


}