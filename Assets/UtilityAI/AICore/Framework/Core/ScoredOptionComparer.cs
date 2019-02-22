namespace AtlasAI
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public struct ScoredOptionComparer<T> : IComparer<ScoredOption<T>>
    {
        public bool descending;

        public ScoredOptionComparer(bool descending = false)
        {
            this.descending = descending;
        }

        public int Compare(ScoredOption<T> x, ScoredOption<T> y)
        {
            if (x.score > y.score)
            {
                return descending ? 1 : -1;
            }
            if (x.score < y.score)
            {
                return descending ? -1 : 1;
            }
            else
            {
                return 0;
            }
        }

    }

}