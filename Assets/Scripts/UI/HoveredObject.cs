using System;
using System.Collections;
using System.Collections.Generic;
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

    //private PaintSplatter paintSplatter;
    void Start()
    {
        //paintSplatter = paintPrefab.GetComponent<PaintSplatter>();
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
                    isFocused = true;
                    Debug.Log("Focus change");
                }
                /*if (focusedObject != null && focusedObject.layer == 7)
                {
                    focusedObject.layer = 6;
                    focusedObject = null;
                    Debug.Log("Unfocused Object");
                }
                if (hoveredObject != focusedObject)
                {
                    focusedObject = hoveredObject;
                    focusedObject.layer = 7;
                    Debug.Log("Focused Object");
                }*/
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && focusedObject != null)
        {
            focusedObject = null;
            isFocused = false;
            Debug.Log("Focus to null (Aim at nothing)");
        }
        /*else if (focusedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            focusedObject.layer = 6;
            focusedObject = null;
            Debug.Log("Unfocused Object");
        }
        //Debug.Log(displayPercent + "%");
        if (focusedObject == null)
        {
            paintSplatter.ChangePaintingLayer(6);
            Debug.Log("Painting layer = 6");
        }
        else
        {
            paintSplatter.ChangePaintingLayer(7);
            Debug.Log("Painting layer = 7");
        }*/
    }
}
