using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _zoomSpeed = 100f;
    [SerializeField] private float _yClickZoneMin = -Mathf.Infinity;
    [SerializeField] private float _yClickZoneMax = Mathf.Infinity;
    [SerializeField] private RectTransform _clickZone;

    private Camera _camera;

    private Vector3? _lastPosition;
    private Vector3 _stablePosition;
    private float _stableSize;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _stablePosition = transform.position;
        _stableSize = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (_yClickZoneMin <= Input.mousePosition.y && Input.mousePosition.y < _yClickZoneMax)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
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
                _camera.orthographicSize = Mathf.Max(0, _camera.orthographicSize - Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                _camera.transform.position = _stablePosition;
                _lastPosition = _stablePosition;
                _camera.orthographicSize = _stableSize;
            }
        }

    }
}
