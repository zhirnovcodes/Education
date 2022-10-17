using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverbWorkspace : MonoBehaviour
{
    [SerializeField] private GameObject _moleculesPrefab;
    [SerializeField] private GameObject _string;
    [SerializeField] private GameObject _ear;
    [SerializeField] private float _density = 0.6f;
    [SerializeField] private List<GameObject> _objects;


    private List<GameObject> _molecules = new List<GameObject>();
    private List<FluctuatingMolecule1D> _earComps = new List<FluctuatingMolecule1D>();

    /*
    public void Start()
    {
        SpawnToEar(_ear);
        foreach (var obj in _objects)
        {
            SpawnToObject(obj, _ear);
        }
    }

    private void DestroyAllMolecules()
    {
        var c = _molecules.Count;
        for (int i = c - 1; i >= 0; i--)
        {
            var go = _molecules[i];
            _molecules.RemoveAt(i);
            GameObject.Destroy(go);
        }

        c = _earComps.Count;
        for (int i = c - 1; i >= 0; i--)
        {
            var go = _earComps[i];
            _earComps.RemoveAt(i);
            GameObject.Destroy(go);
        }
    }

    private void SpawnToObject(GameObject obj, GameObject ear)
    {
        var gr1 = SceneSpawner.SpawnMoleculesChain(_moleculesPrefab, _string.transform.position, obj.transform.position, _density, _string.GetComponent<FluctuatingObject1D>());
        var lastMol1 = gr1.transform.GetChild(gr1.transform.childCount - 1).GetComponent<FluctuatingMolecule1D>();
        
        var gr2 = SceneSpawner.SpawnMoleculesChain(_moleculesPrefab, obj.transform.position, ear.transform.position, _density, lastMol1.GetComponent<FluctuatingObject1D>());
        var lastMol2 = gr2.transform.GetChild(gr2.transform.childCount - 1).GetComponent<FluctuatingMolecule1D>();

        var m = ear.AddComponent<FluctuatingMolecule1D>();
        m.Sources.Add( lastMol2 as FluctuatingObject1D );

        _molecules.Add(gr1);
        _molecules.Add(gr2);
        _earComps.Add(m);
    }


    private void SpawnToEar(GameObject ear)
    {
        var gr = SceneSpawner.SpawnMoleculesChain(_moleculesPrefab, _string.transform.position, ear.transform.position, _density, _string.GetComponent<FluctuatingObject1D>());
        var lastMol = gr.transform.GetChild(gr.transform.childCount - 1).GetComponent<FluctuatingMolecule1D>();

        var m = ear.AddComponent<FluctuatingMolecule1D>();
        m.Source = lastMol;

        _molecules.Add(gr);
        _earComps.Add(m);
    }
    */
}
