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
    //private bool changeFocusTxt;

    void Start()
    {
    }

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
                    //changeFocusTxt = false;
                    isFocused = true;
                    Debug.Log("Focus from null");
                }
                else if (hoveredObject == focusedObject)
                {
                    focusedObject = null;
                    isFocused = false;
                    Debug.Log("Focus to null (Aim at focus)");
                } 
                else
                {
                    focusedObject = hoveredObject;
                    //changeFocusTxt = false;
                    isFocused = true;
                    Debug.Log("Focus change");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && focusedObject != null)
        {
            focusedObject = null;
            isFocused = false;
            hoveredObjectName = null;
            Debug.Log("Focus to null (Aim at nothing)");
        }
        else
        {
            hoveredObjectName = null;
        }

        if (isFocused/* && !changeFocusTxt*/)
        {
            focusTMP.text = "Focused on : " + hoveredObjectName;
            //changeFocusTxt = true;
        }  
        else
            focusTMP.text = "";

        if (hoveredObjectName != null)
            hoverTMP.text = hoveredObjectName;
        else
            hoverTMP.text = "";
    }
}
