using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPainting : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private Texture texture;

    private int activeColor = 0;

    private void Start()
    {
        colors = new() { Color.red, Color.cyan, Color.blue }; 
    }

    private void Update()
    {
        // Shooting
        if (Input.GetMouseButton(0))
            Shoot();
        else if (shootParticle.isPlaying)
            shootParticle.Stop();

        // Color scroll
        if (Input.mouseScrollDelta.y > 0)
            NextColor();
        else if (Input.mouseScrollDelta.y < 0)
            PrevColor();
    }

    private void Shoot()
    {
        if (shootParticle.isStopped)
            shootParticle.Play();

        if (Physics.Raycast(startPoint.position, gun.transform.forward, out RaycastHit hit, 10) && hit.transform.gameObject.layer == 6)
        {
            PaintableObject obj = hit.transform.GetComponent<PaintableObject>();
            Vector2 textureCoord = hit.textureCoord;

            Texture tex = obj.ColorTexture;
            int pixelX = (int)(textureCoord.x * tex.width);
            int pixelY = (int)(textureCoord.y * tex.height);
            Vector2Int paintPosition = new(pixelX, pixelY);

            obj.ChangeTexture(paintPosition, colors[activeColor]);
        }
    }

    private void NextColor()
    {
        activeColor++;

        if (activeColor > colors.Count - 1)
            activeColor = 0;
    }

    private void PrevColor()
    {
        activeColor--;

        if (activeColor < 0)
            activeColor = colors.Count - 1;
    }
}
