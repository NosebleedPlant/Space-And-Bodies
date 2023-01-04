using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBounce : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    void Start()
    {
        var to = transform.position + offset;
        LeanTween.move(this.gameObject,to,1).setLoopPingPong();
    }
}
