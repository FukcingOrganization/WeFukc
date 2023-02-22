using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class BlockchainReader : MonoBehaviour
{
    public static BlockchainReader instance;

    // GENERAL
    [Header("General")]
    [SerializeField] TextMeshProUGUI[] addressTexts;
    [SerializeField] TextMeshProUGUI[] tokenBalanceTexts;
    [SerializeField] TextMeshProUGUI[] daoBalanceTexts;
    [SerializeField] TextMeshProUGUI[] clanNameTexts;

    // CLAN
    [Header("Clan")]
    [SerializeField] TextMeshProUGUI clanInfoNameText;        // Clan Info
    [SerializeField] TextMeshProUGUI clanInfoIDText;
    [SerializeField] TextMeshProUGUI clanInfoMottoText;
    [SerializeField] TextMeshProUGUI clanInfoDescriptionText;
    [SerializeField] TextMeshProUGUI clanPointText;
    [SerializeField] TextMeshProUGUI rewardShareText;
    [SerializeField] TextMeshProUGUI walletClanPoints;
    [SerializeField] TextMeshProUGUI walletRewardShare;
    [SerializeField] Member memberPrefab;
    [SerializeField] GameObject memberPanel;

    // ITEM
    [Header("Item")]
    [SerializeField] TextMeshProUGUI[] itemBalanceTexts;
    [SerializeField] GameObject[] approveButtons;

    // LORD
    [Header("Lord")]
    [SerializeField] Lord lordPrefab;
    [SerializeField] GameObject lordPanel;

    // ELECTION
    [Header("Election")]
    [SerializeField] BossContainer bossPrefab;
    [SerializeField] GameObject bossListPanel;


    // Variables
    BlockchainManager chainManager;

    [HideInInspector]
    public Clan walletClan = new Clan();
    [HideInInspector]
    public Clan displayClan = new Clan();
    [HideInInspector]
    public int walletMemberPoints;
    string walletAddress;
    int numOfLords;

    bool walletInfoSet;
    bool itemInfoSet;
    bool lordInfoSet;

    bool itemMintAllowance;
    int roundTotalClanReward;

    /* BlockchainReader Script:
     * 
                         * at the Begninning
                         * --> Read token and DAO balance
                         * --> Get the clan of wallet
                         * --> If wallet changes, update all info
                         * 
                         * at Profile/Items Canvas
                         * --> Check if any allowance needed for mints
                         * --> Get Owned Items
                         * 
                         * at Profile/Lord Canvas
                         * --> Get Owned Lords and their infos
                         * 
                         * at Profile/Clan Canvas
                         * --> Get current clan's info
                         * 
     * at Election Canvas
     * --> Get Boss Supply and add existing bosses to the boss list
     * --> Get current round's backer rewards
     * 
     * at DAO canvas
     * --> Get all active proposals
     * --> Get last 10 passed/rejected proposals
     */

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        chainManager = GetComponent<BlockchainManager>();
    }
    void Update()
    {
        if (chainManager == null) { chainManager = FindObjectOfType<BlockchainManager>(); }
    }

    /**
     *  MAKE BUTTON CALLS, STARTCOUROUTINE CALL
     *  
     */

    // Basic Info Display
    public void OnWalletChange(string address)
    {
        // if info already set and request comes for the same address, then skip it!
        if (walletInfoSet && walletAddress == address) { return; }

        walletAddress = address;

        foreach (TextMeshProUGUI text in addressTexts)
        {
            text.text = address.Substring(0, 6) + "...." + address.Substring(38, 4);
        }
        print("Wallet Texts are set!: " + address);

        StartCoroutine(chainManager.WalletClanCall());
        StartCoroutine(chainManager.TokenBalanceOfCall());
        StartCoroutine(chainManager.DAOBalanceOfCall());

        walletInfoSet = true;
    }
    public void DisplayTokenBalance(double balance)
    {
        foreach (var text in tokenBalanceTexts)
        {
            text.text = ((int)balance).ToString() + " STICK";
        }
    }
    public void DisplayDAOBalance(double balance)
    {
        foreach (var text in daoBalanceTexts)
        {
            text.text = ((int)balance).ToString() + " STICK";
        }
    }
    
    // Check Item Balance and Allowance
    public void Button_ItemCheckBalance()
    {
        // if info already set, then skip it!
        if (itemInfoSet) { return; }

        chainManager.Button_ItemBalanceOfBatch(
            new List<string> { walletAddress }, new List<BigInteger> { 1, 2, 3, 4, 5 }
        );

        itemInfoSet = true;
    }
    public void WriteItemMintAllowance(bool allowanceGiven) { 
        itemMintAllowance = allowanceGiven;
        if (allowanceGiven)
        {
            // Deactivate allowance buttons
            foreach (GameObject button in approveButtons)
            {
                button.SetActive(false);
            }
        }
    }
    public void WriteItemBalance(List<BigInteger> balances)
    {
        for (int i = 0; i < balances.Count; i++) 
        { 
            itemBalanceTexts[i].text = balances[i].ToString(); 
        }
    }

    // Get Lords
    public void Button_GetLordBalance()
    {
        // if info already set, then skip it!
        if (lordInfoSet) { return; }

        StartCoroutine(chainManager.LordBalanceOfCall());

        lordInfoSet = true;
    }
    public void OnLordBalanceReturn(int balance)
    {
        numOfLords = balance;

        // Call IDs of owned lords
        for (int i = 0; i < numOfLords; i++)
        {
            StartCoroutine(chainManager.LordIDCall((BigInteger)i));
        }
    }
    public void OnLordIDReturn(Lord lordInfo)
    {
        // First, create the empty prefab
        Lord newLord = Instantiate(lordPrefab, lordPanel.transform);
        // Then, write the info on it
        newLord = lordInfo;

        // Get number of clans, license and collected taxes for this lord
        StartCoroutine(chainManager.LordNumberOfClansCall(newLord));
        StartCoroutine(chainManager.LordNumberOfLicenseCall(newLord));
        StartCoroutine(chainManager.LordCollectedTaxesCall(newLord));
    }

    // Clan
    public void Button_ViewMyClan()
    {
        DisplayClanInfo(walletClan);
        DisplayClanPoints(walletClan);
    }
    public void DisplayClanInfo(Clan clan)
    {
        // Update Clan Name texts
        foreach (var text in clanNameTexts)
        {
            text.text = clan.name;
        }

        // Display in the clan canvas
        clanInfoNameText.text = clan.name;
        clanInfoIDText.text = clan.id.ToString();
        clanInfoMottoText.text = clan.motto;
        clanInfoDescriptionText.text = clan.description;
    }
    public void DisplayClanPoints(Clan clan)
    {
        clanPointText.text = clan.clanPoint.ToString() +
            " (out of " + clan.totalClanPoints.ToString() + ")"
        ;

        double clanRewardShare = (clan.clanPoint / clan.totalClanPoints) * 100;
        double clanReward = (clanRewardShare * roundTotalClanReward) / 100;

        rewardShareText.text = clanRewardShare.ToString() + "% (" + clanReward + " STICK)";

        if (walletClan.id == clan.id)
        {
            walletClanPoints.text = walletMemberPoints.ToString() +
                " (out of " + clan.totalMemberPoints.ToString() + ")"
            ;

            double memberRewardShare = (walletMemberPoints / clan.totalMemberPoints) * 100;
            double memberReward = (clanRewardShare * clanReward) / 100;

            walletRewardShare.text = memberRewardShare.ToString() + "% (" + memberReward + " STICK)";
        }
        else
        {
            walletClanPoints.text = "--";
            walletRewardShare.text = "--";
        }

        // Add members to the panel
        foreach (Member memberInfo in clan.members) 
        { 
            // First, create the empty prefab
            Member newMember = Instantiate(memberPrefab, memberPanel.transform);
            // Then, write the info on it
            newMember = memberInfo;
        }
    }

    // Election


}
