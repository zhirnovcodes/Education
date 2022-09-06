using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StringView : MonoBehaviour
{
    [SerializeField] private Slider _freqS;
    [SerializeField] private Slider _ampS;
    [SerializeField] private Slider _perS;
    [SerializeField] private Slider _timeS;

    [SerializeField] private TMP_InputField _freqI;
    [SerializeField] private TMP_InputField _ampI;
    [SerializeField] private TMP_InputField _perI;

    public event Action<float> OnFreqChanged;
    public event Action<float> OnAmpChanged;
    public event Action<float> OnTimeChanged;

    private bool _shouldChangeSlider = true;

    public float Freq
    {
        get
        {
            return _freqS.value;
        }
        set
        {
            _freqI.text = (value * 1000f).ToString();
            _freqS.value = value;
        }
    }

    public float Amp
    {
        get
        {
            return _ampS.value;
        }
        set
        {
            _ampI.text = value.ToString();
            _ampS.value = value;
        }
    }

    public float Time
    {
        get
        {
            return _timeS.value;
        }
        set
        {
            _timeS.value = value;
        }
    }

    private void OnEnable()
    {
        _freqS.onValueChanged.AddListener(FreqSliderValueChanged);
        //_freqI.onValueChanged.AddListener(FreqInputValueChanged);
        _freqI.onSubmit.AddListener( FreqInputValueChanged );

        _ampS.onValueChanged.AddListener(AmpSliderValueChanged);
        _ampI.onValueChanged.AddListener(AmpInputValueChanged);

        _timeS.onValueChanged.AddListener(TimeSliderValueChanged);
    }

    private void OnDisable()
    {
        _freqS.onValueChanged.RemoveListener(FreqSliderValueChanged);
        _freqI.onSubmit.RemoveListener(FreqInputValueChanged);

        _ampS.onValueChanged.RemoveListener(AmpSliderValueChanged);
        _ampI.onValueChanged.RemoveListener(AmpInputValueChanged);

        _timeS.onValueChanged.RemoveListener(TimeSliderValueChanged);
    }

    private void FreqSliderValueChanged(float value)
    {
        OnFreqChanged?.Invoke(value);

        _shouldChangeSlider = false;
        _freqI.text = (value * 1000f).ToString();
        _shouldChangeSlider = true;

        if (_perS != null)
        {
            _perS.value = 1f / (value);
        }
        if (_perI != null)
        {
            _perI.text = (1f / (value * 1000f)).ToString();
        }

    }

    private void FreqInputValueChanged(string value)
    {
        if (float.TryParse(value, out var val))
        {
            val /= 1000f;
            if (_shouldChangeSlider)
            { 
                _freqS.value = val; 
            }
        }
    }


    private void AmpSliderValueChanged(float value)
    {
        OnAmpChanged?.Invoke(value);

        _shouldChangeSlider = false;
        _ampI.text = value.ToString();
        _shouldChangeSlider = true;

    }

    private void AmpInputValueChanged(string value)
    {
        if (float.TryParse(value, out var val))
        {
            if (_shouldChangeSlider)
            {
                _ampS.value = val;
            }
        }
    }

    private void TimeSliderValueChanged(float value)
    {
        OnTimeChanged?.Invoke(value);
    }
}
