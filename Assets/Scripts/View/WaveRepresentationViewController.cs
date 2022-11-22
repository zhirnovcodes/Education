using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveRepresentationViewController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private ValuesToMoleculeBridge _bridge;

    private void OnEnable()
    {
        _slider.value = _bridge.WaveScale;
        _slider.onValueChanged.AddListener(OnValueChaged);    
    }

    private void OnDisable()
    {
        
    }

    private void OnValueChaged(float value)
    {
        _bridge.WaveColorValue = value;
        _bridge.WaveScale = 0;
    }
}
