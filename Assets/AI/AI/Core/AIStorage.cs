namespace UtilAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;


    public class AIStorage : ScriptableObject
    {
        public static string StoragePath = "Assets/UtilityAI/Resources/AIStorage";

        [TextArea(1, 20)]
        public string description;
        public string version;
        public string aiId;       //  The name of the ai.  aiId has a Guid associated with it made through the NameMapGenerator.

        public UtilityAI configuration;



        public string friendlyName{
            get{
                var assetDir = AssetDatabase.GetAssetPath(this);
                return Path.GetFileNameWithoutExtension(assetDir);
            }
        }




        public static AIStorage CreateAsset(string aiId, string aiName)
        {
            AIStorage asset = CreateInstance<AIStorage>();
            string assetDir = AssetDatabase.GenerateUniqueAssetPath(StoragePath + "/" + aiName + ".asset");

            asset.aiId = aiId;
            asset.version = Application.version;


            AssetDatabase.CreateAsset(asset, assetDir);
            AssetDatabase.SaveAssets();
            return asset;
        }

    }
}
