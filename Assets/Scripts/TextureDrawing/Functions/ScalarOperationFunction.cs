using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarOperationFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private FunctionOperation _operation;
    [SerializeField] private float _scalar = 0;

    public float Scalar
    {
        set
        {
            _scalar = value;
        }
        get
        {
            return _scalar;
        }
    }

    private float _value;

    public override float Value => _value;

    private void Update()
    {
        _value = _function.Value;

        switch (_operation)
        {
            case FunctionOperation.Minus:
                _value -= _scalar;
                break;
            case FunctionOperation.Plus:
                _value += _scalar;
                break;
            case FunctionOperation.Multiply:
                _value *= _scalar;
                break;
            case FunctionOperation.Divide:
                _value /= _scalar;
                break;
        }

    }
}
