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

    public Vector2 Position
    {
        get => GetRigidbody().position; 
        set
        {
            Reset();
            GetRigidbody().position = value;
            _stable = value;
        }
    }

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

    private Rigidbody2D GetRigidbody()
    {
        _rigid = _rigid ?? GetComponent<Rigidbody2D>();
        return _rigid;
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
        _stable = _stable ?? GetRigidbody().position;

        if (_amplitudeCurr <= 0)
        {
            GetRigidbody().MovePosition(_stable.Value);
            return;
        }

        var amp = Mathf.Lerp(_amplitudeCurr, 0, Mathf.InverseLerp(_timeStart, _timeStart + DecayTime, Time.realtimeSinceStartup));
        if (amp <= 0)
        {
            _amplitudeCurr = 0;
            GetRigidbody().MovePosition(_stable.Value);
            return;
        }
        var offset = Mathf.Sin((Time.realtimeSinceStartup * Time.timeScale - _timeStart) * Frequency * 2 * Mathf.PI ) * amp * Vector3.left;
        var position = offset + _stable.Value;
        GetRigidbody().MovePosition(position);
    }
}
