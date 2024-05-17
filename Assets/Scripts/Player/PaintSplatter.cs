using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSplatter : MonoBehaviour
{
    public Color color;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private new Renderer renderer;

    public void Send(Color _color, Vector3 force, Vector3 scale)
    {
        color = _color;
        renderer.material.color = _color;
        rb.AddRelativeForce(force);
        transform.localScale = scale;
    }
}
