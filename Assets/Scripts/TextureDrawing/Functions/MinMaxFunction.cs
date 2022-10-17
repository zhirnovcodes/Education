using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MinMaxCalculator))]
public class MinMaxFunction : GraphFunctionBase
{
    [SerializeField] private MinMaxCalculator _calc;
    [SerializeField] private bool _isMin;

    public override float Value => _isMin ? _calc.Min : _calc.Max;
}
