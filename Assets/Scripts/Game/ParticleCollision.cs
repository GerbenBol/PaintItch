using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private GameObject tempPaintBlock;

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
                Instantiate(tempPaintBlock, pos, rotation);
            }
            i++;
        }
    }
}
