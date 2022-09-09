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
    [SerializeField] private GameObject _quadPrefab;
    [SerializeField] private Vector2Int _cellsCount = new Vector2Int(16, 16);

    private Grid _grid;
    private Texture2D _bufferTex;
    private List<float> _numbers = new List<float>();
    private GridDrawer _gridDrawer;

    public List<float> Numbers => _numbers;
    public int MaxHeight => _cellsCount.y;
    public IEnumerable<Vector3> LeftColumn()
    {
        Init();
        for (int y = -_cellsCount.y/2; y < _cellsCount.y/2; y++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(-_cellsCount.x / 2 - 1, y, 0));
        }
    }

    public IEnumerable<Vector3> BottomColumn()
    {
        Init();
        for (int x = -_cellsCount.x / 2; x < _cellsCount.x / 2; x++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(x, -_cellsCount.y / 2 - 1, 0));
        }
    }

    public void DrawIndicies()
    {
        Init();

        int index = 0;
        foreach (var pos in LeftColumn())
        {
            var t = GameObject.Instantiate(_textPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();
            t.text = index.ToString();
            t.transform.position = pos;
            index++;
        }
    }

    public void DrawGrid()
    {
        Init();

        var texScale = _graph.transform.localScale;
        var cellSizePixels = new Vector2Int((int)(_graph.Texture.width * _grid.cellSize.x / texScale.x),
            (int)(_graph.Texture.height * _grid.cellSize.y / texScale.y));
        _gridDrawer.PaintGrid(cellSizePixels, _graph.Texture);
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
        foreach (var pos in BottomColumn())
        {
            var valueX = (index + 0.5f) * valTex.width / _cellsCount.x;
            var t = GameObject.Instantiate(_textPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();
            var value = _bufferTex.GetPixel((int)valueX, 0).r;

            float c = _cellsCount.y;
            var maxOffset = _graph.MaxOffset;
            maxOffset = GraphController.MaxOffset ?? maxOffset;
            value = (Mathf.Clamp(value, -maxOffset, maxOffset) / maxOffset + 1) / 2f * c;

            var valueInt = Mathf.RoundToInt(value);

            _numbers.Add(valueInt);

            t.text = valueInt.ToString() + ";";
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

        _grid.cellSize = new Vector3(texScale.x / _cellsCount.x, texScale.y / _cellsCount.y, _grid.cellSize.z);
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

        var cell = new Vector3Int(x - _cellsCount.x / 2, value - _cellsCount.y / 2, 0);
        var pos = _grid.GetCellCenterWorld(cell);
        q.transform.position = pos;
    }
}


/*float ValueToUV(float x){
float c = (ResultSize.y - 1);
float val = (clamp(Values[float2(x, 0)].r, -MaxOffset, MaxOffset) / MaxOffset + 1) / 2 * c;
    return val;
}
*/