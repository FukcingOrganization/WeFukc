using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDetailsPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI armorText;

                                    // Name,     Damage, Range,  Speed,  Armor
    string[] batProps = new string[5] { "Bat",     "5",  "0",    "7",    "0"};
    string[] knifeProps = new string[5] { "Knife", "10", "0",    "7",    "0"};
    string[] swordProps = new string[5] { "Sword", "15", "0",    "3",    "0"};
    string[] gunProps = new string[5] { "Gun",     "10", "15",   "5",    "0"};

    public void OpenPopup(string itemName) {
        gameObject.SetActive(true);

        string[] item = new string[5];

        if (itemName == "Bat") item = batProps;
        if (itemName == "Knife") item = knifeProps;
        if (itemName == "Sword") item = swordProps;
        if (itemName == "Gun") item = gunProps;

        nameText.text = item[0];
        damageText.text = item[1];
        rangeText.text = item[2];
        speedText.text = item[3];
        armorText.text = item[4];
    }
}
