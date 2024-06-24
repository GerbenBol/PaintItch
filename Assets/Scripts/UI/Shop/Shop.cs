using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static double Money = 50;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject mainShop;
    [SerializeField] private GameObject brushesShop;
    [SerializeField] private GameObject weaponsShop;
    [SerializeField] private GameObject upgradesShop;
    private static TextMeshProUGUI mText;
    private static GameObject gO;

    private void Start()
    {
        mText = moneyText;
        gO = gameObject;
        CloseShop();
    }

    private void Update()
    {
        if (gO.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) || gO.activeInHierarchy && Input.GetKeyDown(KeyCode.Tab))
            CloseShop();
    }

    public static void OpenShop()
    {
        GameManagerScript.InMenu = true;
        gO.SetActive(true);
        UpdateMoney();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gO.SetActive(false);
        GameManagerScript.InMenu = false;
    }

    public void BuyItem(BuyableItem item)
    {
        if (item.Cost <= Money && !item.Bought)
        {
            Money -= item.Cost;
            item.Buy();
            UpdateMoney();
        }
    }

    public void OpenMainShop(bool active)
    {
        mainShop.SetActive(active);
        brushesShop.SetActive(!active);
        weaponsShop.SetActive(!active);
        upgradesShop.SetActive(!active);
    }

    public void OpenBrushes(bool active)
    {
        brushesShop.SetActive(active);
        weaponsShop.SetActive(!active);
        upgradesShop.SetActive(!active);
        mainShop.SetActive(!active);
    }

    public void OpenWeapons(bool active)
    {
        weaponsShop.SetActive(active);
        brushesShop.SetActive(!active);
        upgradesShop.SetActive(!active);
        mainShop.SetActive(!active);
    }

    public void OpenUpgrades(bool active)
    {
        upgradesShop.SetActive(active);
        weaponsShop.SetActive(!active);
        brushesShop.SetActive(!active);
        mainShop.SetActive(!active);
    }

    private static void UpdateMoney()
    {
        mText.text = Money.ToString();
    }
}
