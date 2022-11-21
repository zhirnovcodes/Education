using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedFunction : FunctionBase
{
    [SerializeField] private GraphFunctionBase _function;    
    [SerializeField] private float _maxValue = 1;

    public override float Value => _function.Value / (_maxValue * 2) + (_maxValue / 2);

    public override float GetValue(float t)
    {
        if ( _function is FunctionBase f)
        {
            return f.GetValue(t) / (_maxValue * 2) + (_maxValue / 2);
        }
        Debug.LogError("FunctionBase error: " + _function.GetType().Name);
        return 0;
    }
}
