using System;
using UnityEngine;
using UnityEngine.UI;

public class EnvelopeView : MonoBehaviour
{
    [SerializeField] private Slider _attack;
    [SerializeField] private Slider _decay;
    [SerializeField] private Slider _sustain;
    [SerializeField] private Slider _release;

    public event Action ValueChanged;

    public Envelope Envelope
    {
        get => new Envelope() {Attack = _attack.value, Decay = _decay.value, Release = _release.value, Sustain = _sustain.value };
        set
        {
            _attack.value = value.Attack;
            _decay.value = value.Decay;
            _sustain.value = value.Sustain;
            _release.value = value.Release;
        }
    }

    private void OnEnable()
    {
        _attack.onValueChanged.AddListener(OnSliderChanged);
        _decay.onValueChanged.AddListener(OnSliderChanged);
        _sustain.onValueChanged.AddListener(OnSliderChanged);
        _release.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDisable()
    {
        _attack.onValueChanged.RemoveListener(OnSliderChanged);
        _decay.onValueChanged.RemoveListener(OnSliderChanged);
        _sustain.onValueChanged.RemoveListener(OnSliderChanged);
        _release.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        ValueChanged?.Invoke();
    }

}
