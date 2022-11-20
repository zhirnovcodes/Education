using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextEditView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_InputField _input;

    public event Action<float> ValueChanged;

    private bool _shouldInvoke = true;

    public float Value
    {
        get => _slider.value;
        set
        {
            _slider.value = value;
        }
    }

    private void OnEnable()
    {
        _input.text = Math.Round(_slider.value, 2).ToString();

        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        _input.onSubmit.AddListener(OnInputValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        _input.onSubmit.RemoveListener(OnInputValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (_shouldInvoke)
        {
            _shouldInvoke = false;
            _input.text = Math.Round(value, 2).ToString();
            _shouldInvoke = true;
            ValueChanged?.Invoke(value);
        }
    }

    private void OnInputValueChanged(string valueStr)
    {
        if (_shouldInvoke)
        {
            if (float.TryParse(valueStr, out var value))
            {
                _shouldInvoke = false;
                _slider.value = value;
                _shouldInvoke = true;
                ValueChanged?.Invoke(value);
            }
        }
    }
}
