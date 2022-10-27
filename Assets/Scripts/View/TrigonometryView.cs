using System;
using UnityEngine;
using UnityEngine.UI;

public class TrigonometryView : MonoBehaviour
{
    [SerializeField] private Dropdown _type;
    [SerializeField] private Slider _period;
    [SerializeField] private Slider _amplitude;

    public event Action ValueChanged;

    public Trigonometry Function
    {
        get
        {
            var type = (TrigFunctionType)Mathf.Max(_type.value, 0);
            return new Trigonometry()
            {
                Type = type,
                Period = _period.value,
                Amplitude = _amplitude.value
            };
        }
        set
        {
            var type = (int)value.Type;
            _type.value = type;
            _period.value = value.Period;
            _amplitude.value = value.Amplitude;
        }
    }

    private void OnEnable()
    {
        _type.onValueChanged.AddListener(OnValueChangedInt);
        _period.onValueChanged.AddListener(OnValueChangedFloat);
        _amplitude.onValueChanged.AddListener(OnValueChangedFloat);
    }

    private void OnDisable()
    {
        _type.onValueChanged.RemoveListener(OnValueChangedInt);
        _period.onValueChanged.RemoveListener(OnValueChangedFloat);
        _amplitude.onValueChanged.RemoveListener(OnValueChangedFloat);
    }

    private void OnValueChangedInt(int value)
    {
        ValueChanged?.Invoke();
    }

    private void OnValueChangedFloat(float value)
    {
        ValueChanged?.Invoke();
    }
}
