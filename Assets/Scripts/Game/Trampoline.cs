using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private bool beingHold;
    private Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact(GameObject trampHolder = null)
    {
        // Check of we vastgehouden worden & of we vastzitten aan een button
        if (trampHolder != null)
        {
            if (!beingHold)
                Pickup(trampHolder);
            else
                Drop();
        }
    }
    private void Pickup(GameObject trampHolder)
    {
        // Oppakken van de doos
        transform.SetParent(trampHolder.transform);
        transform.localPosition = new(0, 0, transform.localScale.z + 1);
        transform.rotation = Quaternion.identity;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.drag = 10;
        beingHold = true;
    }
    private void Drop()
    {
        // Laat doos vallen
        transform.parent = null;
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.drag = 1;
        beingHold = false;
    }
}
