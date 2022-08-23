using UnityEngine;

public class MetalString : MonoBehaviour
{
    public float Frequency;
    public float DecayTime;

    private Vector3? _stable;
    private Rigidbody2D _rigid;
    private float _amplitude;
    private float _amplitudeCurr;
    private float _timeStart;

    void Start()
    {

        _rigid = _rigid ?? GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _stable = _stable ?? _rigid.position;

        if (_amplitude <= 0)
        {
            _rigid.MovePosition(_stable.Value);
            return;
        }

        var amp = Mathf.Lerp(_amplitude, 0, Mathf.InverseLerp(_timeStart, _timeStart + DecayTime, Time.realtimeSinceStartup));
        if (amp <= 0)
        {
            _rigid.MovePosition(_stable.Value);
            return;
        }
        var offset = Mathf.Sin((Time.realtimeSinceStartup - _timeStart) * Frequency) * amp * Vector3.left;
        var position = offset + _stable.Value;
        _rigid.MovePosition(position);
    }

    public void Hit(float amplitude)
    {
        _amplitude = amplitude;
        _timeStart = Time.realtimeSinceStartup;
    }
}
