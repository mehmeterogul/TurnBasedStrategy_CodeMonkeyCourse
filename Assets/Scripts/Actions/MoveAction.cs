using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int maxMoveDistance = 3;
    private Vector3 targetPosition;
    
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = 0.1f;
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {   
            float moveSpeed = 5f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            
            ActionComplete();
        }

        float rotationSpeed = 25f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    // same grid position where the unit is already at
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // grid position has already occupied with another unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtGridPosition(gridPosition);

        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
