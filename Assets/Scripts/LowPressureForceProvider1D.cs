using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPressureForceProvider1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
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

        const float maxDistance = 10f;

        var rightCast = _collider.Raycast(Vector2.right, _hits, maxDistance);

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

        //var powerDistance = Mathf.Clamp01( Mathf.InverseLerp(0, maxDistance * maxDistance, sqrDistance));
        var powerDistance = Mathf.Clamp01( Mathf.InverseLerp(maxDistance * maxDistance, 0, sqrDistance));

        var direction2D = (Vector2)(direction * _power * powerDistance * Time.fixedDeltaTime);
        Force = direction2D;
        //_rigidbody.MovePosition(_rigidbody.position + direction2D);

    }
}
