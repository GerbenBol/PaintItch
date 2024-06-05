using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceOpen : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 openRotation;
    private Vector3 closedPosition;

    private void Start()
    {
        closedPosition = transform.localPosition;
        GameManagerScript.LevelFences.Add(this);
    }

    public void Open()
    {
        transform.localPosition = openPosition;
        transform.Rotate(openRotation);
    }

    public void Close()
    {
        transform.localPosition = closedPosition;
        transform.Rotate(-openRotation);
    }
}
