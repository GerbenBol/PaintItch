using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private Level currentFocused;
    private bool returnedToSize = false;



    private bool holdingBox;
    private bool firstPickup;
    private bool firstDrop;
    private string interactableName;
    [SerializeField] private GameObject trampTxt;

    private void Update()
    {
        // Open the shop
        if (Input.GetKeyDown(KeyCode.Tab))
            Shop.OpenShop();

        Level lvl;

        if (currentFocused != null)
            lvl = currentFocused;
        else
            lvl = GameObject.Find("Level0").GetComponent<Level>();

        // Raycast recht voor ons uit
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hitInfo, 5))
        {
            GameObject hitObj = hitInfo.transform.gameObject;

            // Check of we naar een quest kijken (voorkomt error messages)
            if (hitObj.name[..5] == "Level")
            {
                if (returnedToSize)
                {
                    if (currentFocused == null || hitObj != currentFocused.gameObject)
                        lvl = hitObj.GetComponent<Level>();

                    currentFocused = lvl;
                    lvl.Enlarge();
                    returnedToSize = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                    GameManagerScript.OpenLevel(lvl.LevelID);
            }
            else if (!returnedToSize)
            {
                returnedToSize = true;
                lvl.OriginalSize();
            }
        }
        else if (!returnedToSize)
        {
            returnedToSize = true;
            lvl.OriginalSize();
        }
    }

    // Temp tramp pickup system
    /*private void OnPickUp()
    {
        if (interactableName == "P_Trampo" || interactableName == "Underside")
        {
            interactable.Interact(boxHolder);
            holdingBox = !holdingBox;

            if (firstPickup)
            {
                trampTxt.SetActive(true);
                firstPickup = false;
            }
            else if (firstDrop)
            {
                trampTxt.SetActive(false);
                firstDrop = false;
            }
        }
    }*/
}
