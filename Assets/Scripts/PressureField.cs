using UnityEngine;

public class PressureField : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _object;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _pressurePower = 1;
    
    private Collider2D _collider;

    private Vector3 _basePosition;
    private Vector3 _lastPosition;
    private Vector3 _peak;
    private float _lastSign;
    private Color _baseColor;

    private Vector3 _position;
    private Vector3 _scale;

    private Vector3 _power;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }

    void Start()
    {
        _basePosition = _renderer.transform.position;
        _baseColor = _renderer.color;
        _peak = _basePosition;
        _lastPosition = _basePosition;
        _lastSign = 0;

        _position = _basePosition;
        _scale = _renderer.transform.localScale;

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
        var scale = new Vector3(scaleX, _renderer.transform.localScale.y, _renderer.transform.localScale.z);

        //transform.position = position;
        //transform.localScale = scale;
        _position = position;
        _scale = scale;

        var maxDev = (_peak - _basePosition).x * 2;

        _power = Mathf.Clamp(Mathf.Abs(scaleX) <= 0.05 ? 0 : scaleX, 0, 500) * Vector3.left;

        var newColor = _baseColor;
        newColor.a = Mathf.Clamp01( Mathf.InverseLerp(0, maxDev, scaleX));
        _renderer.color = newColor;

        _collider.enabled = newColor.a > 0;
    }

    void Update()
    {
        _renderer.transform.position = _position;
        _renderer.transform.localScale = _scale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.attachedRigidbody.AddForce(_power * _pressurePower);
    }
}
