using UnityEngine;

public class AmplitudeFunction : GraphFunctionBase
{
    [SerializeField] private FluctuatingString1D _string;

    public override float Value => _string.Fluctuation.Amplitude;

}
