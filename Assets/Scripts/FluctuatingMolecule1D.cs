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

public class FluctuatingMolecule1D : FunctionBase
{
    [SerializeField] private List<FunctionBase> _sources = new List<FunctionBase>();
    [SerializeField, Range(0, 1f)] private float _positionOffsetFactor = 0;

    private float? _distance;
    private float _value;

    private void OnEnable()
    {
        _value = 0;
    }

    private void Update()
    {
        _value = GetValue(Time.time);
    }

    public override float GetValue(float t)
    {
        return _sources.Select(s =>
        {
            /*
            var timeStart = delay * (s.TimeStart + 1 / s.Fluctuation.Frequency / 4f);*/
            var value = s.GetValue(t);
            value /= Air.Rigid;
            return value;
        }).Sum();
    }

    public float PositionOffsetFactor
    {
        set
        {
            _positionOffsetFactor = value;
        }
    }

    public List<FunctionBase> Sources => _sources;

    public override float Value => _value;


}