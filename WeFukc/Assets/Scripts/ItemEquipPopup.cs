using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemEquipPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TMP_InputField input;

    public void OpenPopup(string itemName)
    {
        gameObject.SetActive(true);

        text.text = "Enter the amount of " + itemName + " you want to equip";
    }
}
