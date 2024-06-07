using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private Level currentFocused;

    void Update()
    {
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hitInfo, 2))
        {
            GameObject hitObj = hitInfo.transform.gameObject;
            Level lvl;

            if (currentFocused != null)
                lvl = currentFocused;
            else
                lvl = GameObject.Find("level0").GetComponent<Level>();

            if (currentFocused == null || hitObj != currentFocused.gameObject)
            {
                lvl = hitObj.GetComponent<Level>();

                if (currentFocused != null)
                    currentFocused.OriginalSize();
            }

            currentFocused = lvl;
            lvl.Enlarge();

            if (Input.GetKeyDown(KeyCode.E))
                GameManagerScript.OpenLevel(lvl.LevelID);
        }
    }
}
