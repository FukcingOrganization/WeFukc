using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;

public class LordContainer : MonoBehaviour
{
    [SerializeField] TMP_InputField proposalIDInput;
    [SerializeField] TMP_InputField licenseAmountInput;
    [SerializeField] TextMeshProUGUI mintCostText;

    public BigInteger lordID { get; set; }

    public void Button_MintLicense()
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().
            MintLicenseCall(this, GetComponent<Lord>(), BigInteger.Parse(licenseAmountInput.text
        )));
    }

    public void MintSuccess()
    {
        // TODO
    }
    public void Button_DAOvote(bool _isApproving)
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().
            LordDAOvoteCall(this, BigInteger.Parse(proposalIDInput.text), _isApproving
        ));

        // Reset the DAO info so we can see updated version after voting
        FindObjectOfType<BlockchainReader>().DAOinfoSet = false;
    }

    public void VoteSuccess()
    {
        // TODO
    }

    public void UpdateLordCostText(int _cost)
    {
        mintCostText.text = "Mint Cost: " + _cost + " STICK";
    }
}
