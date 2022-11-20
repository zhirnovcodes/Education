using UnityEngine;

public class FunctionBuffer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _maxTime = 4;
    [SerializeField] private float _deltaTime = 0.05f;
    [SerializeField] private bool _shouldClearOnEnable = true;

    private IndexedQueue<float> _queue;

    private float _value;
    private float _timeStart;

    private PseudoTimer _timer;

    public void InitializeWithTexture(RenderTexture values)
    {
        var tex = new Texture2D(values.width, 1, TextureFormat.R16, false);
        var bckp = RenderTexture.active;
        RenderTexture.active = values;
        tex.ReadPixels(new Rect(0, 0, values.width, 1), 0, 0);
        tex.Apply();
        RenderTexture.active = bckp;

        _queue = new IndexedQueue<float>(values.width);

        for (int i = 0; i < values.width; i++)
        {
            var pix = tex.GetPixel(i, 0).r;
            if (pix <= -99999)
            {
                break;
            }
            _queue.Add(pix);
        }

        DestroyImmediate(tex);
    }

    public float GetValue(float time)
    {
        if (_queue == null)
        {
            return 0;
        }

        var index = FindIndex(time);

        if (index < 0 || _queue.Count <= 0)
        {
            return 0;
        }

        index = Mathf.Min(index, _queue.Count - 1);

        var nextIndex = Mathf.Min(index + 1, _queue.Count - 1);

        var v1 = _queue[index];
        var v2 = _queue[nextIndex];

        var t1 = _timeStart + _deltaTime * index;
        var t2 = _timeStart + _deltaTime * (index + 1);

        var foundValue = Mathf.Lerp(v1, v2, Mathf.InverseLerp(t1, t2, time));

        return foundValue;
    }

    private void OnEnable()
    {
        if (_queue == null)
        {
            var count = Mathf.CeilToInt(_maxTime / _deltaTime);
            _queue = new IndexedQueue<float>(count);
        }

        if (_timer == null)
        {
            _timer = new PseudoTimer(this);
            _timer.Tick += OnTimerTick;

        }

        if (_shouldClearOnEnable)
        { 
            _queue.Clear(); 
        }
        _timer.Start(_deltaTime);
        _timeStart = Time.time;
        _value = 0;
    }

    private void OnTimerTick(float time)
    {
        if (_function == null)
        {
            return;
        }
        if (_queue.Add(_value))
        {
            _timeStart += _deltaTime;
        }
    }

    private void OnDisable()
    {
        _timer.Stop();
    }

    private int FindIndex(float time)
    {
        return Mathf.FloorToInt( (time - _timeStart) / _deltaTime );
    }

    void Update()
    {
        _value = _function?.Value ?? 0;
    }
}
