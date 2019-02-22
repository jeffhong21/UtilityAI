namespace AtlasAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;




    [Serializable]
    public class AIStorage : ScriptableObject
    {
        //
        // Fields
        //
        [TextArea(1, 20)]
        public string description;
        [ReadOnly]
        public string version;
        [ReadOnly]
        public string aiId;                 //  The name of the ai.  aiId has a Guid associated with it made through the NameMapGenerator.
        [HideInInspector]
        public IUtilityAI configuration;
        [ReadOnly]
        public string editorConfiguration;

        //
        // Properties
        //
        public string friendlyName{
            get{
                var assetDir = AssetDatabase.GetAssetPath(this);
                return Path.GetFileNameWithoutExtension(assetDir);
            }
        }





        ///<summary>
        ///Creates an instance for the specified AI.
        ///</summary>
        ///<param name = "aiId" > The name of the ai that gets registered.</param>
        ///<param name = "aiName" > Name of the asset file.</param>
        ///<returns></returns>
        public static AIStorage CreateAsset(string aiId, string aiName, bool isSelect = false)
        {
            AIStorage asset = CreateInstance<AIStorage>();
            string assetDir = AssetDatabase.GenerateUniqueAssetPath(AIManager.TempStorageFolder + "/" + aiName + ".asset");

            asset.aiId = aiId;
            asset.version = Application.version;

            AssetDatabase.CreateAsset(asset, assetDir);
            AssetDatabase.SaveAssets();

            if (isSelect) Selection.activeObject = asset;
            return asset;
        }


	}

}

