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
    }
}
