using System;
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

    // REWARD
    [Header("Reward")]
    [SerializeField] TMP_InputField clanRoundInput;
    [SerializeField] TMP_InputField clanIDInput;
    [SerializeField] TextMeshProUGUI clanRewardText;
    [SerializeField] TMP_InputField playerRoundInput;
    [SerializeField] TextMeshProUGUI[] playerRewardsTexts;
    [SerializeField] TextMeshProUGUI playerRewardsTotalText;
    [SerializeField] TextMeshProUGUI[] playerTotalRewardsTexts;
    [SerializeField] TextMeshProUGUI playerTotalRewardsTotalText;
    [SerializeField] TextMeshProUGUI[] playerCountTexts;
    [SerializeField] TMP_InputField backerRoundInput;
    [SerializeField] TextMeshProUGUI[] backerRewardsTexts;
    [SerializeField] TextMeshProUGUI backerRewardsTotalText;
    [SerializeField] TextMeshProUGUI[] backerFundsTexts;
    [SerializeField] TextMeshProUGUI backerFundsTotalText;
    [SerializeField] TextMeshProUGUI[] backerTotalFundsTexts;
    [SerializeField] TextMeshProUGUI backerTotalFundsTotalText;
    [SerializeField] TextMeshProUGUI[] backerCountTexts;
    [SerializeField] TextMeshProUGUI backerCountTotalText;

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
    [SerializeField] TextMeshProUGUI[] electionRewardTexts;
    [SerializeField] TMP_InputField TEST_currentRoundInput; // NOT IN USE
    [SerializeField] BossContainer candidatePrefab;
    [SerializeField] GameObject[] candidatePanels;

    // DAO
    [Header("DAO")]
    [SerializeField] ProposalContainer activeProposalPrefab;
    [SerializeField] ProposalContainer completedProposalPrefab;
    [SerializeField] GameObject activeProposalPanel;
    [SerializeField] GameObject completedProposalPanel;

    // References
    BlockchainManager chainManager;
    TextFitter messageWindow;

    List<ProposalContainer> proposals;

    [HideInInspector] public Clan walletClan = new Clan();
    [HideInInspector] public Clan displayClan = new Clan();
    [HideInInspector] public int walletMemberPoints;
    string walletAddress;
    int numOfLords;

    bool walletInfoSet;
    bool itemInfoSet;
    bool lordInfoSet; 
    bool bossListed;
    [HideInInspector] public bool clanInfoSet;

    int roundTotalClanReward;
    [HideInInspector] public int currentRound;
    [HideInInspector] public double[] backerRewards;
    int[] itemBalances = new int[5];

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
                         * --> Display candidates
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
        chainManager = FindObjectOfType<BlockchainManager>();
        messageWindow = FindObjectOfType<LevelManager>().messageWindow.GetComponent<TextFitter>();

        print("Name:" + messageWindow.name);
    }
    void Update()
    {
        if (chainManager == null) { chainManager = FindObjectOfType<BlockchainManager>(); }
        if (messageWindow == null) { messageWindow = FindObjectOfType<LevelManager>().messageWindow.GetComponent<TextFitter>();
            print("Name:" + messageWindow.name);
        }
    }

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
        StartCoroutine(chainManager.GetCurrentRoundNumberCall());

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

    // Rewards
    // Clan rewards
    bool checkClanRewardParameters()
    {
        if (clanRoundInput.text == "" || clanIDInput.text == "" || clanIDInput.text == "0")
        {
            messageWindow.DisplayMessage("Please enter round number and clan ID to check clan rewards!", false, 3f);
            return false;
        }

        int roundNumber = int.Parse(clanRoundInput.text);

        if (roundNumber < 1 || roundNumber > currentRound)
        {
            messageWindow.DisplayMessage("Invalid round nubmer!", false, 3f);
            return false;
        }

        if (roundNumber == currentRound)
        {
            messageWindow.DisplayMessage("You can't have any reward for this round because it is still going on!", false, 5f);
            return false;
        }

        return true;
    }
    public void Button_ClanRewardCheck()
    {   
        if (!checkClanRewardParameters()) { return; }
        StartCoroutine(chainManager.ViewMemberRewardCall(BigInteger.Parse(clanIDInput.text), BigInteger.Parse(clanRoundInput.text)));
    }
    public void Button_ClaimClanReward()
    {
        // Check parameters and claim amount. If not sufficient, then return.
        if (!checkClanRewardParameters() || double.Parse(clanRewardText.text) == 0) { return; }
        StartCoroutine(chainManager.ClaimMemberRewardCall(BigInteger.Parse(clanIDInput.text), BigInteger.Parse(clanRoundInput.text)));
    }
    // Player rewards
    public void Button_PlayerRewardCheck() { }
    // Backer Rewards
    bool checkBackerRewardParameters()
    {
        if (backerRoundInput.text == "")
        {
            messageWindow.DisplayMessage("Please enter round number to check backer rewards!", false, 3f);
            return false;
        }

        int roundNumber = int.Parse(backerRoundInput.text);

        if (roundNumber == currentRound)
        {
            messageWindow.DisplayMessage("You can't have any reward for this round because it is still going on!", false, 5f);
            return false;
        }
        if (roundNumber < 0 || roundNumber > currentRound)
        {
            messageWindow.DisplayMessage("Invalid round nubmer!", false, 3f);
            return false;
        }

        return true;
    }
    public void Button_BackerRewardCheck()
    {        
        if (!checkBackerRewardParameters()) { return; }
        StartCoroutine(chainManager.GetBackerRewards(BigInteger.Parse(backerRoundInput.text)));
    }
    public void Button_BackerRewardClaim()
    {
        // If the round number is wrong or there is no backer reward, then return
        if (!checkBackerRewardParameters() || double.Parse(backerRewardsTotalText.text) == 0) { return; }
        StartCoroutine(chainManager.ClaimBackerRewardCall(BigInteger.Parse(backerRoundInput.text)));
    }
    // Return functions
    public void OnClanRewardReturn(double reward)
    {
        clanRewardText.text = reward.ToString();
    }
    public void OnPlayerRewardsReturn(
        List<BigInteger> rewards, List<BigInteger> totalRewards,
        List<BigInteger> playerCounts
    )
    {
        double totalUserReward = 0;
        double totalAllRewards = 0;

        for (int i = 0; i < rewards.Count; i++)
        {
            // Convert values from Wei to Ether
            double dReward = FromWei(rewards[i]);
            double dTotalRewards = FromWei(totalRewards[i]);
            double dPlayerCounts = FromWei(playerCounts[i]);

            // Add to total
            totalUserReward += dReward;
            totalAllRewards += dTotalRewards;

            // Display
            playerRewardsTexts[i].text = dReward.ToString();
            playerTotalRewardsTexts[i].text = dTotalRewards.ToString();
            playerCountTexts[i].text = dPlayerCounts.ToString();
        }

        // Display totals
        playerRewardsTotalText.text = totalUserReward.ToString();
        playerTotalRewardsTotalText.text = totalAllRewards.ToString();
    }
    public void OnBackerRewardsReturn(
        List<BigInteger> rewards, List<BigInteger> funds, 
        List<BigInteger> totalFunds, List<BigInteger> backerCounts
    )
    {
        print("OnBackerRewardsReturn:");

        double totalUserReward = 0;
        double totalUserFunds = 0;
        double totalAllFunds = 0;
        double totalBackersCount = 0;

        for (int i = 0; i < rewards.Count; i++)
        {
            // Convert values from Wei to Ether
            double dReward = FromWei(rewards[i]);
            double dFunds = FromWei(funds[i]);
            double dTotalFunds = FromWei(totalFunds[i]);
            double dBackerCounts = FromWei(backerCounts[i]);

            // Add to total
            totalUserReward += dReward;
            totalUserFunds += dFunds;
            totalAllFunds += dTotalFunds;
            totalBackersCount += dBackerCounts;

            // Display
            backerRewardsTexts[i].text = dReward.ToString();
            backerFundsTexts[i].text = dFunds.ToString();
            backerTotalFundsTexts[i].text = dTotalFunds.ToString();
            backerCountTexts[i].text = dBackerCounts.ToString();

            print("Level: " + i);
            print("Rewards: " + dReward.ToString());
            print("Funds: " + dFunds.ToString());
            print("Total Funds: " + dTotalFunds.ToString());
            print("Backer Count: " + dBackerCounts.ToString());
        }

        // Display totals
        backerRewardsTotalText.text = totalUserReward.ToString();
        backerFundsTotalText.text = totalUserFunds.ToString();
        backerTotalFundsTotalText.text = totalAllFunds.ToString();
        backerCountTotalText.text = totalBackersCount.ToString();
    }
    
    // Check Item Balance and Allowance
    public void Button_ItemCheckBalance()
    {
        // if info already set, then skip it!
        if (itemInfoSet) { return; }

        chainManager.Button_ItemBalanceOf(0);

        itemInfoSet = true;
    }
    public void WriteItemMintAllowance(bool allowanceGiven) { 
        if (allowanceGiven)
        {
            // Deactivate allowance buttons
            foreach (GameObject button in approveButtons)
            {
                button.SetActive(false);
            }
        }
    }
    public void WriteItemBalance(int id, int balance)
    {
        itemBalances[id] = balance;
        itemBalanceTexts[id].text = itemBalances[id].ToString();  
    }
    public void AddItemBalance(int id, int amount)
    {
        itemBalances[id] += amount;
        itemBalanceTexts[id].text = itemBalances[id].ToString();
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
    public void OnLordIDReturn(int lordID)
    {
        // First, create the empty prefab
        Lord newLord = Instantiate(lordPrefab, lordPanel.transform);
        // Then, write the info on it
        newLord.SetID(lordID);
    }

    // Clan
    public void Button_ViewMyClan()
    {
        clanInfoSet = false;
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

        if (clan.id == walletClan.id) { clanInfoSet = true; }
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

    public void Button_ListBosses()
    {
        // if info already set, then skip it!
        if (bossListed) { return; }

        // Gets the supply and triggers the display function
        StartCoroutine(chainManager.BossSupplyCall());  

        // Gets the rewards and triggers the display function
        StartCoroutine(chainManager.GetCurrentBackerRewardCall());

        // Get the candidates for each level
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(chainManager.GetLevelCandidatesCall(
                (BigInteger)currentRound, (BigInteger)i
            ));
        }

        bossListed = true;
    }
    public void OnBossSupplyReturn(int supply)
    {
        for (int i = 0; i < supply; i++)
        {
            // First, create the empty prefab
            BossContainer newBoss = Instantiate(bossPrefab, bossListPanel.transform);
            // Then, write the info on it
            newBoss.BossListIDset(i);
        }

    }
    public void OnLevelBackerRewardReturn(List<BigInteger> rewards)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            backerRewards[i] = FromWei(rewards[i]);

            electionRewardTexts[i].text = "Level " + (i + 1).ToString() +
                " Backer Rewards: " + backerRewards[i] + " STICK";
        }
    }
    public void OnLevelCandidateReturnReturn(int level, List<BigInteger> candidateIDs)
    {
        for (int i = 0; i < candidateIDs.Count; i++)
        {
            BossContainer newCandidate = Instantiate(candidatePrefab, candidatePanels[level].transform);
            newCandidate.CandidateIDset((int)candidateIDs[i], level, currentRound);
        }
    }

    // DAO
    public void Button_GetLastProposals(int proposalAmount)
    {
        StartCoroutine(chainManager.GetLastProposalBasics((BigInteger)proposalAmount));
    }
    public void OnProposalReturn(
        List<BigInteger> ids, 
        List<string> descriptions, 
        List<BigInteger> startTime, 
        List<BigInteger> endTime, 
        List<BigInteger> status
    ) {
        int rejectedCount = 0;

        // create containers with basic information
        for (int i = 0; i < ids.Count; i++)
        {
            // Decide which panel it belongs
            GameObject panel = activeProposalPanel;
            if (status[i] > 1)
            {
                if (rejectedCount > 10) { continue; }   // skip to add it to the proposals

                panel = completedProposalPanel;
                rejectedCount++;
            }

            // create the empty prefab
            ProposalContainer newProp = Instantiate(activeProposalPrefab, panel.transform);

            // Then, write the info on it
            newProp.id = (int)ids[i];
            newProp.description = descriptions[i];
            newProp.startTime = (int)startTime[i];
            newProp.endingTime = (int)endTime[i];
            newProp.status = (int)status[i];

            // Add it to the list
            proposals.Add(newProp);
        }

        // Send them to the blockchain manager to request number information
        StartCoroutine(chainManager.GetLastProposalNumbers((BigInteger)proposals.Count, proposals));
    }

    // Conversion Tools
    private static BigInteger ToWei(double value) { return (BigInteger)(value * Math.Pow(10, 18)); }
    private static double FromWei(BigInteger value) { return ((double)value / Math.Pow(10, 18)); }
}
