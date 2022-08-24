using UnityEngine;

public class Ear : MonoBehaviour
{
    private Vector3 _stable;
    private Rigidbody2D _rigid;
    public float Offset { get; private set; }

    void Start()
    {
        _rigid = _rigid ?? GetComponent<Rigidbody2D>();
        _stable = _rigid.position;
    }

    void FixedUpdate()
    {
        var offset = (Vector3)_rigid.position - _stable;
        Offset = offset.x;
    }
}
