using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private PlayerPainting paint;
    [SerializeField] private LayerMask paintableMask;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
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
                    PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                    Vector2 textureCoord = hit.textureCoord;

                    Texture tex = obj.MainTexture;
                    int pixelX = (int)(textureCoord.x * tex.width);
                    int pixelY = (int)(textureCoord.y * tex.height);
                    Vector2Int paintPosition = new(pixelX, pixelY);
                    obj.ChangeTexture(paintPosition, paint.Colors[paint.ActiveColor]);
                }
            }
            i++;
        }
    }
}
