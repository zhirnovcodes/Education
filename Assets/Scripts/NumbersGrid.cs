using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumbersGrid : MonoBehaviour, IDisposable
{
    [SerializeField] private PositionOffsetDrawer _graph;
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _quadsParent;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private TextMeshProUGUI _valuesPrefab;
    [SerializeField] private GameObject _quadPrefab;
    [SerializeField] private FreqDiscr _freqDiscr = FreqDiscr.F44000;
    [SerializeField] private BitDepth _bitDepth = BitDepth.B4;
    [SerializeField] private float _time = 8f;

    private Vector2Int? _cellsCount;
    private bool _isFloat;
    private float _maxNum;

    private Vector2Int CellsCount
    {
        get
        {
            if (_cellsCount == null)
            {
                var widthMul = 0f;

                switch (_freqDiscr)
                {
                    case FreqDiscr.F11000:
                        widthMul = 2f;
                        break;
                    case FreqDiscr.F22000:
                        widthMul = 1f;
                        break;
                    case FreqDiscr.F44000:
                        widthMul = 0.5f;
                        break;
                    case FreqDiscr.F88000:
                        widthMul = 0.25f;
                        break;
                }


                var y = 0;
                _isFloat = false;

                switch (_bitDepth)
                {
                    case BitDepth.B1:
                        _maxNum = 1;
                        y = 2;
                        break;
                    case BitDepth.B4:
                        _maxNum = 15;
                        y = 16;
                        break;
                    case BitDepth.B8:
                        _maxNum = 255;
                        y = 16;
                        break;
                    case BitDepth.B32:
                        _maxNum = 1;
                        y = 16;
                        _isFloat = true;
                        break;
                }

                _cellsCount = new Vector2Int((int)(_time / widthMul), y);
            }
            return _cellsCount.Value;
        }
    }

    private Grid _grid;
    private Texture2D _bufferTex;
    private List<float> _numbers = new List<float>();
    private GridDrawer _gridDrawer;

    public List<float> Numbers => _numbers;
    public float Height => _maxNum;
    public IEnumerable<Vector3> LeftColumn()
    {
        Init();
        for (int y = -CellsCount.y/2; y < CellsCount.y/2; y++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(-CellsCount.x / 2 - 1, y, 0));
        }
    }

    public IEnumerable<Vector3> BottomColumn()
    {
        Init();
        for (int x = -CellsCount.x / 2; x < CellsCount.x / 2; x++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(x, -CellsCount.y / 2 - 1, 0));
        }
    }

    public void DrawIndicies()
    {
        Init();

        int index = 0;
        var height = CellsCount.y;
        var step = (_maxNum + (_isFloat ? 0 : 1)) / height;

        foreach (var pos in LeftColumn())
        {
            var t = GameObject.Instantiate(_textPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();

            var v = step * index;
            v = _isFloat ? (float)Math.Round(v, 3) : (int)v;

            t.text = v.ToString();
            t.transform.position = pos;
            index++;
        }
    }

    public void DrawGrid(bool paintRows = true, bool paintCols = true)
    {
        Init();

        //var cellSizePixels = new Vector2Int((int)(_graph.Texture.width * _grid.cellSize.x / texScale.x),
    //(int)(_graph.Texture.height * _grid.cellSize.y / texScale.y));

        var texScale = _graph.transform.localScale;

        var width = (int)(_graph.Texture.width * ( paintRows ? _grid.cellSize.x / texScale.x : 1));
        var height = (int)(_graph.Texture.height * (paintCols ? _grid.cellSize.y / texScale.y : 1));
        var cellSizePixels = new Vector2Int(width, height);

        _gridDrawer.PaintGrid(cellSizePixels, _graph.Texture);
    }

    public void HideValues()
    {
        _canvas.gameObject.SetActive(false);
        _quadsParent.gameObject.SetActive(false);
    }

    public void DrawValues()
    {
        Init();

        var valTex = _graph.Values;

        if (_bufferTex == null)
        {
            _bufferTex = new Texture2D(valTex.width, valTex.height, TextureFormat.RFloat, false);

        }

        RenderTexture.active = valTex;
        _bufferTex.ReadPixels(new Rect(0, 0, valTex.width, valTex.height), 0, 0);
        _bufferTex.Apply();

        int index = 0;
        var height = _isFloat ? _maxNum : (_maxNum + 1);
        foreach (var pos in BottomColumn())
        {
            var valueX = (index + 0.5f) * valTex.width / CellsCount.x;
            var t = GameObject.Instantiate(_valuesPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();
            var value = _bufferTex.GetPixel((int)valueX, 0).r;

            float c = CellsCount.y;
            var maxOffset = _graph.MaxOffset;
            maxOffset = GraphController.MaxOffset ?? maxOffset;
            value = (Mathf.Clamp(value, -maxOffset, maxOffset) / maxOffset + 1) / 2f;

            var valueFloat = value * height;
            valueFloat = _isFloat ? (float)Math.Round(valueFloat, 2) : Mathf.RoundToInt(valueFloat);
            var valueInt = Mathf.RoundToInt(value * c);


            _numbers.Add(valueFloat);

            t.text = valueFloat.ToString();
            t.transform.position = pos;

            SpawnQuad(index, valueInt);

            index++;
        }

    }

    // Update is called once per frame
    private void Init()
    {
        if (_grid != null)
        {
            return;
        }

        _gridDrawer = GetComponent<GridDrawer>();

        _grid = GetComponent<Grid>();
        var texScale = _graph.transform.localScale;

        _grid.cellSize = new Vector3(texScale.x / CellsCount.x, texScale.y / CellsCount.y, _grid.cellSize.z);
        transform.position = _graph.transform.position;

        _canvas.position = _grid.transform.position;
    }

    private int _index = 0;

    public void Dispose()
    {
        if (_bufferTex != null)
        {
            UnityEngine.Object.Destroy(_bufferTex);
        }
    }

    private void SpawnQuad(int x, int value)
    {
        var q = GameObject.Instantiate(_quadPrefab, _quadsParent);

        q.transform.localScale = _grid.cellSize;

        var cell = new Vector3Int(x - CellsCount.x / 2, value - CellsCount.y / 2, 0);
        var pos = _grid.GetCellCenterWorld(cell);
        q.transform.position = pos;
    }

    public enum FreqDiscr
    {
        F11000 = 0,
        F22000 = 1,
        F44000 = 2,
        F88000 = 3
    }

    public enum BitDepth
    {
        B1 = 0,
        B4 = 1,
        B8 = 2,
        B24 = 3,
        B32 = 4
    }
}


/*float ValueToUV(float x){
float c = (ResultSize.y - 1);
float val = (clamp(Values[float2(x, 0)].r, -MaxOffset, MaxOffset) / MaxOffset + 1) / 2 * c;
    return val;
}
*/