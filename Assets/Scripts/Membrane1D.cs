using UnityEngine;

public class Membrane1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    [SerializeField] private float _maxDeviation = 1f;

    private Rigidbody2D _rigidbody;
    private Vector2? _stablePosition;

    public Vector2 GetForce()
    {
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
        _stablePosition = _stablePosition ?? _rigidbody.position;

        var dir = _stablePosition.Value - _rigidbody.position;
        var x = (InvLerp(-_maxDeviation, _maxDeviation, dir.x) - 0.5f) * 2;

        return new Vector2(x, 0) * _power;
    }

    private void OnDisable()
    {
        _stablePosition = null;
    }

    private float InvLerp(float a, float b, float x)
    {
        b = a == b ? b + 0.000001f : b;
        return (x - a) / (b - a);
    }
}
