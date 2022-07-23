using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private Animator unitAnimator;

    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            // Unit changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
