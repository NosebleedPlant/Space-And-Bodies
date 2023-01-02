using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingObjects : MonoBehaviour
{
    [SerializeField] private bool animate = false;
    
    private Vector4[] _startPositions = new Vector4[RayMarcher.ObjectCount];
    private Vector4[] _endPositions = new Vector4[RayMarcher.ObjectCount];
        
    private float _reset = 10;
    private float _timer = 0;

    private void OnEnable()
    {
        RandomPositions();
    }

    private void Update()
    {
        if (animate)
            Move();
    }

    private void RandomPositions()
    {
        for (int i = 0; i < RayMarcher.ObjectPositions.Length; i++)
        {
            RayMarcher.ObjectPositions[i] = new Vector3
            (
                Random.Range(-3.0f, 3.0f),
                Random.Range(-3.0f, 3.0f),
                Random.Range(-3.0f, 3.0f)
            );
            _startPositions[i] = RayMarcher.ObjectPositions[i];
            _endPositions[i] = new Vector3
            (
                Random.Range(-3.0f, 3.0f),
                Random.Range(-3.0f, 3.0f),
                Random.Range(-3.0f, 3.0f)
            );
        }
    }

    private void Move()
    {
        _timer += Time.deltaTime;
        if (_timer >= _reset)
        {
            _timer = 0;
            for (int i = 0; i < _endPositions.Length; i++)
            {
                _startPositions[i] = _endPositions[i];
                _endPositions[i] = new Vector3
                (
                    Random.Range(-3.0f, 3.0f),
                    Random.Range(-3.0f, 3.0f),
                    Random.Range(-3.0f, 3.0f)
                );
            }
        }

        for (int i = 0; i < RayMarcher.ObjectPositions.Length; i++)
        {
            RayMarcher.ObjectPositions[i] = Vector3.Lerp
            (
                _startPositions[i],
                _endPositions[i],
                _timer / _reset
            );
        }
    }
}
