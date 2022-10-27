using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathIZ
{
    public static float GetValue(this Fluctuation f, float x)
    {
        if (x <= 0)
        {
            return 0;
        }

        var time = Mathf.Max(f.Time, 0.01f);
        var amp = Mathf.Lerp(f.Amplitude, 0, Mathf.InverseLerp(0, time, x));
        amp *= f.Attack == 0 ? 1 : Mathf.InverseLerp(0, f.Attack, x);
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


    public static Vector3? ReflectedPosition(Vector3 position, Line line)
    {
        var perp = (Vector3)Vector2.Perpendicular(line.Direction).normalized;

        var crossPoint = CrossPoint(line, new Line() { P1 = position, P2 = perp + position });
        if (crossPoint == null)
        {
            return null;
        }

        var dir = crossPoint.Value - position;
        var pos = dir * 2 + position;

        return pos;
    }

    public static Vector3? CrossPoint(Line l1, Line l2)
    {
        var x1 = l1.P1.x;
        var y1 = l1.P1.y;

        var x2 = l1.P2.x;
        var y2 = l1.P2.y;

        var x3 = l2.P1.x;
        var y3 = l2.P1.y;

        var x4 = l2.P2.x;
        var y4 = l2.P2.y;

        float n;

        if (y2 - y1 != 0)
        {  // a(y)
            float q = (x2 - x1) / (y1 - y2);
            float sn = (x3 - x4) + (y3 - y4) * q;
            if (sn == 0)
            {
                return null;
            }  // c(x) + c(y)*q
            float fn = (x3 - x1) + (y3 - y1) * q;   // b(x) + b(y)*q
            n = fn / sn;
        }
        else
        {
            if (y3 - y4 == 0)
            {
                return null;
            }  // b(y)
            n = (y3 - y1) / (y3 - y4);   // c(y)/b(y)
        }
        var x = x3 + (x4 - x3) * n;  // x3 + (-b(x))*n
        var y = y3 + (y4 - y3) * n;  // y3 +(-b(y))*n
        return new Vector3(x, y, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectPosition"></param>
    /// <param name="circlePosition"></param>
    /// <param name="radius"></param>
    /// <returns>0 if object in center, 1 - on the radius</returns>
    public static float LerpObjectInSphere(Vector3 objectPosition, Vector3 circlePosition, float radius, bool clamp = false)
    {
        var res = (objectPosition - circlePosition).magnitude / radius;
        return clamp ? Mathf.Clamp01(res) : res;
    }
}

