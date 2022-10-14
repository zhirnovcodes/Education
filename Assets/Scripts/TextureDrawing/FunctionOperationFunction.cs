using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionOperationFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _f1;
    [SerializeField] private GraphFunctionBase _f2;
    [SerializeField] private Operation _operation;

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

    public enum Operation
    {
        Minus = 0,
        Plus = 1
    }

    private void Update()
    {
        switch (_operation)
        {
            case Operation.Minus:
                _value = _f1.Value - _f2.Value;
                break;
            case Operation.Plus:
                _value = _f1.Value + _f2.Value;
                break;
        }
    }
}
