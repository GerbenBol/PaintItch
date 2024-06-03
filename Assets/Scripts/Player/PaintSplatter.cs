using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSplatter : MonoBehaviour
{
    public Color color;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private LayerMask paintableLayer;

    private int paintingLayer = 6;

    private void Start()
    {
        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Splatter"))
        {
            Vector3 collisionPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (other.gameObject.layer == paintingLayer)
                Paint(collisionPoint);

            Destroy(gameObject);
        }
    }

    public void Send(Color _color, Vector3 force, Vector3 scale)
    {
        color = _color;
        renderer.material.color = _color;
        rb.AddRelativeForce(force);
        transform.localScale = scale;
    }

    private void Paint(Vector3 collisionPoint)
    {
        Vector3 pos = transform.position - rb.velocity.normalized;
        Vector3 heading = collisionPoint - pos;
        float distance = heading.magnitude;
        Vector3 dir = heading / distance;
        RaycastHit[] hits = Physics.RaycastAll(pos, dir, distance + 1, paintableLayer, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
            if (hit.collider.gameObject.layer == paintingLayer)
            {
                PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                Vector2 textureCoord = hit.textureCoord;

                Texture tex = obj.MainTexture;
                int pixelX = (int)(textureCoord.x * tex.width);
                int pixelY = (int)(textureCoord.y * tex.height);
                Vector2Int paintPosition = new(pixelX, pixelY);
                obj.ChangeTexture(paintPosition, color);
                break;
            }
    }

    public void ChangePaintingLayer()
    {
        if (paintingLayer == 6)
            paintingLayer = 7;
        else if (paintingLayer == 7)
            paintingLayer = 6;
    }
}
