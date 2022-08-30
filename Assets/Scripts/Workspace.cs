using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    [SerializeField] private GameObject _moleculePrefab;
    [SerializeField] private GameObject _stringPrefab;
    [SerializeField] private Rigidbody2D _ear;
    [SerializeField] private Transform _leftAnchor;
    [SerializeField] private Transform _rightAnchor;
    [SerializeField] private float _stringOffset = 2;

    [SerializeField, Range(0,1)] private float _moleculesDensity = 0.5f;
    //[SerializeField] private float xMax;

    private List<List<GameObject>> _molecules = new List<List<GameObject>>();
    private List<FluctuatingString1D> _strings = null;
    //private List<LowPressureForceProvider1D> _earLpfs = new List<LowPressureForceProvider1D>();

    private List<FluctuatingString1D> GetStrings()
    {
        _strings = _strings ?? FindObjectsOfType<FluctuatingString1D>().ToList();
        return _strings;
    }

    public GameObject Ear => _ear?.gameObject;

    public IEnumerable<GameObject> Molecules
    {
        get
        {
            foreach (var m in _molecules)
            {
                foreach (var g in m)
                {
                    yield return g;
                }
            }
        }
    }

    public IEnumerable<FluctuatingString1D> Strings => GetStrings();

    public int StringsCount 
    { 
        get 
        {
            return GetStrings().Count;
        }
        set 
        { 
            if (value < GetStrings().Count)
            {
                for (int i = 0; i < GetStrings().Count - value; i++)
                {
                    DeleteStringFromEnd();
                }
            }
            else if (value > GetStrings().Count)
            {
                for (int i = 0; i < value - GetStrings().Count; i++)
                {
                    AddStringToEnd();
                }
            }
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
                AddMoleculesRow(i);
            }
        }
    }

    public FluctuatingString1D StringById(int stringId)
    {
        return GetStrings()[stringId];
    }

    private void DeleteStringFromEnd()
    {
        if (GetStrings().Count == 0)
        {
            return;
        }

        var stringId = _strings.Count - 1;
        DeleteMoleculesRow(stringId);

        var go = _strings[stringId].gameObject;
        _strings.RemoveAt(stringId);
        GameObject.Destroy(go);
    }

    private void AddStringToEnd()
    {
        var newIndex = GetStrings().Count;
        var go = GameObject.Instantiate(_stringPrefab);
        var s = go.GetComponent<FluctuatingString1D>();

        _strings.Add(s);

        ResetStringPosition(newIndex);

        AddMoleculesRow(newIndex);
    }

    private void DeleteMoleculesRow(int stringId)
    {
        //if (_earLpfs.Count > stringId)
        //{
        //    _earLpfs[stringId].Left = null;
        //}

        if (_molecules.Count > stringId)
        {
            for (int i = 0; i < _molecules[stringId].Count; i++)
            {
                Destroy(_molecules[stringId][i]);
            }
            _molecules[stringId].Clear();
        }
    }

    private void AddMoleculesRow(int stringId)
    {
        DeleteMoleculesRow(stringId);

        if (_molecules.Count <= stringId)
        {
            var c = stringId - _molecules.Count + 1;
            for (int i = 0; i < c; i++)
            {
                _molecules.Add(new List<GameObject>());
            }
        }

        var s = GetStrings()[stringId];
        var mS = _molecules[stringId];
        var xMin = s.transform.position.x + s.transform.localScale.x / 2;
        var xMax = _rightAnchor.position.x - _rightAnchor.localScale.x / 2;
        var scale = _moleculePrefab.transform.localScale.x;
        var count = (int)Mathf.Max((xMax - xMin) / scale * _moleculesDensity, 1);
        var offset = (xMax - xMin) / (float)(count + 1);

        for (int i = 0; i < count; i++)
        {
            var posX = Mathf.Lerp(xMin + offset, xMax, (float)i / count);
            var pos = new Vector3(posX, s.transform.position.y, 0);

            var go = GameObject.Instantiate(_moleculePrefab);
            go.SetActive(false);

            go.transform.position = pos;

            var molecule = go.GetComponent<FluctuatingMolecule1D>();

            molecule.Source = (i == 0 ? s.gameObject : mS[i - 1]).GetComponent<FluctuatingObject1D>();
            go.SetActive(true);

            mS.Add(go);
        }

        if (_ear == null) 
        {
            return;
        }

        /*
        if (_earLpfs.Count <= stringId)
        {
            var countToAdd = stringId - _earLpfs.Count;
            for (int i = 0; i <= countToAdd; i++)
            {
                var c = _ear.gameObject.AddComponent<LowPressureForceProvider1D>();
                c.Rigth = _ear;
                _earLpfs.Add(c);
            }
        }

        _earLpfs[stringId].Left = mL.GetComponent<Rigidbody2D>();

        _earLpfs[stringId].gameObject.SetActive(false);
        _ear.transform.position = new Vector3( (mL.transform.position.x + scale / 2 + xMax) / 2, (_ear ?? _wall).transform.position.y, 0);
        _earLpfs[stringId].gameObject.SetActive(true);
        */
    }

    private void ResetStringPosition(int index)
    {
        var x = _leftAnchor.position.x;
        var y = _leftAnchor.position.y - (_stringOffset + _stringPrefab.transform.localScale.y) * (float)index;

        _strings[index].gameObject.SetActive(false);
        _strings[index].transform.position = new Vector2(x, y);
        _strings[index].gameObject.SetActive(true);
    }

}
