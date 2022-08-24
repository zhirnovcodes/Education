using UnityEngine;

public class String1D : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private float _decayTime;
    [SerializeField] private float _amplitude;

    private Vector3? _stable;
    private Rigidbody2D _rigid;
    private float _amplitudeCurr;
    private float _timeStart;

    public Vector3 Position => _rigid.position;

    public PressureField Field { get; private set; }

    public float Frequency
    {
        set
        {
            Reset();
            _frequency = value;
        }
        get
        {
            return _frequency;
        }
    }
    public float DecayTime
    {
        set
        {
            Reset();
            _decayTime = value;
        }
        get
        {
            return _decayTime;
        }
    }
    public float Amplitude
    {
        set
        {
            Reset();
            _amplitude = value;
        }
        get
        {
            return _amplitude;
        }
    }

    void Start()
    {
        _rigid = _rigid ?? GetComponent<Rigidbody2D>();
        Field = transform.parent.GetComponentInChildren<PressureField>();
    }

    public void Hit()
    {
        _amplitudeCurr = Amplitude;
        _timeStart = Time.realtimeSinceStartup;
    }

    public void Reset()
    {
        _amplitudeCurr = 0;
    }

    void FixedUpdate()
    {
        _stable = _stable ?? _rigid.position;

        if (_amplitudeCurr <= 0)
        {
            _rigid.MovePosition(_stable.Value);
            return;
        }

        var amp = Mathf.Lerp(_amplitudeCurr, 0, Mathf.InverseLerp(_timeStart, _timeStart + DecayTime, Time.realtimeSinceStartup));
        if (amp <= 0)
        {
            _amplitudeCurr = 0;
            _rigid.MovePosition(_stable.Value);
            return;
        }
        var offset = Mathf.Sin((Time.realtimeSinceStartup - _timeStart) * Frequency * 2 * Mathf.PI ) * amp * Vector3.left;
        var position = offset + _stable.Value;
        _rigid.MovePosition(position);
    }
}
