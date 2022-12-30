using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGR_ForceClearRT : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTex;
    private void Start()
    {
        renderTex.Release();
    }
}
