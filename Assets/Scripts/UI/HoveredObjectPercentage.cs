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
                if (focusedObject != null && focusedObject.layer == 7)
                {
                    focusedObject.layer = 6;
                    focusedObject = null;
                    Debug.Log("Unfocused Object");
                }
                if (focusedObject != hoveredObject)
                {
                    focusedObject = hoveredObject;
                    //paintSplatter.ChangePaintingLayer();
                    focusedObject.layer = 7;
                    Debug.Log("Focused Object");
                }
            }
        }
        else if (focusedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            focusedObject.layer = 6;
            focusedObject = null;
            Debug.Log("Unfocused Object");
        }
        //Debug.Log(displayPercent + "%");
        //if (focusedObject = null)
            //paintSplatter.ChangePaintingLayer();
    }
}
