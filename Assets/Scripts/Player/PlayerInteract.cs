using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private Level currentFocused;
    private bool returnedToSize = false;

    private bool holdingTramp;
    private bool firstPickup;
    private bool firstDrop;
    [SerializeField] private GameObject tramp;
    [SerializeField] private GameObject trampHolder;
    [SerializeField] private GameObject trampTxt;
    [SerializeField] private Trampoline trampoline;

    private void Update()
    {
        // Open the shop
        if (Input.GetKeyDown(KeyCode.Tab) && !GameManagerScript.InMenu)
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
            else if (hitObj.name == "P_Trampo" || hitObj.name == "Underside")
            {
                if (Input.GetKeyDown(KeyCode.E))
                    OnPickUp();
            }
            else if (hitObj.name == "LeaveBus" && Input.GetKeyDown(KeyCode.E))
                GameManagerScript.StartFinalCamera();
        }
        else if (!returnedToSize)
        {
            returnedToSize = true;
            lvl.OriginalSize();
        }
    }

    // Tramp pickup system
    private void OnPickUp()
    {
        trampoline.Interact(trampHolder);
        holdingTramp = !holdingTramp;

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
}
