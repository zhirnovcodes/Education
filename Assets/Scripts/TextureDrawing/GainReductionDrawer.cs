using UnityEngine;

public class GainReductionDrawer : PositionOffsetDrawer
{
    [SerializeField] private float _threshold;
    [SerializeField] private float _attack;
    [SerializeField] private float _release;
    [SerializeField] private float _ratio;


    private float? _timeStart;

    private float GetFunction(float t)
    {
        if (t < _attack)
        {
            return Mathf.Lerp(0, 1, (_attack - t) / _attack) * _ratio;
        }

        t -= _attack;

        if (t < _release)
        {
            return Mathf.Lerp(1, 0, (_release - t) / _release) * _ratio;
        }

        return 0;
    }

    protected override float? GetValue()
    {
        var offset = base.GetValue();
        if (offset == null)
        {
            return null;
        }

        if (offset.Value > _threshold)
        {
            if (_timeStart == null)
            {
                _timeStart = Time.time;
            }
            return offset - GetFunction(_timeStart.Value - Time.time);
        }

        if (_timeStart == null)
        {
            return null;
        }

        var time = Time.time - _timeStart.Value;

        if (time >= _attack + _release)
        {
            _timeStart = null;
            return 0;
        }

        return offset - GetFunction(time);
    }
}
