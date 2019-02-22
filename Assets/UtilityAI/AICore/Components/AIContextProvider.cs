//namespace AtlasAI
//{
//    using System;
//    using UnityEngine;


//    public class AIContextProvider : MonoBehaviour, IContextProvider
//    {


//        [SerializeField]
//        private AIContext _context;
//        public AIContext context { get {return _context;} set {_context = value;}}


//        private void OnEnable()
//        {
//            _context = new AIContext(GetComponent<EntityAIController>());
//        }


//        public IAIContext GetContext(){
//            return context as IAIContext;
//        }

//        public IAIContext GetContext(Guid aiId){
//            return GetContext();
//        }

//    }
//}
