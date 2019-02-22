using System;
using System.Collections.Generic;
using UnityEngine;


namespace AtlasAI.AIEditor
{

    public class AIGeneralSettings : ScriptableObject
    {
        
        public const string NameMapFileName = "AINameMap.cs";
        private static AIGeneralSettings _instance;


        //
        // Fields
        //
        [HideInInspector, SerializeField]
        private string _storagePath = "Assets/Scripts/UtilityAI/Resources/AIStorage";
        [HideInInspector, SerializeField]
        private string _nameMapPath = "Assets/Scripts/UtilityAI";
        private bool _isDirty;

        //
        // Static Properties
        //
        public static AIGeneralSettings instance
        {
            get{
                if(_instance == null){
                    _instance = CreateInstance<AIGeneralSettings>();
                }
                return _instance;
            }
        }

        //
        // Properties
        //
        public bool isDirty
        {
            get;
        }

        public string nameMapPath
        {
            get { return _nameMapPath; }
            set { _nameMapPath = value; }
        }

        public string storagePath
        {
            get { return _storagePath; }
            set { _storagePath = value; }
        }



    }
}
