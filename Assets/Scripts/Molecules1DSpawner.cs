using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecules1DSpawner : MoleculesSpawner<Molecule1D>
{
    [SerializeField] private FunctionBuffer _buffer;

    protected override void SetupComponent(Molecule1D component, MoleculePosition pos)
    {
        var maxDelay = Mathf.Lerp(3, 0.2f, _grid.Density);
        var minPower = Mathf.Lerp(0.1f, 1, _grid.Density);

        var factor = pos.Uv.x;
        var delay = Mathf.Lerp(0.1f, maxDelay, factor);
        var power = Mathf.Lerp(1, minPower, factor);
        component.Delay = delay;
        component.Power = power;
        if (_buffer != null)
        { 
            component.Buffer = _buffer; 
        }
    }
}
