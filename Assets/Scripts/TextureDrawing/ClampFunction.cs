using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _min = 0;
    [SerializeField] private float _max = 1;

    public override float Value => Mathf.Clamp(_function.Value, _min, _max);

}
