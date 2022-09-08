using System.Collections;
using UnityEngine;

public class DigitalGraphDrawer : MonoBehaviour
{
    [SerializeField] private DigitalCable _cable;
    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color _linesColor = new Color(1, 1, 1, 1);
    private GraphDrawer _drawer;

    private int _lastIndex;

    void Start()
    {
        _drawer = new GraphDrawer();
        _cable.OnDigitalValueSent += ValueSent;
    }

    private void ValueSent(int value)
    {
        _drawer.AddValue(_lastIndex, value);
        _drawer.Paint(_bckgColor, _linesColor, 1);
        _lastIndex++;
    }
}
