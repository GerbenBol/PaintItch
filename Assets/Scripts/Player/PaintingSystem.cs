using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingSystem : MonoBehaviour
{
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Shoot();
    }

    private void Shoot()
    {
        GameObject splatterObject = Instantiate(paintPrefab, spawnPoint.position, spawnPoint.rotation);
        PaintSplatter splatter = splatterObject.GetComponent<PaintSplatter>();
        Vector3 force = new(0, 0, 1000);
        Vector3 scale = new(1, 1, 1);
        Debug.Log(3.402823 * Mathf.Pow(10, 38));
        splatter.Send(Color.red, force, scale);
    }
}
