using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModelSettings : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _value;
    [SerializeField] private Material _devices;
    [SerializeField] private Material[] _models;

    public float Value 
    {
        set
        {
            _value = value;
            UpdateValue();
        }
        get
        {
            return _value;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateValue();
    }
#endif

    private void UpdateValue()
    {
        if (_devices == null || (_models?.Length ?? 0) == 0)
        {
            return;
        }

        var devCol = _devices.color;
        devCol.a = Mathf.Clamp01( (1 - _value) + 0.125f);
        _devices.color = devCol;

        foreach (var model in _models)
        {
            if (model == null)
            {
                return;
            }
            var elCol = model.color;
            elCol.a = Mathf.Clamp01(_value);
            model.color = elCol;
        }
    }
}
