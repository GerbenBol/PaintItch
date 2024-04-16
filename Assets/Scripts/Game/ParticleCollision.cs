using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] GameObject tempPaintBlock;

    private Vector3 collisionLocation;
    private Quaternion rotation;


    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void Update()
    {
        
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    if (other.CompareTag("Paintable"))
    //    {
    //        collisionLocation = transform.position;
    //        GameObject paint = Instantiate(tempPaintBlock, collisionLocation, transform.rotation);
    //    }
    //}
    //private void OnParticleCollision(GameObject other)
    //{
    //    if (!other.CompareTag("Player"))
    //    {
    //        ContactPoint _contact = other.GetComponent<ContactPoint>();
    //        Quaternion _rotation = Quaternion.FromToRotation(Vector3.up, _contact.normal);
    //        Vector3 _position = _contact.point;
    //        Instantiate(tempPaintBlock, _position, _rotation);
    //    }
    //}

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.CompareTag("Paintable"))
            {
                Vector3 pos = collisionEvents[i].intersection;
                Instantiate(tempPaintBlock, pos, rotation);
            }
            i++;
        }
    }
}
