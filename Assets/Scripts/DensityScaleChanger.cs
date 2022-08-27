using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityScaleChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  0 = 1
        //0.5 = 0.75
        //  1 = 0.5
        //  2 = 0.25

        var scaleX = Mathf.Clamp( Mathf.Lerp(1, 0.5f, Air.Density), 0.1f, 1 );
        transform.localScale = new Vector3( scaleX, scaleX, transform.localScale.z);
    }
}
