using UnityEngine;

public class FluctuatingString1D : FluctuatingObject1D
{
    [SerializeField] private Fluctuation _fluctuation;

    private float _timeStart;
    public override float TimeStart => _timeStart;
    public override Fluctuation Fluctuation => _fluctuation;


    private void OnEnable()
    {
        _timeStart = 0;
    }

    public void Hit()
    {
        _timeStart = Time.time;
    }
}

