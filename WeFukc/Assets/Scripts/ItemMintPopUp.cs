using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemMintPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;

    public void OpenPopup(string itemName)
    {
        gameObject.SetActive(true);

        nameText.text = itemName;
        costText.text = "100 FUKC($1)"; // Test
    }
}
