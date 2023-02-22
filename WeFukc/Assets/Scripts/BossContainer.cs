using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossContainer : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI idText;
    [SerializeField] TextMeshProUGUI rektText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [Header("Candidate Info")]
    [SerializeField] TextMeshProUGUI votesText;
    [SerializeField] TextMeshProUGUI allFundsText;
    [SerializeField] TextMeshProUGUI userFundsText;
    [SerializeField] TextMeshProUGUI profitText;

    [Header("Inputs")]
    [SerializeField] TMP_InputField levelInput;
    [SerializeField] TMP_InputField fundAmountInput;
    [SerializeField] TMP_InputField defundAmountInput;

    [Header("Objects")]
    [SerializeField] GameObject fundingPopup;
    [SerializeField] GameObject defundingPopup;
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;

    public string name { get; set; }
    public int id { get; set; }
    public int selectedLevel { get; set; }
    public int rektCount { get; set; }
    public string description { get; set; }
    public double candidateFunds { get; set; }

    /*
     *      TO-DO
     *  - Complete Nominate Button
     *  - Complete 
     */


    public void BossListIDset(int id)
    {
        this.id = id;
        idText.text = id.ToString();
        
        image.sprite = sprites[id - 1]; // lord ids starts from 1 | reduce to meet with array index

        BossListInfoSet(id);

        // Get number of clans, license and collected taxes for this lord
        StartCoroutine(FindObjectOfType<BlockchainManager>().GetBossRektCall(this));
    }

    public void CandidateIDset(int id, int selectedLevel, int round)
    {
        this.id = id;
        idText.text = id.ToString();
        
        this.selectedLevel = selectedLevel;
        image.sprite = sprites[id - 1]; // lord ids starts from 1 | reduce to meet with array index

        CandidateInfoSet(id);

        // Get candidate information
        StartCoroutine(FindObjectOfType<BlockchainManager>().GetCandidateFunds((BigInteger)round, this));
        StartCoroutine(FindObjectOfType<BlockchainManager>().GetElectionUserFunds((BigInteger)round, this));
        //StartCoroutine(FindObjectOfType<BlockchainManager>(). == VOTES == (this));
    }

    public void Button_Nominate()
    {
        // Check whether the boss is already nominated for this level or not

        // if nominated, show error message

        // if not, open fund popup
    }

    public void Button_FundNominate()
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().
            FundBossCall(BigInteger.Parse(levelInput.text), (BigInteger)id, BigInteger.Parse(fundAmountInput.text)
        ));
    }
    public void Button_FundLevel()
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().
            FundBossCall(selectedLevel, (BigInteger)id, BigInteger.Parse(fundAmountInput.text)
        ));
    }
    public void Button_DefundLevel()
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().
            DefundBossCall(selectedLevel, (BigInteger)id, BigInteger.Parse(defundAmountInput.text)
        ));
    }

    public void OnBossRektReturn(int numOfRekt)
    {
        this.rektCount = numOfRekt;
        rektText.text = numOfRekt.ToString();
    }

    public void OnVotesReturn(int votes)
    {
        votesText.text = votes.ToString();
    }
    public void OnAllFundsReturn(double candidateFunds)
    {
        allFundsText.text = candidateFunds.ToString();
        this.candidateFunds = candidateFunds;
    }
    public void OnUserFundsReturn(double userFunds)
    {
        userFundsText.text = userFunds.ToString();

        BlockchainReader chainReader = FindObjectOfType<BlockchainReader>();

        // Calculate profit
        double totalProfit = chainReader.backerRewards[selectedLevel] - candidateFunds;
        double userContribution = (userFunds / candidateFunds); // 0 -> 1 in all funds
        double userReward = totalProfit * userContribution;     // how much user will get
        double userProfit = userReward - userFunds;             // how much will actually profit
        double userProfitPercentage = userProfit / userFunds;   // in percantage term

        profitText.text = (userProfitPercentage * 100).ToString() + "% (" + 
            (userProfitPercentage + 1).ToString() + "x)"
        ;
    }

    void BossListInfoSet(int id)
    {
        switch (id)
        {
            case 1:
                this.name = nameText.text = "Boss Name #1";
                this.description = descriptionText.text = "We are the Stick Lords!";
                break;

            default:
                this.name = nameText.text = "Boss Name #" + id.ToString();
                this.description = descriptionText.text = "We are the Stick Lords!";
                break;
        }

    }

    void CandidateInfoSet(int id)
    {
        switch (id)
        {
            case 1:
                this.name = nameText.text = "Boss Name #1";
                break;

            default:
                this.name = nameText.text = "Boss Name #" + id.ToString();
                break;
        }

    }
}
