using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CirclesSpawnerViewController : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Slider _slider;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float y;

    private float _lastValue;
    private ObjectsSpawner _spawner;

    private List<GameObject> _list = new List<GameObject>();

    void Start()
    {
        _lastValue = _slider.minValue - 1;
        _spawner = new ObjectsSpawner();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_lastValue != _slider.value)
        {
            SpawnRaw(_slider.value);
            _lastValue = _slider.value;
        }
    }

    public void SpawnRaw(float dens, PressureField field)
    {
        if (_list.Count > 0)
        {
            foreach (var go in _list)
            {
                GameObject.Destroy(go);
            }
        }

        _list.Clear();

        var scale = _prefab.transform.localScale.x;
        var count = (int)Mathf.Max((xMax - xMin) / scale * dens, 1);

        for (int i = 0; i < count; i++)
        {
            var posX = Mathf.Lerp(xMin, xMax, (float)i / count);
            var pos = new Vector3(posX, y, 0);

            var go = GameObject.Instantiate(_prefab);
            go.GetComponent<Rigidbody2D>().position = pos;
            go.GetComponent<PressureAffective>().Field = field;

            _list.Add(go);
        }
    }

}
