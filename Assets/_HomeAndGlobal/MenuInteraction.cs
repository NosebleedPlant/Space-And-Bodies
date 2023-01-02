using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuInteraction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private const float NATURAL_LOG_OF_2 = 0.693147181f;

    [SerializeField] private string nextScene;

    [Header("Animation Parameters")]
    [SerializeField] private float time = 0.6f;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private Vector3 offset = new Vector3(60f, 0,0);
    [SerializeField] private float angle = 2;

    private Vector3 _startPos;
    private Vector3 _endPos;

    private Quaternion _startRot;
    
    private Vector3 _startScale;
    private Vector3 _endScale;

    private LTDescr _tweenIn;
    private LTDescr _tweenOut;

    private void Start()
    {
        _startPos = transform.localPosition;
        _endPos = _startPos + offset;

        _startRot = transform.localRotation;

        _startScale = transform.localScale;
        _endScale = _startScale * scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleIn();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleOut();
    }

    public void ScaleIn()
    {
        if(_tweenIn == null) 
        {
            transform.localPosition = _startPos;
            LeanTween.moveLocal(this.gameObject, _endPos, 1).setEaseLinear();

            transform.localRotation = _startRot;
            LeanTween.rotateAroundLocal(this.gameObject,Vector3.forward,angle, time).setEaseLinear();

            transform.localScale = _startScale;
            _tweenIn = LeanTween.scale(this.gameObject, _endScale, time).setEaseOutBounce().setOnComplete(
                () => { _tweenIn = null; });
        }
    }
        
    public void ScaleOut()
    {
        if (_tweenOut == null)
        {
            transform.localPosition = _endPos;
            LeanTween.moveLocal(this.gameObject, _startPos, time).setEaseLinear();

            LeanTween.rotateAroundLocal(this.gameObject, Vector3.forward, -angle, time).setEaseLinear();

            transform.localScale = _endScale;
            _tweenOut = LeanTween.scale(this.gameObject, _startScale, 1).setEaseOutQuint().setOnComplete(
                () => { _tweenOut = null; });
        }
    }

    private IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
