using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveredObjectPercentage : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    public static GameObject focusedObject;
    [SerializeField] public static GameObject nonFocusObj;
    private string hObjectName;
    [SerializeField] private LayerMask layerMask;
    private float displayPercent;
    void Start()
    {
        focusedObject = nonFocusObj;
    }

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 10, layerMask))
        {
            hObjectName = hitInfo.transform.name;
            PaintableObject obj = hitInfo.transform.GetComponent<PaintableObject>();

            if (displayPercent < 100)
                displayPercent = 100 / (obj.completionPercentage * 100) * obj.completedPercentage * 100;
            else if (displayPercent > 100)
                displayPercent = 100;

            if (Input.GetKeyDown(KeyCode.F))
                focusedObject = hitInfo.collider.gameObject;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            focusedObject = nonFocusObj;
        }
        //Debug.Log(displayPercent + "%");
    }
}
