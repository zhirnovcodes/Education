using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AmplitudeData
{
    public GraphFunctionBase AmplitudeFunction;
    public float Frequency;
}

public class EqDrawerBase : MonoBehaviour
{
    [SerializeField] private List<AmplitudeData> _data = new List<AmplitudeData>();
    [SerializeField] private int _bands = 128;
    [SerializeField] private float _minFreq = 0;
    [SerializeField] private float _maxFreq = 10000;

    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color _linesColor = new Color(1, 1, 1, 1);

    private GraphDrawer _drawer;

    public RenderTexture Texture => Drawer.Texture;

    private GraphDrawer Drawer
    {
        get
        {
            if (_drawer == null)
            {
                _drawer = new GraphDrawer(_bands, 128);
            }
            return _drawer;
        }
    }

    void OnEnable()
    {
        Drawer.Clear();
    }

    void Update()
    {
        foreach (var d in _data)
        {
            SetValue(d.Frequency, d.AmplitudeFunction.Value - 3);
        }

        Drawer.Paint(_bckgColor, _linesColor, 3, GraphDrawer.DrawType.FillColorBckg);
    }

    void SetValue(float freq, float amp)
    {
        var band = FreqToId(freq);

        if (band < 0 || band >= _bands)
        {
            return;
        }

        Drawer.AddValue(band, amp);
    }

    private int FreqToId(float freq)
    {
        var bandWidth = (_maxFreq - _minFreq) / _bands;
        var band = (freq - _minFreq) / bandWidth;
        return Mathf.RoundToInt(band);
    }
}
