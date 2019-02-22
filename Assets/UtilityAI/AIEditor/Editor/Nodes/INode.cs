using System;

namespace AtlasAI.AIEditor
{

    public interface INode
    {
        //string description{
        //    get;
        //    set;
        //}

        string name{
            get;
            set;
        }

        AIUI parentUI{
            get;
        }
    }
}
