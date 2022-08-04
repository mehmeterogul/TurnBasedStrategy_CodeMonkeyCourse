using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.T))
    {
      GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
      GridPosition startGridPosition = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
      List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition, out int pathLength);

      for (int i = 0; i < gridPositionList.Count - 1; i++)
      {
        Debug.DrawLine(
            LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
            LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
            Color.blue,
            10f
          );
      }

      Debug.Log(pathLength);
    }
  } 
}
