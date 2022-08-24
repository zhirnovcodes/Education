using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Membrane1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    [SerializeField] private float _stabelizeDistance = 0.2f;
    [SerializeField] private float _maxDeviation = 1f;

    private Rigidbody2D _rigidbody;
    private Vector2? _stablePosition;

    public Vector2 Force { get; private set; }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _stablePosition = _stablePosition ?? _rigidbody.position;

        var dir = _stablePosition.Value - _rigidbody.position;
        var sqrDist = dir.sqrMagnitude;

        if (sqrDist < _stabelizeDistance * _stabelizeDistance)
        {
            Force = Vector2.zero;
            return;
        }

        //var dirPower = Mathf.Clamp01( Mathf.InverseLerp(_stabelizeDistance * _stabelizeDistance, _maxDeviation * _maxDeviation, sqrDist) );
        var dirPower = Mathf.Clamp01( Mathf.InverseLerp(_stabelizeDistance * _stabelizeDistance, _maxDeviation * _maxDeviation, sqrDist) ) * _power;
        Force = dir.normalized * dirPower;
        if (_rigidbody.isKinematic)
        {
            return;
        }
        _rigidbody.AddForce(Force);
    }
}
