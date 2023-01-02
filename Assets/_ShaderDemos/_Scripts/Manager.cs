using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private string nextScene;

    [Header("Control Parameters")]
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private float returnTime = 5;


    private Transform _camera;
    private Quaternion _originalRotation;
    private Quaternion _endRotation;
    private float _timer = 0;
    
    private void Start()
    {
        _camera = Camera.main.transform;
        _originalRotation = _endRotation = _camera.rotation;
        _timer = returnTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadYourAsyncScene(nextScene));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadYourAsyncScene("_Home")); 
        }
        else if (Input.GetMouseButton(0))
        {
            Rotate();
            _timer = 0;
        }
        else if(_timer<=returnTime)
        {
            _timer += Time.deltaTime;
            _camera.rotation = Quaternion.Lerp(
                _endRotation,
                _originalRotation,
                Mathf.SmoothStep(0,1,_timer/returnTime));
        }
    }
    
    //async load the next scene
    private IEnumerator LoadYourAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //rotate the camera based on mouse input
    private void Rotate()
    {
        float magnitude = Time.deltaTime * sensitivity;

        float angleX = magnitude * Input.GetAxisRaw("Mouse X");
        float angleY = magnitude * Input.GetAxisRaw("Mouse Y");
        
        Quaternion yaw = Quaternion.Euler(0f, angleX, 0f);
        Quaternion pitch = Quaternion.Euler(-angleY, 0f, 0f);
        
        Quaternion rotation = yaw * _camera.rotation;
        _endRotation = rotation * pitch;
        _camera.rotation = _endRotation;
    }
}
