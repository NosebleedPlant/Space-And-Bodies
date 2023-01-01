using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private float sensitivity = 30;
    [SerializeField] private float returnTime = 10;
    private Transform _camera;
    private Quaternion _origialRotation;
    private Quaternion _endRotaion;
    private float _timer = 0;

    private void Start()
    {
        _camera = GetComponent<Camera>().transform;
        _origialRotation = _endRotaion = _camera.rotation;
        _timer = returnTime;
    }
    
    private void Update()
    {

        if (Input.GetMouseButton(1))
        {
            Rotate();
            _timer = 0;
        }
        else if(_timer<=returnTime)
        {
            _timer += Time.deltaTime;
            _camera.rotation = Quaternion.Lerp(
                _endRotaion,
                _origialRotation,
                Mathf.SmoothStep(0,1,_timer/returnTime));
        }
    }
    
    private void Rotate()
    {
        float magnitude = Time.deltaTime * sensitivity;

        float angleX = magnitude * Input.GetAxisRaw("Mouse X");
        float angleY = magnitude * Input.GetAxisRaw("Mouse Y");
        
        Quaternion yaw = Quaternion.Euler(0f, angleX, 0f);
        Quaternion pitch = Quaternion.Euler(-angleY, 0f, 0f);
        
        Quaternion rotation = yaw * _camera.rotation;
        _endRotaion = rotation * pitch;
        _camera.rotation = _endRotaion;
    }
}
