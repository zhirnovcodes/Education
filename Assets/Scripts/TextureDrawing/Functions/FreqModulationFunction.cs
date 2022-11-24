using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqModulationFunction : GraphFunctionBase, IModulator
{
    [SerializeField] private TrigonometryFunction _source;
    [SerializeField] private GraphFunctionBase _modulator;
    [SerializeField, Range(0, 1)] private float _modulatorValue = 1f;

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

            var t = Time.time - _startTime;
            t = Mathf.Lerp(t, t / (1 - Mathf.Clamp(modValue, 0, 0.9999f)), modValue);
            return func.GetValue(t);
        }
    }

    public float ModulationValue { get => _modulatorValue; set => _modulatorValue = value; }
}
