using UnityEngine;
using UnityEngine.UI;

public class CompressorView : MonoBehaviour
{
    [SerializeField] private Slider _t;
    [SerializeField] private Slider _r;
    [SerializeField] private Slider _a;
    [SerializeField] private Slider _rel;
    [SerializeField] private Slider _make;

    public float Threshold 
    { 
        get
        {
            return _t.value;
        }
        set
        {
            _t.value = value;
        }
    }
    public float Ratio
    {
        get
        {
            return _r.value;
        }
        set
        {
            _r.value = value;
        }
    }

    public float Attack
    {
        get
        {
            return _a.value;
        }
        set
        {
            _a.value = value;
        }
    }

    public float Release
    {
        get
        {
            return _rel.value;
        }
        set
        {
            _rel.value = value;
        }
    }

    public float Makeup
    {
        get
        {
            return _make.value;
        }
        set
        {
            _make.value = value;
        }
    }
}
