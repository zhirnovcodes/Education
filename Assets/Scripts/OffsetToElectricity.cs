using System.Collections.Generic;
using UnityEngine;

public class OffsetToElectricity : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _source;
    [SerializeField] private Material _material;
    [SerializeField] private Color _from;
    [SerializeField] private Color _to;
    [SerializeField] private float _power = 1;
    [SerializeField] private float _maxOffset = 2;
    [SerializeField] private bool _reflected;
    [SerializeField] private bool _zeroIfUnplugged = false;

    private void Update()
    {
        var offset = _source.Value * _power;

        var t = Mathf.InverseLerp(-_maxOffset, _maxOffset, offset);
        t = _reflected ? offset * -1 : offset;

        t = _zeroIfUnplugged ? (ElectricitySettings.IsPluged ? offset : 0) : offset;

        var resColor = Color.Lerp(_from, _to, t);
        resColor.a = _material.color.a;
        _material.color = resColor;

    }
}
