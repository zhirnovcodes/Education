using System.Collections;
using UnityEngine;

public class NumbersGridController : MonoBehaviour
{
    [SerializeField] private DigitalToAnalogueGraphDrawer _digital;
    [SerializeField] private bool _drawFullGrid = true;
    private NumbersGrid _grid;

    private int _index;

    private void Start()
    {
        _grid = GetComponent<NumbersGrid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (_index)
            {
                case 0:
                    {
                        if (_drawFullGrid)
                        {
                            _grid.DrawGrid();
                            _grid.DrawIndicies();
                            _grid.DrawValues();
                            _index++;
                        }
                        else
                        {
                            _grid.DrawGrid(true, false);
                        }
                        break;
                    }
                case 1:
                    {
                        _grid.DrawGrid(false, true);
                        _grid.DrawIndicies();
                        _grid.DrawValues();
                        break;
                    }
                case 2:
                    {
                        _digital.Fill();
                        break;
                    }
                case 3:
                    {
                        _digital.transform.position = _grid.transform.position;
                        _grid.HideValues();
                        break;
                    }
            }
            _index++;
        }
    }
}
