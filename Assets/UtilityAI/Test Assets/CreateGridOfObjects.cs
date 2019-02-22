using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGridOfObjects : MonoBehaviour 
{
    public int m_ObjectsInX;
    public int m_ObjectsInZ;
    public float m_Width;
    public float m_Height;
    public GameObject m_GameObject;

    Vector3 defaultPosition;
    Quaternion defaultRotation;
    float widthDesinity;
    float heightDesinity;
    float widthHalfRange;
    float heightHalfRange;

    List<GameObject> m_AllGameObjects;

    public void Execute()
    {
        if (m_GameObject == null) return;

        defaultPosition = transform.position;
        defaultRotation = Quaternion.identity;
        widthDesinity = m_Width / m_ObjectsInX;
        heightDesinity = m_Height / m_ObjectsInZ;
        widthHalfRange = m_Width * 0.5f;
        heightHalfRange = m_Height * 0.5f;


        for (var x = -widthHalfRange; x < widthHalfRange; x += widthDesinity){
            for (var z = -heightHalfRange; z < heightHalfRange; z += heightDesinity){
                var position = new Vector3(defaultPosition.x + x, 0f, defaultPosition.z + z);
                var rotation = defaultRotation;
                var go = Instantiate(m_GameObject, position, rotation, this.transform);
            }
        }
    }





}
