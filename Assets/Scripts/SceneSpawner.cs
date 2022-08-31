using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneSpawner
{
    public static GameObject SpawnMoleculesChain(GameObject moleculesPrefab, Vector3 posLeft, Vector3 posRight, int count, FluctuatingObject1D first)
    {
        if (count <= 0)
        {
            return null;
        }

        var parent = new GameObject();
        parent.transform.position = (posLeft + posRight) / 2;

        var xDir = (posRight - posLeft);
        var cross = Vector3.Angle(Vector3.right, xDir);
        parent.transform.rotation = Quaternion.Euler(0, 0, cross);

        var length = (posRight - posLeft).magnitude;
        var offset = length / count;

        var xPos = offset / 2f;
        FluctuatingObject1D last = first;

        for (int i = 0; i < count; i++)
        {
            var go = GameObject.Instantiate(moleculesPrefab);
            go.SetActive(false);

            go.transform.parent = parent.transform;
            go.transform.localPosition = new Vector2(xPos - length / 2, 0);

            var molecule = go.GetComponent<FluctuatingMolecule1D>();

            molecule.Source = last;
            last = molecule;

            xPos += offset;

            go.SetActive(true);
        }

        return parent;
    }
}

/*
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

 */
