using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _function;    
    [SerializeField] private float _maxValue = 3;

    public override float Value => _function.Value + _maxValue;
}
