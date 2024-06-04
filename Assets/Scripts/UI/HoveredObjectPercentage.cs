using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoveredObjectPercentage : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    private string hoveredObjectName;
    private GameObject hoveredObject;
    private GameObject focusedObject;
    [SerializeField] private LayerMask layerMask;
    private float displayPercent;
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private LayerMask focussedLayerMask;
    [SerializeField] private LayerMask unFocussedLayerMask;

    private PaintSplatter paintSplatter;
    void Start()
    {
        paintSplatter = paintPrefab.GetComponent<PaintSplatter>();
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
                if (focusedObject != null && focusedObject.layer == focussedLayerMask)
                {
                    focusedObject.layer = unFocussedLayerMask;
                    focusedObject = null;
                    Debug.Log("Unfocused Object");
                }
                if (hoveredObject != focusedObject)
                {
                    focusedObject = hoveredObject;
                    focusedObject.layer = focussedLayerMask;
                    Debug.Log("Focused Object");
                }
            }
        }
        else if (focusedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            focusedObject.layer = unFocussedLayerMask;
            focusedObject = null;
            Debug.Log("Unfocused Object");
        }
        //Debug.Log(displayPercent + "%");
        if (focusedObject == null)
        {
            paintSplatter.ChangePaintingLayer(unFocussedLayerMask);
            Debug.Log("Painting layer = 6");
        }
        else
        {
            paintSplatter.ChangePaintingLayer(focussedLayerMask);
            Debug.Log("Painting layer = 7");
        }
    }
}
