using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fluctuation
{
    public float Frequency;
    public float Amplitude;
    public float Attack;
    public float Length;
}

public interface IFluctuatingObject1D
{
    float GetX();
}

public class FluctuatingObject1D : MonoBehaviour, IFluctuatingObject1D
{
    [SerializeField] private FluctuatingString1D _source;
    [SerializeField] private bool _shouldLog;

    private Vector2? _stablePosition;

    public FluctuatingString1D Source
    {
        set
        {
            _source = value;
        }
    }

    private void OnDisable()
    {
        _stablePosition = null;
    }

    public float GetX()
    {
        var timeStart = _source == null ? 0 : _source.TimeStart;
        _stablePosition = _stablePosition ?? transform.position;

        Log(timeStart);

        if (timeStart == 0)
        {
            return 0;
        }

        timeStart += Mathf.Abs(_stablePosition.Value.x) * 2f / _source.Fluctuation.Frequency / Air.Density;

        var deltaTime = Air.Time - timeStart;
        var x = _source.Fluctuation.GetValue(deltaTime);
        return x;
    }

    private void Log( object l )
    {
        if (_shouldLog)
        {
            Debug.Log(l);
        }
    }

}