using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lord : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI idText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI numOfClansText;
    [SerializeField] TextMeshProUGUI numOfLicenseText;
    [SerializeField] TextMeshProUGUI collectedTaxesText;
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;

    public string name { get; set; }
    public string description { get; set; }
    public int id { get; set; }
    public int numOfClans { get; set; }
    public int numOfLicense { get; set; }
    public int collectedTaxes { get; set; }

    public void SetID(int id)
    {
        print("Set Lord ID:" + id);
        this.id = id;
        image.sprite = sprites[id - 1]; // lord ids starts from 1 | reduce to meet with array index
        idText.text = id.ToString();

        infoSet(id);
        print("Info set lord ID:" + id);

        BlockchainManager chainManager = FindObjectOfType<BlockchainManager>();

        // Get number of clans, license and collected taxes for this lord
        StartCoroutine(chainManager.LordNumberOfClansCall(this));
        StartCoroutine(chainManager.LordNumberOfLicenseCall(this));
        StartCoroutine(chainManager.LordCollectedTaxesCall(this));
    }

    public void ClanNumberSet(int numOfClans)
    {
        this.numOfClans = numOfClans;
        numOfClansText.text = numOfClans.ToString();
    }

    public void LicenseNumberSet(int numOfLicense)
    {
        this.numOfLicense = numOfLicense;
        numOfLicenseText.text = numOfLicense.ToString();
    }

    public void CollectedTaxSet(int collectedTaxes)
    {
        this.collectedTaxes = collectedTaxes;
        collectedTaxesText.text = collectedTaxes.ToString();
    }

    void infoSet(int id)
    {
        switch (id){
            case 1:
                this.name = nameText.text = "Lord ID: 1";
                this.description = descriptionText.text = "We are the Stick Lords!";
                break;

            default:
                this.name = nameText.text = "Lord ID: " + id.ToString();
                this.description = descriptionText.text = "We are the Stick Lords!";
                break;
        }

    }
}
