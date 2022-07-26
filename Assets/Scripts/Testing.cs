using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridSystemVisual.Instance.HideAllGridPosition();
            List<GridPosition> unitValidPositionList = unit.GetMoveAction().GetValidActionGridPositionList();
            GridSystemVisual.Instance.ShowGridPositionList(unitValidPositionList);
        }
    }
}
