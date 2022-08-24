using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringCountViewController : MonoBehaviour
{
    [SerializeField] private Workspace _workspace;
    [SerializeField] private StringViewController _vcPrefab;
    [SerializeField] private Slider _slider;

    private List<StringViewController> _vcs = new List<StringViewController>();

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
        for (int i = 0; i < _vcs.Count; i++)
        {
            GameObject.Destroy(_vcs[i].gameObject);
        }

        _vcs.Clear();

        _workspace.StringsCount = (int)_slider.value;

        for (int i = 0; i < _workspace.StringsCount; i++)
        {
            var vc = GameObject.Instantiate(_vcPrefab).GetComponent<StringViewController>();
            vc.String = _workspace.StringById(i);
            _vcs.Add(vc);
        }
    }
}
