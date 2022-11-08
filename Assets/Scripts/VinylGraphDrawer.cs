using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VinylDrawer))]
public class VinylGraphDrawer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _timeOfFilling = 8;
    
    private VinylDrawer _drawer;
    private float _timeStart;
    private float _valueBefore;

    private void OnEnable()
    {
        _drawer = GetComponent<VinylDrawer>();
        _timeStart = Time.time;
        _valueBefore = _function.Value;
        _drawer.Clear();
    }

    private float TimeToUV(float time)
    {
        return Mathf.InverseLerp(_timeStart, _timeStart + _timeOfFilling, time);
    }

    void Update()
    {
        var val = _function.Value;
        var t = Time.time;

        if (t >= _timeStart + _timeOfFilling)
        {
            return;
        }

        float t1 = TimeToUV(t - Time.deltaTime);
        float t2 = TimeToUV(t);

        _drawer.Point1 = new Vector2(t1, _valueBefore / 2f + 0.5f);
        _drawer.Point2 = new Vector2(t2, val / 2f + 0.5f);

        _drawer.Paint();

        _valueBefore = val;
    }
}
