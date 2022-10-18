using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphController : MonoBehaviour
{
    private static GraphController _instance;

    [SerializeField] private float _maxOffset = 2f;
    [SerializeField] private float _multiplier = 1.5f;
    private float _maxBefore = -float.MaxValue;
    private GraphDrawerBase[] _drawers;

    public static float? MaxOffset
    {
        get
        {
            return _instance?._maxOffset;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        if (_maxBefore != _maxOffset)
        {
            _maxBefore = _maxOffset;
            UpdateMaxValue();
        }
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        _drawers = UnityEngine.Object.FindObjectsOfType<GraphDrawerBase>();

        _maxBefore = -float.MaxValue;
    }

    private void UpdateMaxValue()
    {
        if (_drawers == null)
        {
            return;
        }

        foreach (var d in _drawers)
        {
            d.MaxOffset = _maxOffset;
        }
    }
}
