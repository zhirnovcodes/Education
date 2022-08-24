using UnityEngine;
using UnityEngine.UI;

public class MoleculesCountViewController : MonoBehaviour
{
    [SerializeField] private Workspace _workspace;
    [SerializeField] private Slider _slider;

    private void Start()
    {
        OnValueChanged(_slider.value);
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _workspace.MoleculesDensity = value;
    }

}
