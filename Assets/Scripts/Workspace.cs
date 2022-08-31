using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    [SerializeField] private GameObject _moleculePrefab;
    [SerializeField] private GameObject _stringPrefab;
    [SerializeField] private GameObject _graphPrefab;
    [SerializeField] private GameObject _ear;
    [SerializeField] private PositionOffsetDrawer _earGraph;
    [SerializeField] private Transform _leftAnchor;
    [SerializeField] private Transform _rightAnchor;
    [SerializeField] private float _stringOffset = 2;

    [SerializeField, Range(0,1)] private float _moleculesDensity = 0.5f;
    //[SerializeField] private float xMax;

    private List<GameObject> _molecules = new List<GameObject>();
    private List<GameObject> _graphs = new List<GameObject>();
    private List<FluctuatingString1D> _strings = null;
    private List<FluctuatingMolecule1D> _earComps = new List<FluctuatingMolecule1D>();

    private List<FluctuatingString1D> GetStrings()
    {
        var shouldResetPosition = _strings == null;
        _strings = _strings ?? FindObjectsOfType<FluctuatingString1D>().ToList();
        if (shouldResetPosition)
        {
            ResetStringsPosition();
            ResetEar();
        }
        return _strings;
    }

    public GameObject Ear => _ear?.gameObject;

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
                    DeleteMoleculesFromEnd();
                    DeleteStringFromEnd();
                }
                ResetStringsPosition();
                ResetMoleculesPosition();
                ResetEar();
            }
            else if (value > GetStrings().Count)
            {
                for (int i = 0; i < value - GetStrings().Count; i++)
                {
                    var index = AddStringToEnd();
                    ResetStringsPosition();
                    SpawnMoleculesToString(index);
                }

                ResetMoleculesPosition();
                ResetEar();
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
            var c1 = MoleculesCount(_leftAnchor, out var posMin, out var posMax);

            _moleculesDensity = value;

            if (c1 == MoleculesCount(_leftAnchor, out posMin, out posMax))
            {
                return;
            }

            DeleteMolecules();
            for (var i = 0; i < StringsCount; i++)
            {
                SpawnMoleculesToString(i);
            }

            ResetEar();
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

        var go = _strings[stringId].gameObject;
        _strings.RemoveAt(stringId);
        GameObject.Destroy(go);

        var gr = _graphs[stringId];
        _graphs.RemoveAt(stringId);
        GameObject.Destroy(gr);
    }

    private int AddStringToEnd()
    {
        var newIndex = GetStrings().Count;
        var go = GameObject.Instantiate(_stringPrefab);
        var s = go.GetComponent<FluctuatingString1D>();

        _strings.Add(s);

        return newIndex;
    }

    private void SpawnGraph(FluctuatingString1D str)
    {
        var gr = GameObject.Instantiate(_graphPrefab);
        var dr = gr.GetComponentInChildren<PositionOffsetDrawer>();
        dr.Target = str.transform;
        _graphs.Add(gr);
    }

    private void DeleteMolecules()
    {
        var c = _molecules.Count;
        for ( int i = c - 1; i >= 0; i-- )
        {
            var go = _molecules[i];
            _molecules.RemoveAt(i);
            GameObject.Destroy(go);
        }
    }

    private void DeleteMoleculesFromEnd()
    {
        if (_molecules.Count > 0)
        {
            var go = _molecules[_molecules.Count - 1];
            _molecules.RemoveAt(_molecules.Count - 1);
            GameObject.Destroy(go);
        }
    }

    private int MoleculesCount(Transform _leftObject, out Vector3 posMin, out Vector3 posMax)
    {
        posMin = _leftObject.position;
        posMax = _rightAnchor.position;
        posMax.y = posMin.y;
        var scale = _moleculePrefab.transform.localScale.x;
        return (int)Mathf.Max((posMax.x - posMin.x) / scale * _moleculesDensity, 1);
    }

    private void SpawnMoleculesToString(int stringId)
    {
        var s = GetStrings()[stringId];
        var count = MoleculesCount(s.transform, out var posMin, out var posMax);

        var go = SceneSpawner.SpawnMoleculesChain(_moleculePrefab, posMin, posMax, count, s);
        _molecules.Add(go);
    }

    private void ResetStringsPosition()
    {
        var count = GetStrings().Count;
        var offset = (_stringOffset + _stringPrefab.transform.localScale.y);
        var height = offset * (count - 1);

        var x = _leftAnchor.position.x;
        var y = _leftAnchor.position.y;

        var y0 = y + height / 2;

        for (int i = 0; i < count; i++)
        {
            _strings[i].gameObject.SetActive(false);
            _strings[i].transform.position = new Vector2(x, y0);
            _strings[i].gameObject.SetActive(true);

            if (_graphs.Count <= i)
            {
                for (int j = _graphs.Count; j <= i; j++)
                {
                    SpawnGraph(_strings[j]);
                }
            }
            _graphs[i].transform.position = _strings[i].transform.position - new Vector3(2, 0) - new Vector3(_graphPrefab.transform.localScale.y, 0);
            _graphs[i].GetComponentInChildren<PositionOffsetDrawer>().Target = _strings[i].transform;

            y0 -= offset;
        }
    }


    private void ResetMoleculesPosition()
    {
        var maxPos = _rightAnchor.position;

        for (int i = 0; i < _molecules.Count; i++)
        {
            var s = GetStrings()[i].transform.position;

            var pos = (s + maxPos) / 2;
            pos.y = s.y;

            _molecules[i].transform.position = pos;
        }
    }

    private void ResetEar()
    {
        if (_ear == null)
        {
            return;
        }

        int c = _earComps.Count;

        if (c != StringsCount)
        {
            for (int i = 0; i < c; i++)
            {
                var comp = _earComps[0];
                _earComps.RemoveAt(0);
                GameObject.Destroy(comp);
            }

            for (int i = 0; i <= StringsCount; i++)
            {
                var earM = _ear.gameObject.AddComponent<FluctuatingMolecule1D>();
                if (_molecules.Count > i)
                {
                    var mol = _molecules[i].transform.GetChild(_molecules[i].transform.childCount - 1).GetComponent<FluctuatingObject1D>();
                    earM.Source = mol;
                    _earComps.Add(earM);

                }
            }
        }

        _ear.gameObject.SetActive(false);
        _ear.transform.position = _rightAnchor.position;
        _ear.gameObject.SetActive(true);

        _earGraph.Target = _ear.transform;

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
        _earLpfs[stringId].gameObject.SetActive(true);
        */

    }
}
