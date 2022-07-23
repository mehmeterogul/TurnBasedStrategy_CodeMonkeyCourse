using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GridSystem gridSystem;
    [SerializeField] Transform debugObjectPrefab;

    private void Start()
    {
        gridSystem = new GridSystem(20, 20, 2f);
        gridSystem.CreateDebugObjects(debugObjectPrefab);
    }

    private void Update()
    {
        // Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
