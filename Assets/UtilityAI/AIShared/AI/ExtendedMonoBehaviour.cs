using System;
using UnityEngine;

namespace AtlasAI
{
    /// <summary>
    /// OnEnable() starts before Start()
    /// </summary>
    public abstract class ExtendedMonoBehaviour : MonoBehaviour
    {
        //
        // Fields
        //
        private bool _hasStarted;


        //
        // Methods
        //
        protected virtual void OnEnable()
        {
            OnStartAndEnable();
            _hasStarted = true;
        }


        protected virtual void Start()
        {
            if (_hasStarted){
                return;
            } else {
                OnStartAndEnable();
            }
        }


        protected virtual void OnStartAndEnable()
        {

        }
    }
}
