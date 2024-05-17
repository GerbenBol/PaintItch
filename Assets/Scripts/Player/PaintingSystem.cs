using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSystem : MonoBehaviour
{
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private Transform spawnPoint;

    private readonly float maxTimer = .1f;
    private float timer = .0f;

    private void Update()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
                timer = .0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject splatterObject = Instantiate(paintPrefab, spawnPoint.position, spawnPoint.rotation);
        PaintSplatter splatter = splatterObject.GetComponent<PaintSplatter>();
        Vector3 force = new(0, 0, 1000);
        Vector3 scale = new(1, 1, 1);
        splatter.Send(Color.red, force, scale);
    }
}
