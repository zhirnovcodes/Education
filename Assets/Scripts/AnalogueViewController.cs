using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogueViewController : MonoBehaviour
{
    [SerializeField] private Toggle _togle;
    [SerializeField] private Transform _speaker;
    [SerializeField] private Slider _visible;
    [SerializeField] private ShowModelSettings _settings;
    [SerializeField] private ElectricitySettings _elecSettings;

    [SerializeField] private Vector3 _speakerPosPluged;
    [SerializeField] private Vector3 _speakerPosUnPluged;

    private void OnEnable()
    {
        _visible.value = _settings.Value;

        _visible.onValueChanged.AddListener(SliderValueChange);
        if (_togle != null)
        {
            _togle.onValueChanged.AddListener(ToggleValueChange);
        }
    }

    private void OnDisable()
    {
        _visible.onValueChanged.RemoveListener(SliderValueChange);
        if (_togle != null)
        {
            _togle.onValueChanged.RemoveListener(ToggleValueChange);
        }
    }

    private void SliderValueChange(float value)
    {
        _settings.Value = value;
    }

    private void ToggleValueChange(bool value)
    {
        _elecSettings.SetIsPlugged(value);

        _speaker.position = value ? _speakerPosPluged : _speakerPosUnPluged;
    }
}
