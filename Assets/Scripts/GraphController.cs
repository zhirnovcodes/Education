using UnityEngine;

public class GraphController : MonoBehaviour
{
    private static GraphController _instance;

    [SerializeField] private float _maxOffset = 2f;
    [SerializeField] private float _multiplier = 1.5f;

    public static float? MaxOffset
    {
        get
        {
            return _instance?._maxOffset;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _maxOffset *= _multiplier;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _maxOffset /= _multiplier;
        }
    }
}
