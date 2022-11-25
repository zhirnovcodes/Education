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


public class FunctionOperationFunction : FunctionBase
{
    [SerializeField] private GraphFunctionBase _f1;
    [SerializeField] private GraphFunctionBase _f2;
    [SerializeField] private FunctionOperation _operation;

    public override float Value
    {
        get
        {
            return Operation(_operation, _f1.Value, _f2.Value);
        }
    }

    public override float GetValue(float t)
    {
        if (_f1 is FunctionBase f1)
        {
            if (_f2 is FunctionBase f2)
            {
                return Operation(_operation, f1.GetValue(t), f2.GetValue(t));
            }
            Debug.LogError("FunctionBase error: " + _f2.GetType().Name);
        }
        else
        {
            Debug.LogError("FunctionBase error: " + _f1.GetType().Name);
        }
        return 0;
    }

    public static float Operation(FunctionOperation operation, float v1, float v2)
    {
        switch (operation)
        {
            case FunctionOperation.Minus:
                return v1 - v2;
            case FunctionOperation.Plus:
                return v1 + v2;
            case FunctionOperation.Multiply:
                return v1 * v2;
            case FunctionOperation.Divide:
                return v1 / v2;
        }
        return 0;
    }
}
