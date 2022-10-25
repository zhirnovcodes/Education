using UnityEngine;

public class Molecule1D : FunctionBase
{
    [SerializeField] private FunctionBuffer _buffer;
    [SerializeField] private float _delay;
    [SerializeField] private float _power = 1;

    public FunctionBuffer Buffer { set { _buffer = value; } }
    public float Delay { set { _delay = value; } }
    public float Power { set { _power = value; } }

    public override float GetValue(float t)
    {
        return _buffer.GetValue(t - _delay) * _power;
    }
}
