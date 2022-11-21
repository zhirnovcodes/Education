using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylNeedleMover : MonoBehaviour
{
    [SerializeField] private VinylGraphDrawer _drawer;
    [SerializeField] private bool _moveX = true;
    [SerializeField] private bool _moveZ = true;

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;
        var pos = _drawer.GetPosition(_drawer.RecordedT);
        pos.x = _moveX ? pos.x : x;
        pos.y = y;
        pos.z = _moveZ ? pos.z : z;
        transform.position = pos;
    }
}
