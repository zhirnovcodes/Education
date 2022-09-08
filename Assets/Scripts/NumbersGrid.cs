using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumbersGrid : MonoBehaviour, IDisposable
{
    [SerializeField] private PositionOffsetDrawer _drawer;
    [SerializeField] private Transform _canvas;
    [SerializeField] private TextMeshProUGUI _textPrefab;

    private Grid _grid;
    private Vector2Int _size;
    private Texture2D _bufferTex;

    public Vector2Int GridSize
    {
        get
        {
            Init();
            return _size;
        }
    }

    public IEnumerable<Vector3> LeftColumn()
    {
        for (int y = -GridSize.y/2; y < GridSize.y/2; y++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(-_size.x / 2 - 1, y, 0));
        }
    }
    public IEnumerable<Vector3> BottomColumn()
    {
        for (int x = -GridSize.x / 2; x < GridSize.x / 2; x++)
        {
            yield return _grid.GetCellCenterWorld(new Vector3Int(x, -_size.y / 2 - 1, 0));
        }
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Init()
    {
        if (_grid != null)
        {
            return;
        }

        _grid = GetComponent<Grid>();
        var texture = _drawer.Texture;
        var texScale = _drawer.transform.localScale;

        _grid.cellSize = new Vector3(texScale.x / texture.width, texScale.y / texture.height, _grid.cellSize.z);
        transform.position = _drawer.transform.position;

        _size = new Vector2Int( _drawer.Texture.width, _drawer.Texture.height);

        int index = 0;

        _canvas.position = _grid.transform.position;

        foreach (var pos in LeftColumn())
        {
            var t = GameObject.Instantiate(_textPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();
            t.text = index.ToString();
            t.transform.position = pos;
            index++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var valTex = _drawer.Values;

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
                var t = GameObject.Instantiate(_textPrefab.gameObject, _canvas).GetComponent<TextMeshProUGUI>();
                var value = _bufferTex.GetPixel(index, 0).r;

                float c = _drawer.Texture.height;
                var maxOffset = _drawer.MaxOffset;
                maxOffset = GraphController.MaxOffset ?? maxOffset;
                value = (Mathf.Clamp(value, -maxOffset, maxOffset) / maxOffset + 1) / 2f * c;

                t.text = Mathf.RoundToInt(value).ToString() + ";";
                t.transform.position = pos;
                index++;
            }
        }
    }

    public void Dispose()
    {
        if (_bufferTex != null)
        {
            UnityEngine.Object.Destroy(_bufferTex);
        }
    }
}


/*float ValueToUV(float x){
float c = (ResultSize.y - 1);
float val = (clamp(Values[float2(x, 0)].r, -MaxOffset, MaxOffset) / MaxOffset + 1) / 2 * c;
    return val;
}
*/