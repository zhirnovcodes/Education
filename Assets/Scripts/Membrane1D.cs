using UnityEngine;

public class Membrane1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    [SerializeField] private float _stabelizeDistance = 0.2f;
    [SerializeField] private float _maxDeviation = 1f;

    private Rigidbody2D _rigidbody;
    private Vector2? _stablePosition;

    public Vector2 GetForce()
    {
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
        _stablePosition = _stablePosition ?? _rigidbody.position;

        var dir = _stablePosition.Value - _rigidbody.position;
        var sqrDist = dir.sqrMagnitude;

        //var dirPower = Mathf.Clamp01( Mathf.InverseLerp(_stabelizeDistance * _stabelizeDistance, _maxDeviation * _maxDeviation, sqrDist) );
        var dirPower = Mathf.Max (0, InvLerp(_stabelizeDistance, _maxDeviation, Mathf.Sqrt( sqrDist )));
        return dir.normalized * dirPower * _power;
    }

    private float InvLerp(float a, float b, float x)
    {
        b = a == b ? b + 0.000001f : b;
        return (x - a) / (b - a);
    }
}
