using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nade : MonoBehaviour
{
    public Vector3 Direction;

    private void Start()
    {
        //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 700);
        Destroy(gameObject, 5);
    }

    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        Debug.Log("boom");
    }
}
