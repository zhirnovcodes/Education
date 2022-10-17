using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FunctionsBuffer))]
public class MinMaxCalculator : MonoBehaviour
{
    [SerializeField] private FunctionsBuffer _buffer;
    [SerializeField] private float _timeDelay = 0.2f;

    public float Max { get; private set; }
    public float Min { get; private set; }

    private Coroutine _routine;

    void OnEnable()
    {
        Max = 0;
        Min = 0;
        _routine = StartCoroutine(Routine());
    }

    private void Start()
    {
    }

    private void OnDisable()
    {
        if (_routine == null)
        {
            return;
        }
        StopCoroutine(_routine);
        _routine = null;
    }

    private IEnumerator Routine()
    {
        var hasStarted = false;

        while (_routine != null || !hasStarted)
        {
            CalcMax();

            yield return new WaitForSeconds(_timeDelay);

            CalcMin();

            yield return new WaitForSeconds(_timeDelay);

            hasStarted = true;
        }
    }

    private void CalcMax()
    {
        var max = float.MinValue;

        foreach (var v in _buffer.Values)
        {
            if (v > max)
            {
                max = v;
            }
        }

        Max = max == float.MinValue ? 0 : max;
    }

    private void CalcMin()
    {
        var min = float.MaxValue;

        foreach (var v in _buffer.Values)
        {
            if (v < min)
            {
                min = v;
            }
        }

        Min = min == float.MaxValue ? 0 : min;
    }
}
