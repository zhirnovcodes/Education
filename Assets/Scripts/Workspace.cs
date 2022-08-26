using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    [SerializeField] private GameObject _moleculePrefab;
    [SerializeField] private Rigidbody2D _ear;
    [SerializeField] private Rigidbody2D _wall;
    [SerializeField] private float xMax;

    private List<List<GameObject>> _molecules = new List<List<GameObject>>();
    private List<String1D> _strings = new List<String1D>();
    private List<LowPressureForceProvider1D> _earLpfs = new List<LowPressureForceProvider1D>();

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
                SpawnRow(value, i);
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
        _earLpfs = _ear.GetComponents<LowPressureForceProvider1D>().ToList();
    }

    public void SpawnRow(float dens, int stringId)
    {
        if (_earLpfs.Count > stringId)
        {
            _earLpfs[stringId].Left = null;
        }

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

        var s = _strings[stringId];
        var mS = _molecules[stringId];
        var xMin = s.Position.x + s.transform.localScale.x / 2;
        var scale = _moleculePrefab.transform.localScale.x;
        var count = (int)Mathf.Max((xMax - xMin) / scale * dens, 1);
        var offset = (xMax - xMin) / (float)(count + 1);

        for (int i = 0; i < count; i++)
        {
            var posX = Mathf.Lerp(xMin + offset, xMax, (float)i / count);
            var pos = new Vector3(posX, s.Position.y, 0);

            var go = GameObject.Instantiate(_moleculePrefab);
            go.transform.position = pos;

            var force = go.GetComponent<LowPressureForceProvider1D>();
            
            force.Left = (i == 0 ? s.gameObject : mS[i - 1]).GetComponent<Rigidbody2D>();

            if (i > 0)
            {
                mS[i - 1].GetComponent<LowPressureForceProvider1D>().Rigth = go.GetComponent<Rigidbody2D>();
            }

            mS.Add(go);
        }

        var mL = mS[count - 1];

        if (_earLpfs.Count <= stringId)
        {
            var countToAdd = stringId - _earLpfs.Count;
            for (int i = 0; i <= countToAdd; i++)
            {
                var c = _ear.gameObject.AddComponent<LowPressureForceProvider1D>();
                c.Rigth = _wall;
                _earLpfs.Add(c);
            }
        }


        _earLpfs[stringId].Left = mL.GetComponent<Rigidbody2D>();

        _earLpfs[stringId].gameObject.SetActive(false);
        _earLpfs[stringId].gameObject.SetActive(true);

        mL.GetComponent<LowPressureForceProvider1D>().Rigth = _wall.GetComponent<Rigidbody2D>();
    }

}
