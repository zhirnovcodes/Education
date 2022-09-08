using System;
using UnityEngine;

public class GridDrawer : MonoBehaviour, IDisposable, IGraphDrawer
{
    [SerializeField] private Vector2Int _cellSize = new Vector2Int(10, 10);
    [SerializeField] private Color _gridColor = new Color(1, 1, 0, 1);
    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0.1f);

    [SerializeField] private int _textureWidth = GraphDrawer.DefaultTextureWidth;
    [SerializeField] private int _textureHeight = GraphDrawer.DefaultTextureHeight;

    private const string GridColName = "GridCol";
    private const string CellSizeName = "CellSize";
    private const string ResultName = "Result";
    private const string BckgColName = "BckgCol";

    private const string ShaderName = "GridDrawer";

    private ComputeShader _shader;

    private int _paintGridKernelIndex;

    private RenderTexture _texture;

    public RenderTexture Texture 
    { 
        get
        {
            if (_texture == null)
            {
                CreateShader();
            }
            return _texture;
        }
    }

    public void Dispose()
    {
        if (_texture != null)
        {
            _texture.Release();
        }

        if (_shader != null)
        {
            UnityEngine.Object.Destroy(_shader);
        }
    }

    void Start()
    {
        if (_shader == null)
        {
            CreateShader();
        }
        PaintGrid(_gridColor, _cellSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateShader()
    {
        _shader = (ComputeShader)GameObject.Instantiate(Resources.Load(ShaderName));
        _paintGridKernelIndex = _shader.FindKernel("PaintGrid");

        _texture = new RenderTexture(_textureWidth, _textureHeight, 1);
        _texture.filterMode = FilterMode.Point;
        _texture.enableRandomWrite = true;
        _texture.Create();

        _shader.SetTexture(_paintGridKernelIndex, ResultName, _texture);
    }

    private void PaintGrid(Color gridColor, Vector2Int cellSize)
    {
        _shader.SetVector(BckgColName, _bckgColor);
        _shader.SetVector(GridColName, gridColor);
        _shader.SetVector(CellSizeName, (Vector2)cellSize);
        _shader.Dispatch(_paintGridKernelIndex, Texture.width / 8, Texture.height / 8, 1);
    }
}
