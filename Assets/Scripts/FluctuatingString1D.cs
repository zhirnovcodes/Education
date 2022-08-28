using UnityEngine;

public class FluctuatingString1D : MonoBehaviour, IFluctuatingObject1D
{
    [SerializeField] private Fluctuation _fluctuation;

    public float TimeStart { get; set; }
    public Fluctuation Fluctuation => _fluctuation;

    public float GetX()
    {
        if (TimeStart == 0)
        {
            return 0;
        }

        var deltaTime = Air.Time - TimeStart;
        var x = _fluctuation.GetValue(deltaTime);
        return x;
    }

    private void OnEnable()
    {
        TimeStart = 0;
    }
}

