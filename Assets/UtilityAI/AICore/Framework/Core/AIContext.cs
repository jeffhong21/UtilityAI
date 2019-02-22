//namespace AtlasAI
//{
//    using UnityEngine;
//    using UnityEngine.AI;
//    using System;
//    using System.Linq;
//    using System.Collections.Generic;
    


//    /// <summary>
//    /// Represents knowledge that the AI uses to do what it needs to do.
//    /// </summary>
//    [Serializable]
//    public class AIContext : IAIContext
//    {

//        [SerializeField]
//        private EntityAIController _entity;
//        public EntityAIController entity { get { return _entity; } private set { _entity = value; } }

//        [SerializeField]
//        private Vector3 _destination;
//        public Vector3 destination { get { return _destination; } set { _destination = value; } }

//        [SerializeField]
//        private Vector3 _locationOfInterest;
//        public Vector3 locationOfInterest { get { return _locationOfInterest; } set { _locationOfInterest = value; } }

//        [SerializeField]
//        private Transform _focusTarget;
//        public Transform focusTarget { get { return _focusTarget; } set { _focusTarget = value; } }

//        [SerializeField]
//        private List<Transform> _waypoints = new List<Transform>();
//        public List<Transform> waypoints { get { return _waypoints; } set { _waypoints = value; } }

//        [SerializeField]
//        private List<Transform> _hostileEntities = new List<Transform>();
//        public List<Transform> hostileEntities { get { return _hostileEntities; } set { _hostileEntities = value; } }

//        //[SerializeField]
//        private List<GameObject> _friendlyEntities = new List<GameObject>();
//        public List<GameObject> friendlyEntities { get { return _friendlyEntities; } set { _friendlyEntities = value; } }

//        [SerializeField]
//        private List<Vector3> _sampledPositions = new List<Vector3>();
//        public List<Vector3> sampledPositions { get { return _sampledPositions; } set { _sampledPositions = value; } }




//        //public NavMeshAgent navMeshAgent;





//        public AIContext(EntityAIController aIEntity)
//        {
//            //Debug.Log("Initializing Context");
//            entity = aIEntity;

//        }






//    }
//}








