using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecialStrings : MonoBehaviour
{
    [SerializeField] private GameObject _stringPrefab;
    [SerializeField] private GameObject _graphPrefab;
    [SerializeField] private GameObject _ear;
    [SerializeField] private Transform _leftAnchor;
    [SerializeField] private float _yOffset;
    [SerializeField] private int _count = 16;
    [SerializeField] private float _minFreq = 1;
    [SerializeField] private float _maxFreq = 2;
    [SerializeField] private float _amplitude = 1f;
    [SerializeField] private WaveType _wt = WaveType.Saw;


    void Start()
    {
        //SpawnSquare(_count, _minFreq);
        switch (_wt)
        {
            case WaveType.Saw:
            {
                SpawnSaw(_count, _minFreq);
                break;
            }
            case WaveType.Square:
            {
                SpawnSquare(_count, _minFreq);
                break;
            }
            case WaveType.Noise:
            {
                SpawnNoise(_count, _minFreq, 0.03f);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSaw(int count, float freqStart)
    {
        var height = _yOffset * (count - 1);

        var y = _leftAnchor.position.y;
        var offset = (_yOffset + _stringPrefab.transform.localScale.y);

        ClearEar();

        var y0 = y + height / 2;

        for (int i = 0; i < count; i++)
        {
            var freq = freqStart * (i + 1);
            //var freq = (i + 1) / 4f / 2f;
            var amp = (1f / (freq + 0.5f)) / 2f + 1;
            amp *= _amplitude;

            SpawnString(freq, amp, y0);

            y0 -= offset;
        }
    }

    public void SpawnSquare(int count, float freqStart)
    {
        var height = _yOffset * (count - 1);

        var y = _leftAnchor.position.y;
        var offset = (_yOffset + _stringPrefab.transform.localScale.y);

        ClearEar();

        var y0 = y + height / 2;

        for (int i = 0; i < count; i++)
        {
            var freq = freqStart * (i + 1);
            var amp = 1f / (freq + 0.125f) * Mathf.Abs(Mathf.Sin(2 * freq * Mathf.PI));
            amp *= _amplitude;

            SpawnString(freq, amp, y0);

            y0 -= offset;
        }
    }

    public void SpawnTriangle(int count, float maxFreq)
    {
        var height = _yOffset * (count - 1);

        var y = _leftAnchor.position.y;
        var offset = (_yOffset + _stringPrefab.transform.localScale.y);

        ClearEar();

        var y0 = y + height / 2;

        for (int i = 0; i < count; i++)
        {
            var freq = i * maxFreq / count + 0.5f * maxFreq / 2 / count;
            //var freq = (i + 1) / 4f / 2f;
            var amp = 1 - freq / _maxFreq + 1;
            amp *= _amplitude;

            SpawnString(freq, amp, y0);

            y0 -= offset;
        }
    }

    private void ClearEar()
    {
        var earComps = _ear.GetComponents<FluctuatingMolecule1D>();
        for (int i = 0; i < earComps.Length; i++)
        {
            Destroy(earComps[i]);
        }
    }

    public void SpawnNoise(int count, float minFreq, float offsetF)
    {
        var height = _yOffset * (count - 1);

        var y = _leftAnchor.position.y;
        var offsetY = (_yOffset + _stringPrefab.transform.localScale.y);

        ClearEar();

        var y0 = y + height / 2;

        for (int i = 0; i < count; i++)
        {
            var freq = minFreq + (i * (offsetF + Random.Range(0,1f)));
            var amp = 1f;
            amp *= _amplitude;

            SpawnString(freq, amp, y0);

            y0 -= offsetY;
        }
    }

    private void SpawnString(float freq, float amp, float y)
    {
        var x = _leftAnchor.position.x;

        var str = GameObject.Instantiate(_stringPrefab);
        var strCom = str.GetComponent<FluctuatingString1D>();

        strCom.SetFrq(freq);
        strCom.SetAmp(amp);
        strCom.SetLen(100);
        strCom.SetAtt(0);


        str.gameObject.SetActive(false);
        str.transform.position = new Vector2(x, y);
        str.gameObject.SetActive(true);

        var earComp = _ear.AddComponent<FluctuatingMolecule1D>();
        //earComp.Source = strCom;
        //earComp.WithDelay = false;

        var graph = GameObject.Instantiate(_graphPrefab);
        var graphCom = graph.GetComponentInChildren<PositionOffsetDrawer>();
        graph.transform.position = str.transform.position - new Vector3(2, 0) - new Vector3(_graphPrefab.transform.localScale.y, 0);

        graphCom.Target = str.transform;

    }

    private enum WaveType
    {
        Square,
        Saw,
        Noise
    }
}
