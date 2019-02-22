using System;
using System.Collections.Generic;


namespace AtlasAI.AIEditor
{

    public static class StoredAIs
    {
        //
        // Static Fields
        //
        private static List<AIStorage> _ais = new List<AIStorage>();


        //
        // Static Properties
        //
        public static List<AIStorage> AIs{
            get { return _ais; }
        }


        //
        // Static Methods
        //
        public static string EnsureValidName(string name, AIStorage target)
        {
            if(NameExists(name)){
                int index = 0;
                string suffix = " ";
                do
                {
                    suffix += index.ToString();
                    name += suffix;
                    index++;
                    if (index > 10) break;
                }
                while (NameExists(name) == false);
            }

            target.aiId = name;
            return name;
        }


        public static AIStorage GetById(string aiId)
        {
            for (int i = 0; i < AIs.Count; i++){
                if(AIs[i].aiId == aiId){
                    return AIs[i];
                }
            }
            return null;
        }


        public static bool NameExists(string name)
        {
            for (int i = 0; i < AIs.Count; i++)
                return AIs[i].aiId == name;
            return false;
        }

        public static void Refresh()
        {
            AIStorage[] storedAIs = UnityEngine.Resources.LoadAll<AIStorage>("AIStorage");
            AIs.Clear();
            AIs.AddRange(storedAIs);
        }

        //
        // Nested Types
        //
        private class AIStorageComparer : IComparer<AIStorage>
        {
            public int Compare(AIStorage x, AIStorage y){
                return string.Compare(x.aiId, y.aiId, StringComparison.Ordinal);
            }

        }




    }
}
