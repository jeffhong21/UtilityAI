namespace AtlasAI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LoadBalanceObject : MonoBehaviour
    {
        public bool useUpdate;
        [SerializeField]
        private float speed = 4f;
        [SerializeField]
        private float height = 1f;

        private Vector3 pos;
        private float currentY;


		public void Start()
		{
            //Debug.Log(UtilityAIManager.instance);
            //UtilityAIManager.instance.Register(this);
		}


		public void Update()
		{
            if(useUpdate) OnUpdate(Time.deltaTime);

		}


		public void OnUpdate(float delta)
		{
            pos = transform.position;

            currentY = Mathf.Sin(Time.time * speed);

            transform.position = new Vector3(pos.x, currentY, pos.z) * height;
		}

        Color selectedColor = Color.red;
        Color cachedColor;
        public void OnSelection(){
            //GetComponentInChildren<MeshRenderer>().material.color = selectedColor;
            SetRotation();
        }

        Vector3 tempRot;
        private void SetRotation()
        {
            tempRot.x = 0;
            tempRot.y = Time.deltaTime * 45;
            tempRot.z = 0;


            transform.Rotate(tempRot);
        }

	}
}

