using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    [SerializeField] private GameObject _moleculePrefab;
    [SerializeField] private float xMax;
    [SerializeField] private float size;

    private List<List<GameObject>> _molecules = new List<List<GameObject>>();
    private List<String1D> _strings = new List<String1D>();

    public int StringsCount 
    { 
        get 
        {
            return _strings.Count;
        }
        set 
        { 
        }
    }
    public float MoleculesDensity 
    { 
        get 
        {
            return _moleculesDensity;
        }
        set 
        {
            _moleculesDensity = value;
            for (var i = 0; i < StringsCount; i++)
            {
                SpawnRaw(value, i);
            }
        }
    }

    private float _moleculesDensity;

    public String1D StringById(int stringId)
    {
        return _strings[stringId];
    }

    private void Start()
    {
        _strings = FindObjectsOfType<String1D>().ToList();
    }

    public void SpawnRaw(float dens, int stringId)
    {
        if (_molecules.Count > stringId)
        {
            for (int i = 0; i < _molecules[stringId].Count; i++)
            {
                Destroy(_molecules[stringId][i]);
            }
            _molecules[stringId].Clear();
        }
        else
        {
            var c = stringId - _molecules.Count + 1;
            for (int i = 0; i < c; i++)
            {
                _molecules.Add(new List<GameObject>());
            }
        }

        var xMin = _strings[stringId].Position.x + _strings[stringId].transform.localScale.x;
        var scale = size;//_moleculePrefab.transform.localScale.x;
        var count = (int)Mathf.Max((xMax - xMin) / scale * dens, 1);

        for (int i = 0; i < count; i++)
        {
            var posX = Mathf.Lerp(xMin, xMax, (float)i / count);
            var pos = new Vector3(posX, _strings[stringId].Position.y, 0);

            var go = GameObject.Instantiate(_moleculePrefab);
            go.GetComponent<Rigidbody2D>().position = pos;

            _molecules[stringId].Add(go);
        }
    }

}
