using UnityEngine;

public class EnvelopeViewController : MonoBehaviour
{
    [SerializeField] private EnvelopeFunction _function;
    [SerializeField] private EnvelopeView _view;

    private void OnEnable()
    {
        _view.Envelope = _function.Envelope;
        _view.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _view.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        _function.Envelope = _view.Envelope;
    }
}
