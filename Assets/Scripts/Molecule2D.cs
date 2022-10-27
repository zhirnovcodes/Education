using UnityEngine;

public class Molecule2D : MonoBehaviour
{
    [SerializeField] private FunctionBuffer _buffer;
    [SerializeField] private ReverbZone _reverb;

    private Vector3 _stablePosition;

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

        foreach (var dir in _reverb.Directions(_stablePosition))
        {
            var t = dir.w;
            d += (Vector3)dir * _buffer.GetValue(Time.time - t);
           // break;
        }

        transform.position = _stablePosition + d;
    }
}
