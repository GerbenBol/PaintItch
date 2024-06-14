using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Nade : MonoBehaviour
{
    [SerializeField] private List<GameObject> paintPrefabs;

    private Color color;
    private Quaternion newRotation;
    private Quaternion originalRotation;

    private void OnDestroy()
    {
        transform.rotation = originalRotation;
        transform.Rotate(new(-90, 0));
        transform.position += new Vector3(0, .1f);
        Explode();
    }

    public void SetNade(Color c, Quaternion rot)
    {
        color = c;
        newRotation = rot;
        originalRotation = transform.rotation;
        transform.rotation = newRotation;
        GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 600);
        Destroy(gameObject, 2);
    }

    private void Explode()
    {
        System.Random rand = new();
        int splatters = rand.Next(125, 200);

        while (splatters > 0)
        {
            CreateSplatter();
            splatters--;
        }
    }

    private void CreateSplatter()
    {
        System.Random rand = new();
        int prefabIndex = rand.Next(paintPrefabs.Count);

        // Create splatter
        GameObject splatterObj = Instantiate(paintPrefabs[prefabIndex], transform.position, transform.rotation);
        PaintSplatter splatter = splatterObj.GetComponent<PaintSplatter>();

        // Determine starting velocity and scale
        int randomForceX = rand.Next(-200, 200);
        int randomForceY = rand.Next(-200, 200);
        int randomForceZ = rand.Next(100, 450);
        float randomScale = (float)rand.Next(15, 40) / 100;

        // Set variables
        Vector3 force = new(randomForceX, randomForceY, randomForceZ);
        Vector3 scale = new(randomScale, randomScale, randomScale);

        splatter.Send(color, force, scale);
    }
}
