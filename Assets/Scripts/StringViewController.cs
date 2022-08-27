using UnityEngine;
using UnityEngine.UI;

public class StringViewController : MonoBehaviour
{
    [SerializeField] private Slider _freq;
    [SerializeField] private Slider _amp;
    [SerializeField] private Slider _dec;
    [SerializeField] private String1D _string;

    public String1D String 
    { 
        set 
        {
            _string = value;

            if (String == null)
            {
                return;
            }

            FreqValueChanged(_freq.value);
            AmpValueChanged(_amp.value);
            DecValueChanged(_dec.value);
        }
        private get 
        {
            return _string;
        } 
    }

    private void OnEnable()
    {
        if (String != null)
        {
            _freq.value = _string.Frequency;
            _amp.value = _string.Amplitude;
            _dec.value = _string.DecayTime;
        }

        _freq.onValueChanged.AddListener( FreqValueChanged );
        _amp.onValueChanged.AddListener( AmpValueChanged);
        _dec.onValueChanged.AddListener( DecValueChanged );
    }

    private void OnDisable()
    {
        _freq.onValueChanged.RemoveListener(FreqValueChanged);
        _amp.onValueChanged.RemoveListener(AmpValueChanged);
        _dec.onValueChanged.RemoveListener(DecValueChanged);
    }

    private void FreqValueChanged(float value)
    {
        _string.Frequency = value;
    }

    private void AmpValueChanged(float value)
    {
        _string.Amplitude = value;
    }

    private void DecValueChanged(float value)
    {
        _string.DecayTime = value;
    }
}
