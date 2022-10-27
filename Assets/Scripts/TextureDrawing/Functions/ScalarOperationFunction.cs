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

    public override float Value
    {
        get
        {
            var v = _function.Value;

            switch (_operation)
            {
                case FunctionOperation.Minus:
                    v -= _scalar;
                    break;
                case FunctionOperation.Plus:
                    v += _scalar;
                    break;
                case FunctionOperation.Multiply:
                    v *= _scalar;
                    break;
                case FunctionOperation.Divide:
                    v /= _scalar;
                    break;
            }

            return v;
        }
    }
}
