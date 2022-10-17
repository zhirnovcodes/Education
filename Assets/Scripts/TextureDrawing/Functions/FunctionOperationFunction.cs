using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FunctionOperation
{
    Minus = 0,
    Plus = 1,
    Multiply = 2,
    Divide = 3
}


public class FunctionOperationFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _f1;
    [SerializeField] private GraphFunctionBase _f2;
    [SerializeField] private FunctionOperation _operation;

    private float _value;

    private void OnEnable()
    {
        _value = 0;
    }

    public override float Value
    {
        get
        {
            return _value;
        }
    }

    private void Update()
    {
        switch (_operation)
        {
            case FunctionOperation.Minus:
                _value = _f1.Value - _f2.Value;
                break;
            case FunctionOperation.Plus:
                _value = _f1.Value + _f2.Value;
                break;
            case FunctionOperation.Multiply:
                _value = _f1.Value * _f2.Value;
                break;
            case FunctionOperation.Divide:
                _value = _f1.Value / _f2.Value;
                break;
        }
    }
}
