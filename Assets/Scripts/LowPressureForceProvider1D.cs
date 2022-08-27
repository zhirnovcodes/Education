using UnityEngine;

public class LowPressureForceProvider1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private Rigidbody2D _left;
    [SerializeField] private Rigidbody2D _right;
    
    private Rigidbody2D _rigidbody = null;

    [SerializeField] private bool _shouldLog = false;


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

        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();

        var d = (lp.x + rp.x) / 2 - _rigidbody.position.x;
        var x = d == 0 ? 0 : Mathf.Sign(d);//Mathf.Abs(d) < Air.Density ? 0 : (Mathf.Clamp(d, -1, 1) * (Mathf.Abs( d ) - Air.Density));//Mathf.Clamp( d, -1, 1 );
        var f = new Vector2(x, 0);
        return f;
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
