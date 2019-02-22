using System.Collections.Generic;
using UnityEngine;

namespace AtlasAI.Utilities
{
    public static class ComponentHelper
    {
        //
        // Static Methods
        //

        /// <summary>
        /// Returns a list of all active loaded objects of Type type.
        /// </summary>
        /// <returns>The all components in scene.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> FindAllComponentsInScene<T>() where T : Component
        {
            return Object.FindObjectsOfType<T>();
        }

        /// <summary>
        /// Returns the first active loaded object of Type type.  If none are found, it will 
        /// create a new game object and attach the component to it.
        /// </summary>
        /// <returns>The first component in scene.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T FindFirstComponentInScene<T>() where T : Component
        {
            var component = Object.FindObjectOfType<T>();
            if(component == null){
                Debug.LogFormat("Could not locate {0}.  Creating new one.", typeof(T).Name);
                var go = new GameObject(typeof(T).Name, typeof(T));
                component = go.GetComponent<T>();
            }
            return component;
        }
    }
}
