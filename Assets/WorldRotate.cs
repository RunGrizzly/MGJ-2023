using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WorldRotate : MonoBehaviour
{
    [SerializeField] private float m_rotateSpeed = 1f;
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, m_rotateSpeed / 100f);
    }
}
