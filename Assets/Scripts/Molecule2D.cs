using UnityEngine;

public class Molecule2D : GraphFunctionBase
{
    [SerializeField] private FunctionBuffer _buffer;
    [SerializeField] private ReverbZone _reverb;
    [SerializeField] private bool _shouldMove = true;

    private Vector3 _stablePosition;
    private float _value;

    public override float Value => _value;

    private void Awake()
    {
        if (_reverb == null)
        {
            _reverb = ReverbZone.Instance;
        }
    }

    private void OnEnable()
    {
        _stablePosition = transform.position;
        _value = 0;
    }

    private void OnDisable()
    {
        transform.position = _stablePosition;
    }

    private void Update()
    {
        if (!enabled)
        {
            return;
        }

        var d = Vector3.zero;
        _value = 0;

        foreach (var dir in _reverb.Directions(_stablePosition))
        {
            var t = dir.w;
            var val = _buffer.GetValue(Time.time - t);
            _value += val;
            d += (Vector3)dir * val;
           // break;
        }

        _value *= d.magnitude;
        transform.position = _stablePosition + d;
    }
}
