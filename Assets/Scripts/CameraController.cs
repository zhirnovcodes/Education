using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _zoomSpeed = 100f;
    [SerializeField] private float _minSize = 1f;
    [SerializeField] private Rect _borders;
    [SerializeField] private bool _alert;

    private Camera _camera;

    private Vector3? _lastPosition;
    private Vector3 _stablePosition;
    private float _stableSize;

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

    void Update()
    {
        if (IsMouseOverUI())
        {
            return;
        }

        var bckpPos = _camera.transform.position;
        var bckpSize = _camera.orthographicSize;

        var didChange = false;

        //if (_yClickZoneMin <= Input.mousePosition.y && Input.mousePosition.y < _yClickZoneMax)
        {
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
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current?.IsPointerOverGameObject() ?? false;
    }
}
