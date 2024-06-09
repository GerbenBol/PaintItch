using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelBase;
    [SerializeField] private TextMeshProUGUI levelExtra;
    [SerializeField] private List<GameObject> cameras;

    private static bool openNextFrame = false;
    private static bool closeNextFrame = false;
    private static int activeLevel = 0;
    private static string activeName;
    private static double activePrice;
    private static string activeDesc;
    private static string activeExtra;
    private static GameObject thisObj;

    private void Start()
    {
        thisObj = gameObject;
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
            cameras[activeLevel - 1].SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public static void OpenQuest(string name, double price, string desc, string extra, int levelIndex)
    {
        thisObj.SetActive(true);
        openNextFrame = true;
        activeLevel = levelIndex;
        activeName = name;
        activePrice = price;
        activeDesc = desc;
        activeExtra = extra;
    }

    public static void CloseQuest()
    {
        closeNextFrame = true;
    }

    private void Open()
    {
        cameras[activeLevel - 1].SetActive(true);
        levelName.text = activeName;
        levelBase.text = activeDesc;
        levelExtra.text = activeExtra;
    }
}
