using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalToAnalogueGraphDrawer : MonoBehaviour, IGraphDrawer
{
    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;

    [SerializeField] private NumbersGrid _numbers;
    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color _linesColor = new Color(1, 1, 1, 1);

    [SerializeField] private int _textureWidth = GraphDrawer.DefaultTextureWidth;
    [SerializeField] private int _textureHeight = GraphDrawer.DefaultTextureHeight;

    private GraphDrawer _drawer;


    public RenderTexture Texture
    {
        get
        {
            CreateDrawer();
            return _drawer.Texture;
        }
    }

    private void CreateDrawer()
    {
        _drawer = _drawer ?? new GraphDrawer(_textureWidth, _textureHeight);
    }

    private void OnEnable()
    {
        CreateDrawer();
        _drawer.Clear();
    }

    public void Fill()
    {
        CreateDrawer();

        var numbers = _numbers.Numbers;
        var height = _numbers.Height;

        var cellWidth = (Texture.width / numbers.Count);

        _maxOffset = GraphController.MaxOffset ?? _maxOffset;

        for (int i = 0; i < Texture.width - cellWidth; i++)
        {
            var numberIndex = i / cellWidth;
            var number0 = numbers[numberIndex];
            var number1 = numbers[numberIndex + 1];
            var t = (i % cellWidth) / (float)cellWidth;

            var val = Mathf.Lerp(number0, number1, t);
            val = (val / (height) * 2f - 1f) * _maxOffset;

            _drawer.AddValue(i, val);
        }

        _drawer.Paint(_bckgColor, _linesColor, _maxOffset);
    }

    public void Dispose()
    {
        _drawer.Dispose();
    }
}
