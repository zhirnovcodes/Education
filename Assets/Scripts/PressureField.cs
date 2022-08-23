using UnityEngine;

public class PressureField : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _object;
    [SerializeField] private SpriteRenderer _renderer;

    private Vector3 _basePosition;
    private Vector3 _lastPosition;
    private Vector3 _peak;
    private float _lastSign;
    private Color _baseColor;

    private Vector3 _position;
    private Vector3 _scale;

    public Vector3 Power 
    { 
        get
        {
            return Mathf.Clamp(Mathf.Abs(transform.localScale.x) <= 0.05 ? 0 : transform.localScale.x, 0, 500) * Vector3.left;
        }
    }

    void Start()
    {
        _basePosition = transform.position;
        _baseColor = _renderer.color;
        _peak = _basePosition;
        _lastPosition = _basePosition;
        _lastSign = 0;

        _position = _basePosition;
        _scale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var diff = _lastPosition.x - _object.position.x;
        var newSign = Mathf.Sign(diff);
        if (newSign != _lastSign && newSign > 0)
        {
            _peak = _lastPosition;
        }
        _lastSign = newSign;
        _lastPosition = _object.position;

        var distance = _peak - (Vector3)_object.position;
        var position = (Vector3)_object.position + distance / 2;
        var scaleX = distance.x;
        var scale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

        //transform.position = position;
        //transform.localScale = scale;
        _position = position;
        _scale = scale;

        var maxDev = (_peak - _basePosition).x * 2;

        var newColor = _baseColor;
        newColor.a = Mathf.Clamp01( Mathf.InverseLerp(0, maxDev, scaleX));
        _renderer.color = newColor;
    }

    void Update()
    {
        transform.position = _position;
        transform.localScale = _scale;
    }
}
