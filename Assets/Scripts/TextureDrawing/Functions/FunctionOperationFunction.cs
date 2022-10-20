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

    public override float Value
    {
        get
        {
            switch (_operation)
            {
                case FunctionOperation.Minus:
                    return _f1.Value - _f2.Value;
                case FunctionOperation.Plus:
                    return _f1.Value + _f2.Value;
                case FunctionOperation.Multiply:
                    return _f1.Value * _f2.Value;
                case FunctionOperation.Divide:
                    return _f1.Value / _f2.Value;
            }
            return 0;
        }
    }
}
