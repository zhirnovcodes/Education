using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpViewController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private LerpFunction _function;
    [SerializeField] private GraphDrawerBase _graph1;
    [SerializeField] private GraphDrawerBase _graph2;
    [SerializeField] private GraphDrawerBase _graphLerp;

    private void OnEnable()
    {
        _slider.value = _function.LerpValue;
        SetColor(_slider.value);
        _slider.onValueChanged.AddListener(OnValueChaged);
    }

    private void OnDisable()
    {

    }

    private void OnValueChaged(float value)
    {
        _function.LerpValue = value;
        SetColor(value);
    }

    private void SetColor(float value)
    {
        if (_graph1 == null || _graph2 == null || _graphLerp == null)
        {
            return;
        }

        var c1 = _graph1.LinesColor;
        var c2 = _graph2.LinesColor;
        _graphLerp.LinesColor = Color.Lerp(c1, c2, value);
    }
}
