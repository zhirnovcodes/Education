using System.Collections;
using UnityEngine;

public class NumbersGridController : MonoBehaviour
{
    [SerializeField] private DigitalToAnalogueGraphDrawer _digital;
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
                        _grid.DrawGrid();
                        _grid.DrawIndicies();
                        break;
                    }
                case 1:
                    {
                        _grid.DrawValues();
                        break;
                    }
                case 2:
                    {
                        _digital.Fill() ;
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
