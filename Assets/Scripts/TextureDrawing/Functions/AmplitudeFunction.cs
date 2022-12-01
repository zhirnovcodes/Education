using UnityEngine;

public class AmplitudeFunction : GraphFunctionBase
{
    [SerializeField] private TrigonometryFunction _func;

    public override float Value => _func.Function.Amplitude;

}
