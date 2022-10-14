using UnityEngine;

public interface IThresholdFunction
{
    public float Threshold { get; set; }
}

public class ThresholdFunction : GraphFunctionBase, IThresholdFunction
{
    [SerializeField] float _threshold = 1;

    public override float Value
    {
        get
        {
            return Threshold;
        }
    }

    public float Threshold { get => _threshold; set => _threshold = value; }
}
