using System.Collections;
using UnityEngine;

public abstract class GraphFunctionBase : MonoBehaviour
{
    public abstract float Value { get; }
}

public abstract class FunctionBase : GraphFunctionBase
{
    public override float Value => GetValue(Time.time);
    public abstract float GetValue(float t);
}
