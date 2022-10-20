using UnityEngine;


public class BufferDelayFunction : GraphFunctionBase
{
    [SerializeField] private float _delay;

    [SerializeField] public FunctionBuffer _buffer;

    public override float Value => _buffer.GetValue(Time.time - _delay);
}
