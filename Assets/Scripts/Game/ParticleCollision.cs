using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private PlayerPainting paint;
    [SerializeField] private LayerMask paintableMask;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    //public HoveredObjectPercentage hoveredObject;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        var mainPart = part.main;
        mainPart.startColor = Color.white;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.layer == 6)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 heading = pos - part.transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                if (Physics.Raycast(pos, direction, out RaycastHit hit, 1f, paintableMask))
                {
                    //if (hoveredObject.focusedObject == null)
                    //{
                        Debug.Log("Free Painting");
                        PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                        Vector2 textureCoord = hit.textureCoord;

                        Texture tex = obj.MainTexture;
                        int pixelX = (int)(textureCoord.x * tex.width);
                        int pixelY = (int)(textureCoord.y * tex.height);
                        Vector2Int paintPosition = new(pixelX, pixelY);
                        obj.ChangeTexture(paintPosition, paint.Colors[paint.ActiveColor]);
                    //}
                    //else if (hit.collider.gameObject == hoveredObject.focusedObject)
                    //{
                    //    Debug.Log("Focused Painting");
                    //    PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                    //    Vector2 textureCoord = hit.textureCoord;

                    //    Texture tex = obj.MainTexture;
                    //    int pixelX = (int)(textureCoord.x * tex.width);
                    //    int pixelY = (int)(textureCoord.y * tex.height);
                    //    Vector2Int paintPosition = new(pixelX, pixelY);
                    //    obj.ChangeTexture(paintPosition, paint.Colors[paint.ActiveColor]);
                    //}
                }
            }
            i++;
        }
    }
}
