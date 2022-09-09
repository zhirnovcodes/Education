using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Coroutine _routine;
    private Vector3 _stablePos;

    private void Start()
    {
        _stablePos = transform.position;
    }

    void Update()
    {
        if (_routine == null)
        {
            _routine = StartCoroutine(Send());
        }
    }

    private IEnumerator Send()
    {
        var value = Random.Range(0, 2);

        transform.position = _stablePos + (value == 0 ? Vector3.left : Vector3.right); 

        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));

        _routine = null;
    }
}
