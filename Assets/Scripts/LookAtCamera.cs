using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    // Update is called once per frame
    private void Awake()
    {
        cameraTransform = Camera.main.transform;    
    }

    private void LateUpdate()
    {
        if(invert)
        {
            Vector3 dirToCam = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCam * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
