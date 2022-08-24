using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPressureForceProvider1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    [SerializeField] private float _maxDistance = 5;
    [SerializeField] private bool _shouldLog = false;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _hits = new RaycastHit2D[1];

    public Vector2 Force { get; private set; }

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var leftCast = _collider.Raycast(Vector2.left, _hits, 10);

        var sqrDistance = Mathf.Infinity;
        var direction = Vector3.zero;

        if (leftCast > 0)
        {
            sqrDistance = (_hits[0].rigidbody.position - _collider.attachedRigidbody.position).sqrMagnitude;
        }

        var rightCast = _collider.Raycast(Vector2.right, _hits, _maxDistance);

        if (rightCast > 0)
        {
            var rightSqrDistance = (_hits[0].rigidbody.position - _collider.attachedRigidbody.position).sqrMagnitude;
            direction = rightSqrDistance > sqrDistance ? Vector3.right : Vector3.left;
            sqrDistance = Mathf.Max(rightSqrDistance, sqrDistance);
        }

        if (sqrDistance == Mathf.Infinity)
        {
            Force = Vector3.zero;
            return;
        }

        var powerDistance = Mathf.Clamp01( Mathf.InverseLerp(0, _maxDistance * _maxDistance, sqrDistance));
        //var powerDistance = Mathf.Clamp01( Mathf.InverseLerp(maxDistance * maxDistance, 0, sqrDistance));

        var direction2D = (Vector2)(direction * _power * powerDistance);
        Force = direction2D;

        if (_rigidbody.isKinematic)
        {
            return;
        }
        _rigidbody.AddForce(Force);
        //_rigidbody.MovePosition(_rigidbody.position + direction2D);

    }
}
