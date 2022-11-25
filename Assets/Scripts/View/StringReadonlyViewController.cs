using System;
using UnityEngine;

public class StringReadonlyViewController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _textFrequency;
    [SerializeField] private TMPro.TMP_Text _textAmplitude;
    [SerializeField] private TrigonometryFunction _function;
    [SerializeField] private bool _addCaption = true;
    [SerializeField] private float _freqMultiplier = 1;

    private void Update()
    {
        var captionF = _addCaption ? "Frequency = " : String.Empty;
        var measureF = _addCaption ? " Hz" : String.Empty;

        var captionA = _addCaption ? "Amplitude = " : String.Empty;
        var measureA = _addCaption ? " dB" : String.Empty;

        _textFrequency.text = captionF + Math.Round((1f / _function.Function.Period) * _freqMultiplier, 2).ToString() + measureF;
        _textAmplitude.text = captionA + Math.Round(_function.Function.Amplitude, 2).ToString() + measureA;
    }
}
