using UnityEngine;

public class Air : MonoBehaviour
{
    private static Air _instance;

    [SerializeField, Range(1,10)] private float _rigid = 1f;
    [SerializeField, Range(0.1f,5)] private float _density = 0f;
    [SerializeField, Range(0,50)] private float _timeScale = 1f;
    [SerializeField, Range(0,1)] private float _airPressure = 1f;

    public static float Rigid
    {
        get
        {
            return _instance?._rigid ?? 1;
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

    public static float AirPressure
    {
        get
        {
            return 1 / (_instance?._airPressure ?? 1);
        }
    }

    public void SetRigid( float value )
    {
        _rigid = value;
    }

    public float GetDrag( )
    {
        return _rigid;
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
