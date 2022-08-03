using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugObjectText;
    
    private object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        debugObjectText.text = gridObject.ToString();
    }
}
