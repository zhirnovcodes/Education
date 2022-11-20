using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VinylDrawer))]
public class VinylGraphDrawer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _timeOfFilling = 8;
    [SerializeField, Range(0, 3)] private float _scale = 1;
    [SerializeField, Range(0, 0.5f)] private float _startEndUOffset = 0.1f;
    [SerializeField, Range(0, 1)] private float _startV = 0.5f;
    
    private VinylDrawer _drawer;
    private float _timeStart;
    private float _valueBefore;
    private Vector2 _uvCurrent;

    public Vector3 CurrentPointPosition
    {
        get
        {
            var localPos = new Vector3(_uvCurrent.x, 0, _uvCurrent.y);
            var globalPos = transform.localToWorldMatrix * localPos;
            return globalPos;
        }
    }
    
    private void OnEnable()
    {
        _drawer = GetComponent<VinylDrawer>();
        _timeStart = Time.time;
        _valueBefore = GetValueAsV();
        _drawer.Clear();
        _uvCurrent = Vector2.zero;
    }

    private float TimeToU(float time)
    {
        return Mathf.InverseLerp(_timeStart, _timeStart + _timeOfFilling, time) + _startEndUOffset;
    }

    private float GetValueAsV()
    {
        return _function.Value / 2f * _scale + _startV;
    }

    void Update()
    {
        var t = Time.time;
        var val = GetValueAsV();

        if (t >= _timeStart + _timeOfFilling)
        {
            return;
        }

        float t1 = TimeToU(t - Time.deltaTime);
        float t2 = TimeToU(t);

        if (t2 >= 1 - _startEndUOffset)
        {
            return;
        }

        _uvCurrent = new Vector2(t2, val);

        _drawer.Point1 = new Vector2(t1, _valueBefore);
        _drawer.Point2 = new Vector2(t2, val);

        _drawer.Paint();

        _valueBefore = val;
    }
}
