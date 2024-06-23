using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTP : MonoBehaviour
{
    [SerializeField] private Transform tpLocation;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = tpLocation.position;
    }
}
