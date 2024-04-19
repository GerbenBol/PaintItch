using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private GameObject tempPaintBlock;
    [SerializeField] private PlayerPainting paint;

    private Quaternion rotation;

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
                Physics.Raycast(pos - new Vector3(0, .1f), part.transform.forward, out RaycastHit hit, 1f);
                //Instantiate(tempPaintBlock, pos, rotation);
                PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
                Vector2 textureCoord = hit.textureCoord;

                Texture tex = obj.ColorTexture;
                int pixelX = (int)(textureCoord.x * tex.width);
                int pixelY = (int)(textureCoord.y * tex.height);
                Vector2Int paintPosition = new(pixelX, pixelY);

                obj.ChangeTexture(paintPosition, paint.Colors[paint.ActiveColor]);
            }
            i++;
        }
    }
}
