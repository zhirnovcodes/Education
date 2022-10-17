using UnityEngine;
using UnityEngine.UI;

public class VisibleViewController : MonoBehaviour
{
    public MeshRenderer Uncompressed;
    public MeshRenderer Compressed;
    public MeshRenderer Threshold;
    public MeshRenderer GR;
    public MeshRenderer Makeup;
    public MeshRenderer DefMakeup;

    public Toggle UncompressedT;
    public Toggle CompressedT;
    public Toggle ThresholdT;
    public Toggle GRT;
    public Toggle MakeupT;
    public Toggle DefMakeupT;

    private Vector3 _makeupPos;

    private void Start()
    {
        UncompressedT.isOn = Uncompressed.enabled;
        CompressedT.isOn = Compressed.enabled;
        ThresholdT.isOn = Threshold.enabled;
        GRT.isOn = GR.enabled;
        DefMakeupT.isOn = DefMakeup.enabled;
        MakeupT.isOn = Makeup.enabled;
    }

    private void Update()
    {
        if (Uncompressed.enabled != UncompressedT.isOn)
        {
            Uncompressed.enabled = UncompressedT.isOn;
        }

        if (Compressed.enabled != CompressedT.isOn)
        {
            Compressed.enabled = CompressedT.isOn;
        }

        if (Threshold.enabled != ThresholdT.isOn)
        {
            Threshold.enabled = ThresholdT.isOn;
        }

        if (GR.enabled != GRT.isOn)
        {
            GR.enabled = GRT.isOn;
        }

        if (Makeup.enabled != MakeupT.isOn)
        {
            Makeup.enabled = MakeupT.isOn;
        }

        if (DefMakeup.enabled != DefMakeupT.isOn)
        {
            DefMakeup.enabled = DefMakeupT.isOn;
        }

    }
}
