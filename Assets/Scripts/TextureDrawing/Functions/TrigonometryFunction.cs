using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrigFunctionType
{
    Sin = 0,
    Square = 1
}

[System.Serializable]
public struct Trigonometry
{
    public TrigFunctionType Type;
    public float Period;
    public float Amplitude;
}

public class TrigonometryFunction : GraphFunctionBase
{
    [SerializeField] private Trigonometry _function = new Trigonometry()
    {
        Type = TrigFunctionType.Sin,
        Amplitude = 1,
        Period = 1
    };

    public Trigonometry Function
    {
        get => _function;
        set
        {
            _function = value;
        }
    }

    private float _timeStart;

    private void OnEnable()
    {
        _timeStart = Time.time;
    }

    public override float Value => Func(Time.time - _timeStart);


    private float Func(float t)
    {
        switch (_function.Type)
        {
            case TrigFunctionType.Sin:
                t *= 2f * Mathf.PI / _function.Period;
                return Mathf.Sin(t) * _function.Amplitude;
            case TrigFunctionType.Square:
                t = t / _function.Period;
                return (t - Mathf.Floor(t) <= 0.5 ? 1 : -1) * _function.Amplitude;
            default:
                throw new System.NotImplementedException();
        }
    }
}
