using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSystem : MonoBehaviour
{
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform mainCam;
    [SerializeField] private float minimumDistance;

    private readonly float maxTimer = .05f;
    private float timer = .0f;

    private void Update()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else if (Input.GetMouseButton(0))
        {
            // Check if we're too close to an object
            if (!Physics.Raycast(mainCam.position, mainCam.forward, minimumDistance))
            {
                Shoot();
                timer = .0f;
            }
        }
    }

    private void Shoot()
    {
        // Create splatter
        GameObject splatterObject = Instantiate(paintPrefab, spawnPoint.position, spawnPoint.rotation);
        PaintSplatter splatter = splatterObject.GetComponent<PaintSplatter>();

        // Determine starting velocity and scale (I use System.Random because it's so much better than Unity's Random)
        System.Random rand = new();
        int randomForceX = rand.Next(-50, 50);
        int randomForceY = rand.Next(-50, 50);
        int randomForceZ = rand.Next(300, 600);
        float randomScale = (float)rand.Next(15, 40) / 100;

        // Set the starting velocity and scale and send the splatter flying
        Vector3 force = new(randomForceX, randomForceY, randomForceZ);
        Vector3 scale = new(randomScale, randomScale, randomScale);
        splatter.Send(Color.red, force, scale);
    }
}
