using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float lookSensitivity = 30;
    [SerializeField] private float moveSensitivity = 10;

    private Vector3 _originalPos;
    private Quaternion _originalRot;

    private void Start()
    {
        _originalPos = transform.position;
        _originalRot = transform.rotation;
    }

    private void Update()
    {
        Vector2 magnitude = new Vector2(Input.GetAxisRaw("Horizontal"), -Input.GetAxis("Vertical"));
        Move(magnitude);

        //handle rotation input
        if (Input.GetMouseButton(0))
            Rotate();
        
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            transform.position = _originalPos;
            transform.rotation = _originalRot;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(LoadYourAsyncScene());
    }
    
    private void Rotate()
    {
        float magnitude = Time.deltaTime * lookSensitivity;

        float angleX = magnitude * Input.GetAxisRaw("Mouse X");
        float angleY = magnitude * Input.GetAxisRaw("Mouse Y");
        
        Quaternion yaw = Quaternion.Euler(0f, angleX, 0f);
        Quaternion pitch = Quaternion.Euler(-angleY, 0f, 0f);
        
        Quaternion rotation = yaw * transform.rotation;

        //set calculated rotation
        transform.rotation = rotation * pitch;
    }
    private void Move(Vector2 magnitude)
    {
        magnitude *= moveSensitivity * Time.deltaTime;
        Vector3 displacement = transform.right * magnitude.x - transform.forward * magnitude.y;
        transform.position += displacement;
    }

    private IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("_Home");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
