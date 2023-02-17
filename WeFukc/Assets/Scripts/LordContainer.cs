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
        FindObjectOfType<BlockchainManager>().Button_MintLicense(this, BigInteger.Parse(licenseAmountInput.text));
    }

    public void MintSuccess()
    {
        // TODO
    }
    public void Button_DAOvote(bool _isApproving)
    {
        FindObjectOfType<BlockchainManager>().Button_LordDAOvote(this, BigInteger.Parse(proposalIDInput.text), _isApproving);
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
