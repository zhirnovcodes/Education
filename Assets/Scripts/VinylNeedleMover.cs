using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylNeedleMover : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _timeOfFilling = 8;
    [SerializeField] private float _scale = 1;
    [SerializeField] private Vector3 _targetPosition = Vector3.forward;

    private float _timeStart;
    private Vector3 _stablePos;

    private void OnEnable()
    {
        _stablePos = transform.position;
        _timeStart = Time.time;
    }

    private void OnDisable()
    {
        transform.position = _stablePos;
    }

    private float TimeToT(float time)
    {
        return Mathf.InverseLerp(_timeStart, _timeStart + _timeOfFilling, time);
    }

    void Update()
    {
        var t = Time.time;

        if (t >= _timeStart + _timeOfFilling)
        {
            return;
        }
        var val = _function.Value;

        t = TimeToT(t);
        var posHor = Vector3.Lerp(_stablePos, _targetPosition, t);
        var per = Vector3.Cross(Vector3.down, _targetPosition - _stablePos).normalized;
        var posVer = per * val * _scale;

        transform.position = posHor + posVer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _targetPosition);
    }
}
