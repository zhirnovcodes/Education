using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylController : MonoBehaviour
{
    [SerializeField] private VinylGraphDrawer _drawer;
    [SerializeField] private VinylNeedleMover _needle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _drawer.enabled = !_drawer.enabled;
            _needle.enabled = !_needle.enabled;
        }
    }
}
