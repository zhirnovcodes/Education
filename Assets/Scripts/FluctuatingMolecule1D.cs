using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Fluctuation
{
    public float Frequency;
    public float Amplitude;
    public float Attack;
    public float Length;
}

public abstract class FluctuatingObject1D : MonoBehaviour
{
    public abstract float TimeStart { get; }
    public abstract Fluctuation Fluctuation { get; }
}

public class FluctuatingMolecule1D : FluctuatingObject1D
{
    [SerializeField] private FluctuatingObject1D _source;
    [SerializeField] private bool _shouldLog;

    private float? _distance;

    public FluctuatingObject1D Source
    {
        set
        {
            _source = value;
            _distance = null;
        }
    }

    public override float TimeStart
    {
        get
        {
            if (_source == null)
            {
                return 0;
            }
            var timeStartSource = _source == null ? 0 : _source.TimeStart;
            var timeStart = timeStartSource == 0 ? 0 : (timeStartSource + 1 / _source.Fluctuation.Frequency / 4 );
            return timeStart;
        }
    }

    private float GetDistance()
    {
        if (_distance == null)
        {
            _distance = _source == null ? (float?)null : Mathf.Abs(_source.transform.position.x - transform.position.x);
        }
        return _distance ?? 0;
    }

    public override Fluctuation Fluctuation 
    {
        get
        {
            var f = _source.Fluctuation;
            f.Amplitude /= Air.Rigid;
            return f; 
        }
    }

    private void Log( object l )
    {
        if (_shouldLog)
        {
            Debug.Log(l);
        }
    }

}