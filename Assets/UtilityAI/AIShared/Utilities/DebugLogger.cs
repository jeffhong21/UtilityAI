using System.Collections.Generic;
using UnityEngine;

namespace AtlasAI.Utilities
{
    public static class DebugLogger
    {
        //
        // Static Methods
        //


        public static void LogFormat(string template, params object[] args)
        {
            var message = string.Format(template, args);
            Log(message);
        }

        public static void Log(object message)
        {
            Debug.Log(message);
        }
    }
}
