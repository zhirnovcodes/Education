using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrigFunctionType
{
    Sin = 0,
    Square = 1,
    Saw = 2
}

[System.Serializable]
public struct Trigonometry
{
    public TrigFunctionType Type;
    public float Period;
    public float Amplitude;
    public float XShift;
}

public class TrigonometryFunction : FunctionBase
{
    [SerializeField] private Trigonometry _function = new Trigonometry()
    {
        Type = TrigFunctionType.Sin,
        Amplitude = 1,
        Period = 1
    };
    [SerializeField] private float _frequency;

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

    public override float Value => GetValue(Time.time - _timeStart);

    public override float GetValue(float t)
    {
        return _function.GetValue(t);
    }

}


public static class TrigonometryExtentions
{
    public static float GetValue(this Trigonometry func, float time)
    {
        var t = time + func.XShift;
        switch (func.Type)
        {
            case TrigFunctionType.Sin:
                t *= 2f * Mathf.PI / func.Period;
                return Mathf.Sin(t) * func.Amplitude;
            case TrigFunctionType.Square:
                t = t / func.Period;
                return (t - Mathf.Floor(t) <= 0.5 ? 1 : -1) * func.Amplitude;
            case TrigFunctionType.Saw:
                return (t % func.Period * 2 - 1) * func.Amplitude;
            default:
                throw new System.NotImplementedException();
        }
    }
}