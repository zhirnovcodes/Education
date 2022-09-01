using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringCountViewController : MonoBehaviour
{
    [SerializeField] private Workspace _workspace;
    [SerializeField] private StringViewController _vcPrefab;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _content;

    private List<StringViewController> _vcs = new List<StringViewController>();

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        _slider.value = _workspace.StringsCount;
        if (_workspace.StringsCount != _slider.value)
        {
            _workspace.StringsCount = Mathf.RoundToInt(_slider.value);
        }
        SetViewControllers();
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _workspace.StringsCount = Mathf.RoundToInt( _slider.value );

        SetViewControllers();
    }

    private void SetViewControllers()
    {
        var value = Mathf.RoundToInt(_slider.value);
        if (value < _vcs.Count)
        {
            for (int i = 0; i < _vcs.Count - value; i++)
            {
                DeleteStringFromEnd();
            }
        }
        else if (value > _vcs.Count)
        {
            for (int i = 0; i < value - _vcs.Count; i++)
            {
                AddStringToEnd();
            }
        }
    }

    private void DeleteStringFromEnd()
    {
        if (_vcs.Count == 0)
        {
            return;
        }

        var stringId = _vcs.Count - 1;

        var go = _vcs[stringId].gameObject;
        _vcs.RemoveAt(stringId);
        GameObject.Destroy(go);
    }

    private void AddStringToEnd()
    {
        var newIndex = _vcs.Count;
        var go = GameObject.Instantiate(_vcPrefab.gameObject);
        var s = go.GetComponent<StringViewController>();
        s.String = _workspace.StringById(newIndex);

        //go.SetActive(false);
        go.transform.parent = _content.transform;
        //go.SetActive(true);

        _vcs.Add(s);
    }

}
