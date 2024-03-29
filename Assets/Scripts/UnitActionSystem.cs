using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    private BaseAction selectedAction;
    [SerializeField] LayerMask unitLayerMask;
    private bool isBusy;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if(isBusy)
        {
            return;
        }

        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        // Unit is already selected
                        return false;
                    }

                    if(unit.IsEnemy())
                    {
                        // Clicked on Enemy
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleSelectedAction()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            if(selectedUnit == null)
            {
                List<Unit> friendlyUnitList = UnitManager.Instance.GetFriendlyUnitList();
                
                if(friendlyUnitList.Count > 0)
                {
                    SetSelectedUnit(friendlyUnitList[0]);
                }
                else
                {
                    SetSelectedUnit(null);
                }
            }
        }
    }
}
