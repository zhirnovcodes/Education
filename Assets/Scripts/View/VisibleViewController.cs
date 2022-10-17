using UnityEngine;
using UnityEngine.UI;

public class VisibleViewController : MonoBehaviour
{
    public MeshRenderer Uncompressed;
    public MeshRenderer Compressed;
    public MeshRenderer Threshold;
    public MeshRenderer GR;
    public Transform Makeup;

    public Toggle UncompressedT;
    public Toggle CompressedT;
    public Toggle ThresholdT;
    public Toggle GRT;
    public Toggle MakeupT;

    private Vector3 _makeupPos;

    private void Start()
    {
        UncompressedT.isOn = Uncompressed.enabled;
        CompressedT.isOn = Compressed.enabled;
        ThresholdT.isOn = Threshold.enabled;
        GRT.isOn = GR.enabled;

        MakeupT.isOn = false;
        _makeupPos = Makeup.position;
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

        Makeup.position = MakeupT.isOn ? Threshold.transform.position : _makeupPos;
    }
}
