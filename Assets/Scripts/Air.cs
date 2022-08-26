using UnityEngine;

public class Air : MonoBehaviour
{
    private static Air _instance;

    [SerializeField, Range(0,1)] private float _drag = 0.1f;

    public static float Drag
    {
        get
        {
            return _instance._drag;
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
}
