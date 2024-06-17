using TMPro;
using UnityEngine;

public class BuyableItem : MonoBehaviour
{
    public string Name;
    public double Cost;
    public bool MultipleBuyable;
    public bool Bought;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        nameText.text = Name;
        costText.text = Cost.ToString();
    }

    public void Buy()
    {
        if (!MultipleBuyable)
        {
            costText.text = "Bought!";
            Bought = true;
        }

        if (Name == "Minigun")
            PlayerUpgrades.Minigun = true;
        else if (Name == "Lazer Spray")
            PlayerUpgrades.Lazer = true;
        else if (Name == "Wide Spray")
            PlayerUpgrades.WiderSpray = true;
        else if (Name == "Tight Spray")
            PlayerUpgrades.SmallerSpray = true;
        else if (Name == "Eclipse Brush")
            PlayerUpgrades.EclipseBrush = true;
        else if (Name == "Square Brush")
            PlayerUpgrades.SquareBrush = true;
        else if (Name == "Triangle Brush")
            PlayerUpgrades.TriangleBrush = true;
        else if (Name == "Grenade")
            PlayerUpgrades.Grenades++;
    }
}
