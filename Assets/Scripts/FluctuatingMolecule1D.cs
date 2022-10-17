using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Fluctuation
{
    public float Frequency;
    public float Amplitude;
    public float Attack;
    public float Time;
}

public abstract class FluctuatingObject1D : GraphFunctionBase
{
    public abstract float TimeStart { get; }
    public abstract Fluctuation Fluctuation { get; }

    public override float Value => TimeStart <= 0 ? 0 : Fluctuation.GetValue(Time.time - TimeStart);
}

public class FluctuatingMolecule1D : GraphFunctionBase
{
    [SerializeField] private List<FluctuatingObject1D> _sources = new List<FluctuatingObject1D>();
    [SerializeField] private bool _withDelay = true;
    [SerializeField] private bool _shouldLog;

    private float? _distance;
    private float _value;

    private void OnEnable()
    {
        _value = 0;
    }

    private void Update()
    {
        _value = _sources.Select(s =>
        {
            var delay = _withDelay ? 1f : 0f;
            var timeStart = delay * (s.TimeStart + 1 / s.Fluctuation.Frequency / 4f);
            var value = s.TimeStart <= 0 ? 0 : s.Fluctuation.GetValue(Time.time - timeStart);
            value /= Air.Rigid;
            return value;
        }).Sum();
    }

    public bool WithDelay
    {
        set
        {
            _withDelay = value;
        }
    }

    public List<FluctuatingObject1D> Sources => _sources;

    public override float Value => _value;

    private void Log( object l )
    {
        if (_shouldLog)
        {
            Debug.Log(l);
        }
    }

}