using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModulatorViewController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GraphFunctionBase _function;

    private IModulator Modulator => _function as IModulator;

    private void OnEnable()
    {
        _slider.value = Modulator.ModulationValue;
        _slider.onValueChanged.AddListener(OnValueChaged);    
    }

    private void OnDisable()
    {
        
    }

    private void OnValueChaged(float value)
    {
        Modulator.ModulationValue = value;
    }
}
