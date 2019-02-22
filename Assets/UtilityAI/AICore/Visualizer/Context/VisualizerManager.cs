namespace AtlasAI.Visualization
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;



    /// <summary>
    /// Manager for the visualizers to register too.
    /// </summary>
    public static class VisualizerManager
    {
        //
        // Static Fields
        //
        private static bool _visualizing;
        //  The type to visualize and the visualizer associated with that type.
        private static Dictionary<Type, ICustomVisualizer> _visualizerLookup = new Dictionary<Type, ICustomVisualizer>();
        private static List<IContextProvider> _visualizedContextProviders = new List<IContextProvider>();



        //
        // Static Properties
        //
        public static bool isVisualizing {
            get { return _visualizing; }
		}

        public static IList<IContextProvider> visualizedContextProviders{
            get{ return _visualizedContextProviders; }
		}



        //
        // Static Methods
        //

        public static bool BeginVisualization()
        {
            _visualizing = !_visualizing;
            if(_visualizing){
                foreach(UtilityAIComponent taskNetwork in Utilities.ComponentHelper.FindAllComponentsInScene<UtilityAIComponent>()){
                    _visualizedContextProviders.Add(taskNetwork.GetComponent<IContextProvider>());
                }
            } else {
                _visualizedContextProviders.Clear();
            }
            return _visualizing;
        }


        //private static IEnumerable<Type> GetDerived (Type forType)
        //{
        //    foreach(Type derivedType in _visualizerLookup.Keys){
        //        if(derivedType == forType){
        //            yield return derivedType;
        //        }
        //    }
        //}



        public static void RegisterVisualizer<TFor>(ICustomVisualizer visualizer)
        {
            if (_visualizerLookup.ContainsKey(typeof(TFor)) == false){
                _visualizerLookup.Add(typeof(TFor), visualizer);
                //Debug.LogFormat("Registered {0} to a Visualizer", typeof(TFor));
            }

        }


        public static void UnregisterVisualizer<TFor>()
        {
            if (_visualizerLookup == null) return;
            if (_visualizerLookup.ContainsKey(typeof(TFor))){
                _visualizerLookup.Remove(typeof(TFor));
                //Debug.LogFormat("Unregistered {0} to a Visualizer", typeof(TFor));
            }
        }


        /// <summary>
        /// Check if there is a visualizer for the given type.
        /// </summary>
        /// <returns><c>true</c>, if get visualizer for was tryed, <c>false</c> otherwise.</returns>
        /// <param name="t">T.</param>
        /// <param name="visualizer">Visualizer.</param>
        public static bool TryGetVisualizerFor(Type t, out ICustomVisualizer visualizer)  //  internal
        {
            if (_visualizerLookup.ContainsKey(t)){
                visualizer = _visualizerLookup[t];
                return true;
            }
            visualizer = null;
            return false;
        }



        public static void UpdateSelectedGameObjects(GameObject[] selected)  //  internal
        {
            for (int index = 0; index < selected.Length; index++){
                //  Check if selected gameObject contains UtilityAIComponent.
                if(selected[index].GetComponent<UtilityAIComponent>() == true){
                    //  Get the UtilityAIComponent.
                    UtilityAIComponent go = selected[index].GetComponent<UtilityAIComponent>();
                    //  Get Context.
                    IAIContext context = go.GetComponent<IContextProvider>().GetContext();

                    ICustomVisualizer visualizer = null;
                    if (TryGetVisualizerFor(go.GetType(), out visualizer)){
                        //visualizer.EntityUpdate(go, context);
                        //visualizer.EntityUpdate(go, context, aiId);
                    }
                }
                

            }
        }







        public static string DebugLogRegisteredVisualziers(bool indent = false)
        {
            string registeredVisualziersLog = "";
            string indentString = "|";

            if(indent){
                indentString = "\n    ";
            }

            if(_visualizerLookup != null)
            {
                foreach (KeyValuePair<Type, ICustomVisualizer> visualizer in _visualizerLookup)
                {
                    registeredVisualziersLog += string.Format("VisualizerType:  {0} {2} ICustomVisualizer:  {1}\n", visualizer.Key.Name, visualizer.Value.GetType(), indentString);
                }
            }


            return registeredVisualziersLog;
        }

    }
}




#region OLD
//using UnityEngine;
//public class VisualizerManager : MonoBehaviour
//{


//    Dictionary<Type, ICustomVisualizer> registeredVisualizers;


//    private static VisualizerManager visualizerManager;

//    Dictionary<string, UnityEvent> visualizerEvents;
//    public static VisualizerManager Instance
//    {
//        get
//        {
//            if (!visualizerManager)
//            {
//                visualizerManager = FindObjectOfType<VisualizerManager>() as VisualizerManager;

//                if (visualizerManager == false)
//                {
//                    Debug.LogError("There needs to be an active Visualizer Manager");
//                }
//                else
//                {
//                    if (visualizerManager.visualizerEvents == null)
//                    {
//                        visualizerManager.visualizerEvents = new Dictionary<string, UnityEvent>();
//                    }

//                }
//            }
//            return visualizerManager;
//        }
//    }


//    public void RegisterVisualizer<TFor>(string eventName, UnityAction listener)
//    {
//        UnityAction someAction;
//        UnityEvent thisEvent = null;
//        if (Instance.visualizerEvents.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.AddListener(listener);
//        }
//        else
//        {
//            thisEvent = new UnityEvent();
//            thisEvent.AddListener(listener);
//            Instance.visualizerEvents.Add(eventName, thisEvent);
//        }

//    }

//    public void RegisterVisualizer(Type forType, ICustomVisualizer visualizer, bool registerDerivedTypes)
//    {

//    }



//    public void Unregister<TFor>(string eventName, UnityAction listener)
//    {
//        if (visualizerManager == null) return;
//        UnityEvent thisEvent = null;
//        if (Instance.visualizerEvents.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.RemoveListener(listener);
//        }
//    }


//    public static void TriggerEvent(string eventName)
//    {
//        UnityEvent thisEvent = null;
//        if (!Instance.visualizerEvents.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.Invoke();
//        }
//    }

//}
#endregion