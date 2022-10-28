using UnityEngine;

public class PositionsKeyboardController : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3[] _positions;
    [SerializeField] private bool _shouldReenable;

    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _index = 0;
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        var b = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _index = (_index + 1) % _positions.Length;
            b = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _index = (_index - 1) < 0 ? _positions.Length - 1 : _index - 1;
            b = true;
        }

        if (b)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        if (_shouldReenable)
        {
            _transform.gameObject.SetActive(false);
        }

        _transform.position = _positions[_index];

        if (_shouldReenable)
        {
            _transform.gameObject.SetActive(true);
        }
    }
}
