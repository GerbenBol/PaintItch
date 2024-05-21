using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveredObjectPercentage : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    private string hoveredObject;
    [SerializeField] private LayerMask layerMask;
    private float displayPercent;
    void Start()
    {
        
    }

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 10, layerMask))
        {
            hoveredObject = hitInfo.transform.name;
            PaintableObject obj = hitInfo.transform.GetComponent<PaintableObject>();
            if (displayPercent < 100)
            {
                displayPercent = 100 / (obj.completionPercentage * 100) * obj.completedPercentage;
            }
            else if (displayPercent > 100)
            {
                displayPercent = 100;
            }
        }
        Debug.Log(displayPercent + "%");
    }
}
