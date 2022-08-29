using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathIZ
{
    public static float GetValue(this Fluctuation f, float x)
    {
        var amp = Mathf.Lerp(f.Amplitude, 0, Mathf.InverseLerp(0, f.Length, x));
        amp *= Mathf.InverseLerp(0, f.Attack, x);
        if (amp <= 0)
        {
            return 0;
        }
        var res = Mathf.Sin(x * f.Frequency * 2 * Mathf.PI) * amp;
        return res;
    }
    public static float InvLerp(float a, float b, float x)
    {
        b = a == b ? b + 0.000001f : b;
        return (x - a) / (b - a);
    }
}

