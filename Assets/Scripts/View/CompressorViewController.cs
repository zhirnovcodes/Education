using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompressorViewController : MonoBehaviour
{
    [SerializeField] private GainReductionFunction _gr;
    [SerializeField] private ScalarOperationFunction _makeupFunction;
    [SerializeField] private CompressorView _view;

    private IThresholdFunction[] _thresholdFunctions;

    private float _threshBefore = -float.MaxValue;

    private void Start()
    {
        _thresholdFunctions = UnityEngine.Object.FindObjectsOfType<GraphFunctionBase>().
            Where(t => t is IThresholdFunction).
            Select(t => t as IThresholdFunction).ToArray();


        _threshBefore = -float.MaxValue;

        _view.Attack = _gr.Attack;
        _view.Release = _gr.Release;
        _view.Ratio = _gr.Ratio;
        _view.Makeup = _makeupFunction.Scalar;
    }

    // Update is called once per frame
    void Update()
    {
        if (_threshBefore != _view.Threshold)
        {
            _threshBefore = _view.Threshold;
            UpdateThreshold();
        }

        _gr.Attack = _view.Attack;
        _gr.Release = _view.Release;
        _gr.Ratio = _view.Ratio;

        _makeupFunction.Scalar = _view.Makeup;
    }

    private void UpdateThreshold()
    {
        foreach (var th in _thresholdFunctions)
        {
            th.Threshold = _view.Threshold;
        }
    }


}
