using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxHeight;
    [SerializeField] private float moveY;
    [SerializeField] private int level;

    private bool goingUp = false;
    private SpriteRenderer arrow;
    private Color transparent, visible;

    private void Start()
    {
        GameManagerScript.AddArrow(level, gameObject);
        gameObject.SetActive(false);
        arrow = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transparent = new Color(0, 0, 0, 0);
        visible = arrow.color;
    }

    private void Update()
    {
        if (goingUp)
        {
            transform.localPosition += new Vector3(0, moveY * Time.deltaTime);

            if (transform.localPosition.y > minMaxHeight.y)
                goingUp = false;
        }
        else
        {
            transform.localPosition -= new Vector3(0, moveY * Time.deltaTime);

            if (transform.localPosition.y < minMaxHeight.x)
                goingUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            arrow.color = transparent;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            arrow.color = visible;
    }
}
