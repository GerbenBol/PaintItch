using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceOpen : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Vector3 openRotation;

    private void Start()
    {
        GameManagerScript.AddFence(level, this);
    }

    public void Open()
    {
        transform.Rotate(openRotation);
    }

    public void Close()
    {
        transform.Rotate(-openRotation);
    }
}
