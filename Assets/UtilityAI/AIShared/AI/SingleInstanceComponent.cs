namespace AtlasAI
{
    using System;
    using UnityEngine;

    public abstract class SingleInstanceComponent<T> : MonoBehaviour where T : MonoBehaviour
    {
        //
        // Static Fields
        //
        private static int _instanceMark;


        //
        // Methods
        //
        private void Awake(){
            _instanceMark++;
            //  If there are more than one instances, than destroy this.
            if (_instanceMark > 1){
                Destroy(this);
            }


            OnAwake();
        }

        protected virtual void OnAwake(){
            
        }

        protected virtual void OnDestroy()
        {
            //  When the scene ends or the component is deleted, reset the instance mark.
            if(_instanceMark > 1){
                _instanceMark--;
            }

        }
    }
}