using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSplatter : MonoBehaviour
{
    public Color color;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private LayerMask paintableLayer;

    private void Start()
    {
        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Splatter"))
        {
            Vector3 collisionPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (other.gameObject.layer == 6)
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
            if (hit.collider.gameObject.layer == 6 && HoveredObjectPercentage.focusedObject == HoveredObjectPercentage.nonFocusObj)
            {
                Debug.Log("Free Painting");
                PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                Vector2 textureCoord = hit.textureCoord;

                Texture tex = obj.MainTexture;
                int pixelX = (int)(textureCoord.x * tex.width);
                int pixelY = (int)(textureCoord.y * tex.height);
                Vector2Int paintPosition = new(pixelX, pixelY);
                obj.ChangeTexture(paintPosition, color);
                break;
            }
            else if (hit.collider.gameObject == HoveredObjectPercentage.focusedObject)
            {
                Debug.Log("Focused Painting");
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
}
