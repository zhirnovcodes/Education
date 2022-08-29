using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetDrawerKeyboardController : MonoBehaviour
{
    private PositionOffsetDrawer[] _drawers;

    void Start()
    {
        _drawers = _drawers ?? UnityEngine.Object.FindObjectsOfType<PositionOffsetDrawer>();
        foreach (var d in _drawers)
        {
            d.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Start();
            foreach (var d in _drawers)
            {
                d.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Start();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                string p = null;
                foreach (var d in _drawers)
                {
                    var name = d.gameObject.name;
                    p = d.Texture.SaveRTToFile(name);
                }
                if (p != null)
                {
                    FilesExtensions.ShowInExplorer(p);
                }
            }
        }
    }

}
