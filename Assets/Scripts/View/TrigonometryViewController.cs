using UnityEngine;

public class TrigonometryViewController : MonoBehaviour
{
    [SerializeField] private TrigonometryView _view;
    [SerializeField] private TrigonometryFunction _function;

    private void OnEnable()
    {
        _view.Function = _function.Function;
        _view.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _view.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        _function.Function = _view.Function;
    }

}
