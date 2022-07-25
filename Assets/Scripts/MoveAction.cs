using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] int maxMoveDistance = 3;
    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        targetPosition = transform.position;
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {   
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float moveSpeed = 5f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotationSpeed = 25f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            unitAnimator.SetBool("isWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
