using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0,3)] private float _floatSpeed = 0f;
    [SerializeField, Range(0,1)] private float _floatDistance = 0.1f;
    [SerializeField] private float _minSize = 5f; 
    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _zoomSpeed = 100f;
    [SerializeField, Range(0, 20)] private float _slide = 10f;
    [SerializeField] private Rect _borders;
    [SerializeField] private bool _alert;

    private Camera _camera;

    private Vector3? _lastPosition;
    private Vector3 _stablePosition;
    private float _stableSize;

    private Vector3 _direction;

    public Rect Borders
    {
        get => _borders;
        set
        {
            _borders = value;
        }
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
        _stablePosition = transform.position;
        _stableSize = _camera.orthographicSize;
    }

    private Vector3 GetDirection()
    {
        var xyDirection = Vector2.zero;
        var zDirection = 0f;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_lastPosition.HasValue)
            {
                Vector2 delta = _lastPosition.Value - Input.mousePosition;
                xyDirection = delta * _moveSpeed;
            }

            _lastPosition = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _lastPosition = null;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            zDirection = Mathf.Clamp( Input.mouseScrollDelta.y, -1, 1);
        }

        var newDir = new Vector3(xyDirection.x, xyDirection.y, zDirection);

        var mag = newDir.sqrMagnitude;

        //newDir.Normalize();

        if (mag == 0)
        {
            _direction = Vector3.Lerp(_direction, Vector3.zero, _slide * Time.deltaTime);
        }
        else
        {
            _direction = newDir;
        }

        return _direction;

    }

    private bool IsOutOfBorders()
    {
        var swp1 = _camera.ScreenToWorldPoint(Vector3.zero);
        var swp2 = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));

        var d1 = (Vector2)swp1 - _borders.min;
        var d2 = _borders.max - (Vector2)swp2;

        _alert = d1.x < 0 || d1.y < 0 || d2.x < 0 || d2.y < 0;

        return _alert;
    }

    void Update()
    {
        if (IsMouseOverUI())
        {
            return;
        }

        var pos = _camera.transform.position;
        var size = _camera.orthographicSize;

        var dir = GetDirection();
        //dir.x *= _moveSpeed;
        //dir.y *= _moveSpeed;
        dir.z *= _zoomSpeed;

        dir *= Time.deltaTime;

        var newZ = (size + dir.z);
        dir.z = newZ < _minSize ? _minSize - size : dir.z;
        var maxSize = _borders.height / 2f - 0.01f;
        dir.z = newZ > maxSize ? maxSize - size : dir.z;

        _camera.transform.position += new Vector3(dir.x, dir.y, 0);
        _camera.orthographicSize += dir.z;

        if (IsOutOfBorders())
        {
            _direction = Vector3.zero;
            var newPos = _camera.transform.position;
            var center = new Vector3(_borders.center.x, _borders.center.y, newPos.z);
            _camera.transform.position = Vector3.Lerp(newPos, center, 0.2f);
        }

        return;

        /*
        var didChange = false;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            didChange = true;
            _lastPosition = _lastPosition ?? Input.mousePosition;
            var delta = _lastPosition.Value - Input.mousePosition;
            _camera.transform.position += delta * _moveSpeed * Time.deltaTime;
            _lastPosition = Input.mousePosition;
        }
        else
        {
            _lastPosition = null;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            didChange = true;
            _camera.orthographicSize = Mathf.Max(_minSize, _camera.orthographicSize - Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            didChange = true;
            _camera.transform.position = _stablePosition;
            _lastPosition = _stablePosition;
            _camera.orthographicSize = _stableSize;
        }

        if (_floatSpeed > 0)
        {
            didChange = true;
            _camera.orthographicSize += Mathf.Sin(Time.time * Mathf.PI * _floatSpeed) * _floatDistance * Time.deltaTime;
        }

        if (didChange)
        {
            var swp1 = _camera.ScreenToWorldPoint(Vector3.zero);
            var swp2 = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));

            var d1 = (Vector2)swp1 - _borders.min;
            var d2 = _borders.max - (Vector2)swp2;

            _alert = d1.x < 0 || d1.y < 0 || d2.x < 0 || d2.y < 0;

            if (_alert)
            {
                _camera.orthographicSize = bckpSize;
                _camera.transform.position = bckpPos;
            }
        }
        */
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current?.IsPointerOverGameObject() ?? false;
    }

    private void OnEnable()
    {
        _lastPosition = null;
    }
}
