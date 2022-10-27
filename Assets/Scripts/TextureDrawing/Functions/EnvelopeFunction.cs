using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Envelope
{
    public float Attack;
    public float Decay;
    [Range(0,1)] public float Sustain;
    public float Release;
}

public class EnvelopeFunction : GraphFunctionBase, IResetable
{
    [SerializeField] private Envelope _envelope;

    public Envelope Envelope { get => _envelope; set { _envelope = value; } }

    private float _timeStart;
    private float _timeEnd;
    private float _lastValue;

    public override float Value
    {
        get
        {
            if (_timeStart < 0)
            {
                if (_timeEnd < 0)
                {
                    _lastValue = 0;
                    return 0;
                }
            }

            if (_timeEnd >= 0)
            {
                if (_envelope.Release <= 0)
                {
                    return 0;
                }

                return (1 - Mathf.InverseLerp(_timeEnd, _timeEnd + _envelope.Release, Time.time)) * _lastValue;
            }

            var t = Time.time - _timeStart;

            if (t < _envelope.Attack)
            {
                _lastValue = Mathf.InverseLerp(0, _envelope.Attack, t);
                return _lastValue;
            }


            _lastValue = Mathf.InverseLerp(_envelope.Attack, _envelope.Attack + _envelope.Decay, t);
            _lastValue = Mathf.Lerp(1, _envelope.Sustain, _lastValue);

            return _lastValue;
        }
    }

    private void OnEnable()
    {
        _timeStart = Time.time;
        _timeEnd = -1;
        _lastValue = 0;
    }

    private void OnDisable()
    {
        _timeStart = -1;
        _timeEnd = Time.time;
    }

    public void Reset()
    {
        _timeStart = -1;
        _timeEnd = -1;
        _lastValue = 0;
    }
}
