//namespace AtlasAI
//{
//    using UnityEngine;
//    using UnityEditor;
//    using System;
//    using System.Linq;
//    using System.Collections.Generic;



//    public static class AINameMap
//    {

//        private static Dictionary<Guid, string> _aiNameMap;


//        public static Dictionary<Guid, string> aiNameMap
//        {
//            get {
//                return _aiNameMap;
//            }
//        }


//        static AINameMap()
//        {
//            if (_aiNameMap == null)
//                _aiNameMap = new Dictionary<Guid, string>();
//        }


//        public static Guid GetGUID(string aiId)
//        {
//            if (aiNameMap.ContainsValue(aiId))
//            {
//                Guid _guid = aiNameMap.FirstOrDefault(x => x.Value == aiId).Key;
//                return _guid;
//            }

//            Debug.LogFormat("<color=red> AINameMap does not contain aiId ( {0} ) </color>", aiId);
//            return default(Guid);
//        }


//        public static string GetAiId(Guid guid)
//        {
//            if (guid == Guid.Empty) return null;


//            if (aiNameMap.ContainsKey(guid))
//            {
//                return aiNameMap[guid];
//            }

//            return null;
//        }



//        public static void Register(string guid, string aiId)
//        {
//            Guid _guid;
//            if(Guid.TryParse(guid, out _guid))
//            {
//                if (aiNameMap.ContainsKey(_guid) == false)
//                {
//                    aiNameMap.Add(_guid, aiId);
//                }
//            }
//        }


//        public static bool UnRegister(string guid)
//        {
//            if (aiNameMap == null)
//                return false;


//            Guid _guid;
//            if (Guid.TryParse(guid, out _guid))
//            {
//                if (aiNameMap.ContainsKey(_guid))
//                {
//                    aiNameMap.Remove(_guid);
//                    return true;
//                }
//            }

//            return false;
//        }


//        public static string GenerateUniqueID(string aiId)
//        {     
//            string _aiId = aiId;
//            int index = 1;
//            string suffix = "";

//            do
//            {
//                if (aiNameMap.ContainsValue(_aiId))
//                {
//                    suffix = " " + index.ToString();
//                    _aiId = aiId + suffix;
//                }

//                index++;
//                if (index > 10)
//                    break;
//            }
//            while (aiNameMap.ContainsValue(_aiId) == true);

//            return _aiId;
//        }


//        public static void RegenerateNameMap()
//        {
//            if (aiNameMap.Count == 0)
//            {
//                string filterType = "t:AIStorage";
//                foreach (var guid in AssetDatabase.FindAssets(filterType))
//                {
//                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
//                    AIStorage aiAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as AIStorage;
//                    Register(guid, aiAsset.aiId);
//                }
//            }
//        }


//    }

//}

