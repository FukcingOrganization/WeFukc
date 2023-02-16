using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossContainer : MonoBehaviour
{
    [SerializeField] TMP_InputField levelInput;
    [SerializeField] TMP_InputField fundAmountInput;
    [SerializeField] TMP_InputField defundAmountInput;
    [SerializeField] GameObject fundingPopup;
    [SerializeField] GameObject defundingPopup;

    public Image image { get; set; }
    public string name { get; set; }
    public BigInteger id { get; set; }
    public BigInteger selectedLevel { get; set; }
    public int beatCount { get; set; }
    public string description { get; set; }

    public void Button_Nominate()
    {
        // Check whether the boss is already nominated for this level or not

        // if nominated, show error message

        // if not, open fund popup
    }

    public void Button_FundNominate()
    {
        FindObjectOfType<BlockchainManager>().Button_RoundFund(
            true,
            BigInteger.Parse(levelInput.text),
            id,
            BigInteger.Parse(fundAmountInput.text)
        );
    }
    public void Button_FundLevel()
    {
        FindObjectOfType<BlockchainManager>().Button_RoundFund(
            true,
            selectedLevel,
            id,
            BigInteger.Parse(fundAmountInput.text)
        );
    }
    public void Button_DefundLevel()
    {
        FindObjectOfType<BlockchainManager>().Button_RoundFund(
            false,
            selectedLevel,
            id,
            BigInteger.Parse(defundAmountInput.text)
        );
    }
}
