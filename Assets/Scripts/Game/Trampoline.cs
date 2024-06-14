using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public static bool Locked;
    private bool beingHold;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void Interact(GameObject boxHolder = null)
    {
        // Check of we vastgehouden worden & of we vastzitten aan een button
        if (boxHolder != null && !Locked)
        {
            if (!beingHold)
                Pickup(boxHolder);
            else
                Drop();
        }
    }
    private static void Pickup(GameObject boxHolder)
    {
        // Oppakken van de doos
        transform.SetParent(boxHolder.transform);
        transform.localPosition = new(0, 0, transform.localScale.z + 1);
        transform.rotation = Quaternion.identity;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.drag = 10;
        beingHold = true;
    }
    private static void Drop()
    {
        // Laat doos vallen
        transform.parent = null;
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.drag = 1;
        beingHold = false;

        // Turn off lights
        LevelManager.Levels[level].SearchLights(false, myMatName);
    }
}
