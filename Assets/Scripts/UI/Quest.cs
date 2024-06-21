using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelBase;
    [SerializeField] private TextMeshProUGUI levelExtra;
    [SerializeField] private TextMeshProUGUI levelButton;
    [SerializeField] private Image levelImage;
    [SerializeField] private List<GameObject> cameras;

    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI extraText;

    private static bool openNextFrame = false;
    private static bool closeNextFrame = false;
    private static int lastLevel = 0;
    private static int activeLevel = 0;
    private static string activeName;
    private static string activeButtonText;
    private static double activePrice;
    private static string activeDesc;
    private static string activeExtra;
    private static Color activeColor;
    private static GameObject thisObj;

    private static TextMeshProUGUI objective;
    private static TextMeshProUGUI extra;

    private void Start()
    {
        thisObj = gameObject;
        objective = objectiveText;
        extra = extraText;
        thisObj.SetActive(false);
    }

    private void Update()
    {
        if (openNextFrame)
        {
            Open();
            openNextFrame = false;
        }
        else if (closeNextFrame)
        {
            closeNextFrame = false;
            cameras[lastLevel - 1].SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public static void OpenQuest(string name, double price, string desc, string extra, int levelIndex, Color color)
    {
        if (activeLevel != levelIndex)
            activeButtonText = "Accept Quest - [E]";
        else
            activeButtonText = "Quest Accepted!";

        thisObj.SetActive(true);
        openNextFrame = true;
        lastLevel = levelIndex;
        activeName = name;
        activePrice = price;
        activeDesc = desc;
        activeExtra = extra;
        activeColor = color;
    }

    public static void CloseQuest()
    {
        closeNextFrame = true;
    }

    public static void StartQuest(int levelID, string name, string _extra, Color color)
    {
        activeButtonText = "Quest Accepted!";
        activeLevel = levelID;
        objective.text = name;
        extra.text = _extra;
        extra.color = color;
        objective.gameObject.SetActive(true);
        extra.gameObject.SetActive(true);
        openNextFrame = true;
    }

    private void Open()
    {
        cameras[lastLevel - 1].SetActive(true);
        levelName.text = $"{activeName} (${activePrice})";
        levelBase.text = activeDesc;
        levelExtra.text = activeExtra;
        levelButton.text = activeButtonText;
        levelImage.color = activeColor;
    }
}
