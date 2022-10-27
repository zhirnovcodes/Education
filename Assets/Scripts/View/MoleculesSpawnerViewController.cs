using UnityEngine;
using UnityEngine.UI;

public class MoleculesSpawnerViewController : MonoBehaviour
{
    [SerializeField] private MoleculesGrid _grid;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnValueChanged);
        _slider.value = _grid.Density;
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _grid.Density = _slider.value;
        _spawner.Clear();
        _spawner.Spawn();
    }
}
