using UnityEngine;

public class Air : MonoBehaviour
{
    private static Air _instance;

    [SerializeField, Range(0,1)] private float _drag = 0.1f;
    [SerializeField, Range(0.1f,5)] private float _density = 0f;
    [SerializeField, Range(0,50)] private float _timeScale = 1f;

    public static float Drag
    {
        get
        {
            return _instance?._drag ?? 1;
        }
    }
    public static float Time
    {
        get
        {
            return UnityEngine.Time.time;
        }
    }

    public static float Density
    {
        get
        {
            return _instance?._density ?? 1;
        }
    }

    public void SetDrag( float value )
    {
        _drag = value;
    }

    public float GetDrag( )
    {
        return _drag;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        UnityEngine.Time.timeScale = _timeScale;
    }
}
