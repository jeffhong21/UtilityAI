using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AtlasAI.Visualization;

namespace AtlasAI.AIEditor
{

    public class AIUI : ScriptableObject
    {
        public AICanvas canvas;
        public Node selectedNode;
        public bool isDirty;


        private SelectorNode _currentSelector;
        private QualifierNode _currentQualifier;
        private AIStorage _aiStorage;


        //private string _tempEditorConfiguration;
        //private bool _useTempConfiguration;
        //
        // Properties
        //

        public SelectorNode currentSelector{
            get { return _currentSelector; }
        }

        public QualifierNode currentQualifier
        {
            get { return _currentQualifier; }
        }



        //
        // Static Methods
        //
        public static AIUI Create(string name)
        {
            //AIUI aiui= new AIUI();
            AIUI aiui = CreateInstance<AIUI>();
            aiui.InitNew(name);
            aiui.InitAI();
            return aiui;
        }



        public static AIUI Load(string aiId, bool refreshState)
        {
            //AIUI aiui = Create(aiId);
            AIUI aiui = CreateInstance<AIUI>();
            aiui.InitAI();



            aiui._aiStorage = StoredAIs.GetById(aiId);
            if (aiui._aiStorage != null)
                aiui.LoadFrom(aiui._aiStorage, refreshState);
            else
                Debug.LogFormat("Could not load ai.  aiId was {0} |", aiId);


            //Debug.LogFormat("Loading AIUI.  UtilityAI is {0} |", aiId);
            return aiui;
        }



        //
        // Methods
        //
        public void InitAI()
        {
            _currentSelector = null;
            selectedNode = null;
        }

        private void InitNew(string aiName)
        {
            canvas = new AICanvas();
            //name = aiName;
            if(_aiStorage != null)
                _aiStorage.editorConfiguration = GuiSerializer.Serialize(canvas);

            //_tempEditorConfiguration = GuiSerializer.Serialize(canvas);
            //_useTempConfiguration = true;
        }



        private bool LoadFrom(AIStorage data, bool refreshState)
        {
            canvas = GuiSerializer.Deserialize(this, data.editorConfiguration);
            Debug.Log(canvas);
            return true;
        }



        public void ShowVisualizedAI(UtilityAIVisualizer ai)
        {
            throw new NotImplementedException();
        }


        public bool Connect(QualifierNode qn, TopLevelNode targetNode)
        {
            throw new NotImplementedException();
        }


        public void Save(string newName)
        {
            _aiStorage.editorConfiguration = GuiSerializer.Serialize(canvas);
            Debug.Log(_aiStorage.editorConfiguration);
        }


        public void MarkDirty(){
            if (AIEditorWindow.activeInstance != null)
                AIEditorWindow.activeInstance.Repaint();
            if(_aiStorage != null){
                _aiStorage.editorConfiguration = GuiSerializer.Serialize(canvas);
                EditorUtility.SetDirty(_aiStorage);
            }

        }




        #region Add

        public SelectorNode AddSelector(Vector2 position, Type selectorType, int width = 256, int height = 144)
        {
            Rect rect = new Rect(position.x, position.y, width, height);
            SelectorNode node = SelectorNode.Create(selectorType, this, rect);
            //  Add node to the canvas.
            canvas.nodes.Add(node);
            node.name +=  " " + canvas.nodes.Count;

            MarkDirty();
            return node;
        }

        public QualifierNode AddQualifier(Type qualiferType, SelectorNode parent)
        {
            QualifierNode node = QualifierNode.Create(qualiferType);
            node.parent = parent;
            parent.qualifierNodes.Add(node);
            Debug.Log("Adding " + qualiferType.Name);

            MarkDirty();
            return node;
        }

        public QualifierNode AddQualifier(QualifierNode qn, SelectorNode parent){
            qn.parent = parent;
            parent.qualifierNodes.Add(qn);

            MarkDirty();
            return qn;
        }

        public QualifierNode AddQualifier(Type qualiferType)
        {
            QualifierNode node = QualifierNode.Create(qualiferType);
            MarkDirty();
            return node;
        }

        public ActionNode SetAction(ActionNode an, QualifierNode parent)
        {
            throw new NotImplementedException();
        }

        public ActionNode SetAction(Type actionType, QualifierNode parent)
        {
            throw new NotImplementedException();
        }

        public ActionNode SetAction(Type actionType)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Replace and Remove

        public void ReplaceAction(ActionNode an, IAction replacement)
        {
            throw new NotImplementedException();
        }

        public void ReplaceDefaultQualifier(QualifierNode newDefault, SelectorNode parent)
        {
            throw new NotImplementedException();
        }

        public void ReplaceQualifier(QualifierNode qn, IQualifier replacement)
        {
            throw new NotImplementedException();
        }

        public void ReplaceSelector(SelectorNode sn, Selector replacement)
        {
            throw new NotImplementedException();
        }

        public void RemoveAction(ActionNode an)
        {
            //currentQualifier.actionNode.action = null;
        }

        public void RemoveQualifier(QualifierNode qn)
        {
            currentSelector.qualifierNodes.Remove(qn);
            MarkDirty();
        }



        public bool RemoveSelected()
        {
            throw new NotImplementedException();
        }

        public bool RemoveSelector(SelectorNode sn){
            throw new NotImplementedException();
        }

        public bool RemoveNode(TopLevelNode node)
        {
            bool removed = canvas.nodes.Remove(node);
            if (removed) MarkDirty();
            return removed;
        }

        #endregion


        public void SetRoot(Selector newRoot)
        {
            throw new NotImplementedException();
        }


        public bool Delete()
        {
            throw new NotImplementedException();
        }




        public void RefreshState()
        {
            //_currentSelector = null;
            //selectedNode = null;

            for (int i = 0; i < canvas.nodes.Count; i++){
                canvas.nodes[i].parent = this;
                canvas.nodes[i].parentUI = this;
            }
        }


        public void Select(SelectorNode sn, QualifierNode qn, ActionNode an)
        {
            _currentSelector = sn;
            _currentQualifier = qn;
        }


    }
}
