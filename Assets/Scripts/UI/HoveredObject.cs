using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HoveredObject : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    private string hoveredObjectName;
    private GameObject hoveredObject;
    public static GameObject focusedObject;
    public static bool isFocused;
    [SerializeField] private LayerMask layerMask;
    private float displayPercent;
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private LayerMask focussedLayerMask;
    [SerializeField] private LayerMask unFocussedLayerMask;

    [SerializeField] TextMeshProUGUI focusTMP;
    [SerializeField] TextMeshProUGUI hoverTMP;

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 10, layerMask))
        {
            hoveredObjectName = hitInfo.transform.name;
            hoveredObject = hitInfo.transform.gameObject;
            PaintableObject obj = hitInfo.transform.GetComponent<PaintableObject>();
            if (displayPercent < 100)
            {
                displayPercent = 100 / (obj.completionPercentage * 100) * obj.completedPercentage * 100;
            }
            else if (displayPercent > 100)
            {
                displayPercent = 100;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (focusedObject == null)
                {
                    focusedObject = hoveredObject;
                    isFocused = true;
                }
                else if (hoveredObject == focusedObject)
                {
                    focusedObject = null;
                    isFocused = false;
                } 
                else
                {
                    focusedObject = hoveredObject;
                    isFocused = true;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && focusedObject != null)
        {
            focusedObject = null;
            isFocused = false;
            hoveredObjectName = null;
        }
        else
        {
            hoveredObjectName = null;
        }

        if (isFocused)
        {
            focusTMP.text = "Focused on : " + focusedObject.name;
        }  
        else
            focusTMP.text = "";

        if (hoveredObjectName != null)
            hoverTMP.text = hoveredObjectName;
        else
            hoverTMP.text = "";
    }
}
