using UnityEngine;
using UnityEngine.UI;

public class AirViewController : MonoBehaviour
{
    [SerializeField] private Slider _dragSlider;
    [SerializeField] private Air _air;

    private void OnEnable()
    {
        _dragSlider.value = _air.GetDrag();
        _dragSlider.onValueChanged.AddListener(ValueChanged);
    }

    private void OnDisable()
    {
        _dragSlider.onValueChanged.RemoveListener(ValueChanged);
    }

    private void ValueChanged( float value )
    {
        _air.SetRigid( value );
    }

}
