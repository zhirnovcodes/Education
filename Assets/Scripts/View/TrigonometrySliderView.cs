using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigonometrySliderView : TrigonometryViewBase
{
    [SerializeField] private SliderTextEditView _amp;
    [SerializeField] private SliderTextEditView _freq;
    [SerializeField] private SliderTextEditView _period;

    public override Trigonometry Function
    {
        get
        {
            return new Trigonometry { Amplitude = _amp.Value, Period = 1 / Mathf.Max(0.0001f, _freq.Value) };
        }
        set
        {
            _amp.Value = value.Amplitude;
            _freq.Value = 1 / Mathf.Max(0.0001f, value.Period);
        }
    }

    public override event Action ValueChanged;

    private void OnEnable()
    {
        _amp.ValueChanged += OnAmpChanged;
        _freq.ValueChanged += OnFreqChanged;
    }

    private void OnAmpChanged(float amp)
    {
        ValueChanged?.Invoke();
    }

    private void OnFreqChanged(float freq)
    {
        var per = 1 / Mathf.Max(0.0001f, freq);
        _period.Value = per;
        ValueChanged?.Invoke();
    }
}
