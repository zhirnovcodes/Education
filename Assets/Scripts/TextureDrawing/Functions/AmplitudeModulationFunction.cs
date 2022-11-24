using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModulator
{
    float ModulationValue { get; set; }
}

public class AmplitudeModulationFunction : GraphFunctionBase, IModulator
{
    [SerializeField] private TrigonometryFunction _source;
    [SerializeField] private GraphFunctionBase _modulator;
    [SerializeField, Range(0,1)] private float _modulatorValue = 1f;

    private float _startTime;

    private void OnEnable()
    {
        _startTime = Time.time;
    }

    public override float Value
    {
        get
        {
            var func = _source.Function;
            var modValue = _modulator.Value * _modulatorValue;
            var ampNew = func.Amplitude + modValue;
            func.Amplitude = ampNew;
            return func.GetValue(Time.time - _startTime);
        }
    }

    public float ModulationValue { get => _modulatorValue; set => _modulatorValue = value; }
}
