namespace AtlasAI
{
    using UnityEngine;
    using System;

    /// <summary>
    /// AtlasAI creates the rootSelector and initializes the tree.
    /// </summary>
    public interface IUtilityAI : ISelect
    {
        Guid id{
            get;
        }

        string name{
            get;
            set;
        }

        Selector rootSelector{
            get;
            set;
        }

        int selectorCount { 
            get; 
        }


        //
        // Indexer
        //
        Selector this[int idx]
        {
            get;
        }


        void AddSelector(Selector s);

        Selector FindSelector(Guid id);

        void RemoveSelector(Selector s);

        bool ReplaceSelector(Selector current, Selector replacement);
        
        void RegenerateIds();
        //byte[] PrepareForSerialize();
        //void InitializeAfterDeserialize(byte[] data);

    }
}