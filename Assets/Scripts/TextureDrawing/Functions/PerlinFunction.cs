using UnityEngine;

public class PerlinFunction : FunctionBase
{
    [SerializeField] private float _xScale = 1;
    [SerializeField] private float _y;

    public override float GetValue(float t)
    {
        return (Mathf.PerlinNoise(t * _xScale, _y )- 0.5f) * 2f;
    }
}
