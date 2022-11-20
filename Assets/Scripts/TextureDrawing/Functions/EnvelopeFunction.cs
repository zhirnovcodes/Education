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
            return GetValue(_envelope, Time.time - _timeStart, _timeEnd - _timeStart);
        }
    }

    public static float GetValue(Envelope envelope, float time, float timeEnd)
    {
        if (time < 0)
        {
            return 0;
        }

        if (time >= timeEnd)
        {
            if (envelope.Release <= 0)
            {
                return 0;
            }

            var lastValue = GetValue(envelope, time, Mathf.Infinity);
            return (1 - Mathf.InverseLerp(timeEnd, timeEnd + envelope.Release, time)) * lastValue;
        }

        if (time < envelope.Attack)
        {
            return Mathf.InverseLerp(0, envelope.Attack, time);
        }


        var value = Mathf.InverseLerp(envelope.Attack, envelope.Attack + envelope.Decay, time);
        value = Mathf.Lerp(1, envelope.Sustain, value);

        return value;

    }

    private void OnEnable()
    {
        _timeStart = Time.time;
        _timeEnd = Mathf.Infinity;
        _lastValue = 0;
    }

    private void OnDisable()
    {
        _timeEnd = Time.time;
    }

    public void Reset()
    {
        _timeStart = Mathf.Infinity;
        _timeEnd = Mathf.Infinity;
        _lastValue = 0;
    }
}
