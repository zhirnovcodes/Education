using UnityEngine;

public class StringViewController : MonoBehaviour
{
    [SerializeField] private StringView _view;
    [SerializeField] private FluctuatingString1D _string;

    public FluctuatingString1D String 
    { 
        set 
        {
            _string = value;

            if (_string == null)
            {
                return;
            }

            _view.Freq = _string.Fluctuation.Frequency;
            _view.Amp = _string.Fluctuation.Amplitude;
            _view.Time = _string.Fluctuation.Time;
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
            _view.Freq = _string.Fluctuation.Frequency;
            _view.Amp = _string.Fluctuation.Amplitude;
            _view.Time = _string.Fluctuation.Time;
        }

        _view.OnFreqChanged += FreqValueChanged;
        _view.OnAmpChanged += AmpValueChanged;
        _view.OnTimeChanged += TimeValueChanged;
    }

    private void OnDisable()
    {
        _view.OnFreqChanged -= FreqValueChanged;
        _view.OnAmpChanged -= AmpValueChanged;
        _view.OnTimeChanged -= TimeValueChanged;
    }

    private void FreqValueChanged(float value)
    {
        _string.SetFrq(value);
    }

    private void AmpValueChanged(float value)
    {
        _string.SetAmp(value);
    }

    private void TimeValueChanged(float value)
    {
        _string.SetLen( value );
    }
}
