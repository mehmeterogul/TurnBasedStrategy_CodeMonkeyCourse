using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GridSystem gridSystem;
    [SerializeField] Transform debugObjectPrefab;

    private void Start()
    {
        gridSystem = new GridSystem(10, 10, 2f);
    }

    private void Update()
    {
        // Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        gridSystem.CreateDebugObjects(debugObjectPrefab);
    }
}
