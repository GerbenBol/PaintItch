using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    void Update()
    {
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hitInfo, 5) && Input.GetKeyDown(KeyCode.E))
        {
            GameObject hitObj = hitInfo.transform.gameObject;

            if (hitObj.name == "level0")
                GameManagerScript.OpenLevel(0);
            else if (hitObj.name == "level1")
                GameManagerScript.OpenLevel(1);
        }
    }
}
