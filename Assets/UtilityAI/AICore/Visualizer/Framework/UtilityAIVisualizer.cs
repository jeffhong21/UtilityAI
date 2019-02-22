using System;
using System.Collections.Generic;

using AtlasAI.Utilities;


namespace AtlasAI.Visualization
{
    public class UtilityAIVisualizer : IUtilityAI, ISelect
    {
        //
        // Fields
        //
        private IUtilityAI _ai;
        private List<Action> _postExecute;
        private SelectorVisualizer _visualizerRootSelector;

        private List<SelectorVisualizer> _selectorVisualizers;

        //private List<UtilityAIVisualizer> _linkedAIs;


        //
        // Properties
        //
        public IUtilityAI ai{
            get { return _ai; }
        }

        public Guid id{
            get { return _ai.id; }
        }

        public string name{
            get { return _ai.name; }
            set { _ai.name = value; }
        }

        public Selector rootSelector{
            get { return _ai.rootSelector; }
            set { _ai.rootSelector = value; }
        }

        public int selectorCount{
            get { return _ai.selectorCount; }
        }


        //
        // Indexer
        //
        public Selector this[int idx]{
            get { return _selectorVisualizers[idx]; }
        }


        //
        // Constructors
        //
        public UtilityAIVisualizer(IUtilityAI ai)
        {
            _ai = ai;
            _visualizerRootSelector = new SelectorVisualizer(ai.rootSelector, this);
            name = ai.name;
            rootSelector = ai.rootSelector;

            DebugLogger.LogFormat("Initializing UtilityAI Visualizer.");
        }


        //
        // Static Methods
        //
        private static bool TryFindActionVisualizer(IAction source, IAction target, out ActionVisualizer result)
        {
            throw new NotImplementedException();
        }



        //
        // Methods
        //
        public IAction Select(IAIContext context)
        {
            //  Get action to perform.
            IAction action = ai.Select(context);

            return action;
        }

        public ActionVisualizer FindActionVisualizer(IAction target)
        {
            throw new NotImplementedException();
        }


        public IQualifierVisualizer FindQualifierVisualizer(IQualifier target)
        {
            throw new NotImplementedException();
        }


        public void Hook(Action postExecute)
        {
            throw new NotImplementedException();
        }


        public void PostExecute()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        public void Unhook(Action postExecute)
        {
            throw new NotImplementedException();
        }




        public Selector FindSelector(Guid id)
        {
            return ai.FindSelector(id);
        }

        void IUtilityAI.AddSelector(Selector s)
        {
            _selectorVisualizers.Add(new SelectorVisualizer(s, this));
            ai.AddSelector(s);
        }

        void IUtilityAI.RegenerateIds()
        {
            ai.RegenerateIds();
        }


        void IUtilityAI.RemoveSelector(Selector s)
        {
            for (int index = 0; index < _selectorVisualizers.Count; index++){
                //  If selector id matches given selector id, remove the selector.
                if (_selectorVisualizers[index].selector.id == s.id){
                    //  If the root selector is to be removed, replace the rootSelector field with the next selector.
                    if (index == 0){
                        rootSelector = _selectorVisualizers[1].selector;
                    }
                    _selectorVisualizers.RemoveAt(index);
                    return;
                }
            }
            ai.RemoveSelector(s);
        }


        bool IUtilityAI.ReplaceSelector(Selector current, Selector replacement)
        {
            return ai.ReplaceSelector(current, replacement);
        }





    }
}
