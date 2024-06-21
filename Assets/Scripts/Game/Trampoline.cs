using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private bool beingHeld;
    private bool pickedUp;
    private bool changed;

    private Rigidbody rb;
    private BoxCollider topBox;
    private BoxCollider botBox;
    private MeshCollider mesh;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        topBox = GetComponent<BoxCollider>();
        botBox = transform.GetChild(0).GetComponent<BoxCollider>();
        mesh = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (!changed)
        {
            StartCoroutine(nameof(ChangeTrampTag));
        }

        // Verander positie van tramp als we niet op de juiste plek zijn
        if (beingHeld && Vector3.Distance(transform.position, transform.parent.position) > .1f)
        {
            Vector3 moveDir = (transform.parent.position - transform.position);
            rb.AddForce(moveDir * 100);
        }
        if (beingHeld && Input.GetKeyDown(KeyCode.E) && !pickedUp)
            Drop();
        pickedUp = false;

        if (rb != null && rb.velocity == new Vector3(0, 0, 0) && !beingHeld)
        {
            Destroy(rb);
            mesh.enabled = true;
            topBox.enabled = false;
            botBox.enabled = false;
        }
    }

    public void Interact(GameObject trampHolder = null)
    {
        if (trampHolder != null)
        {
            if (!beingHeld)
                Pickup(trampHolder);
        }
    }
    private void Pickup(GameObject trampHolder)
    {
        // Oppakken van de tramp
        topBox.enabled = true;
        botBox.enabled = true;
        if (rb != null)
            rb = GetComponent<Rigidbody>();
        else
            rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        mesh.enabled = false;
        transform.SetParent(trampHolder.transform);
        transform.localPosition = new(0, 0, transform.localScale.z + 1);
        transform.rotation = Quaternion.identity;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.drag = 10;
        beingHeld = true;
        pickedUp = true;
    }
    private void Drop()
    {
        // Laat tramp vallen
        transform.parent = null;
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.drag = 1;
        beingHeld = false;
    }

    private IEnumerator ChangeTrampTag()
    {
        yield return new WaitForSeconds(1);
        gameObject.tag = "Trampoline";
        changed = true;
    }
}
