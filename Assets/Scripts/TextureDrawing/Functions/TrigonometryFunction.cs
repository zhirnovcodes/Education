using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrigFunctionType
{
    Sin = 0,
    Square = 1
}

public class TrigonometryFunction : GraphFunctionBase
{
    [SerializeField] private TrigFunctionType _type;
    [SerializeField] private float _period = 1;
    [SerializeField] private float _amplitude = 1;

    private float _timeStart;
    private float _value;

    private void OnEnable()
    {
        _timeStart = Time.time;
        _value = 0;
    }

    public override float Value => Func(Time.time - _timeStart) * _amplitude;


    private float Func(float t)
    {
        switch (_type)
        {
            case TrigFunctionType.Sin:
                t *= 2f * Mathf.PI / _period;
                return Mathf.Sin(t);
            case TrigFunctionType.Square:
                t = t / _period;
                return t - Mathf.Floor(t) <= 0.5 ? 1 : -1;
            default:
                throw new System.NotImplementedException();
        }
    }
}
