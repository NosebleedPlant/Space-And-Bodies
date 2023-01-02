using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotation_delta;

    void Update()
    {
        ArbitraryRotation();
    }

    private void ArbitraryRotation()
    {
        transform.Rotate(Vector3.right, rotation_delta.x);
        transform.Rotate(Vector3.up, rotation_delta.y);
        transform.Rotate(Vector3.forward, rotation_delta.z);
    }
}
