using UnityEngine;

public class LowPressureForceProvider1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _maxSpeed = 100;
    [SerializeField] private float _maxDistance = 5;
    [SerializeField] private bool _shouldLog = false;

    [SerializeField] private Rigidbody2D _left;
    [SerializeField] private Rigidbody2D _right;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _hits = new RaycastHit2D[1];

    public Rigidbody2D Left { get => _left; set { _left = value; } }
    public Rigidbody2D Rigth { get => _right; set { _right = value; } }

    public Vector2 GetForce()
    {
        if (_left == null || _right == null)
        {
            return Vector2.zero;
        }

        var lp = _left.position + Vector2.right * _left.transform.localScale.x / 2;
        var rp = _right.position + Vector2.left * _right.transform.localScale.x / 2;

        /*
        var ld = (lp - _rigidbody.position) / 2f;
        var rd = (rp - _rigidbody.position) / 2f;

        var sqrDistanceLeft = ld.sqrMagnitude;
        var sqrDistanceRight = rd.sqrMagnitude;

        var direction = sqrDistanceLeft > sqrDistanceRight ? ld : rd;
        var distanceSqr = Mathf.Max(sqrDistanceLeft, sqrDistanceRight);

        if (distanceSqr >= _maxSpeed * _maxSpeed)
        {
            direction = direction.normalized * _maxSpeed;
        }

        var direction2D = (Vector2)(direction * _speed );
        
        return direction2D;
        */

        var f =  ((lp + rp) / 2 - _rigidbody.position) * _speed;
        f.y = 0;
        return f;
    }

    private void Start()
    {
        _collider = _collider ?? GetComponent<Collider2D>();
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
    }

    private void Log(object log)
    {
        if (_shouldLog)
        {
            Debug.Log(log);
        }
    }
    private float InvLerp(float a, float b, float x)
    {
        b = a == b ? b + 0.000001f : b;
        return (x - a) / (b - a);
    }
}
