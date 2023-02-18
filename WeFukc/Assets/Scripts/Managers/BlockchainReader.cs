using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class BlockchainReader : MonoBehaviour
{
    public static BlockchainReader instance;

    [SerializeField] TextMeshProUGUI[] addressTexts;
    [SerializeField] TextMeshProUGUI[] tokenBalanceTexts;
    [SerializeField] TextMeshProUGUI[] daoBalanceTexts;
    [SerializeField] TextMeshProUGUI[] clanNameTexts;

    [SerializeField] TextMeshProUGUI clanInfoNameText;        // Clan Info
    [SerializeField] TextMeshProUGUI clanInfoIDText;
    [SerializeField] TextMeshProUGUI clanInfoMottoText;
    [SerializeField] TextMeshProUGUI clanInfoDescriptionText;

    [SerializeField] TextMeshProUGUI[] itemBalanceTexts;
    [SerializeField] GameObject[] approveButtons;


    // Variables
    BlockchainManager chainManager;

    [HideInInspector]
    public ClanInfo clanInfo;
    string accountAddress;

    bool itemMintAllowance;

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

    // Basic Info Display
    public void OnWalletChange(string address)
    {
        accountAddress = address;

        foreach (TextMeshProUGUI text in addressTexts)
        {
            text.text = address.Substring(0, 6) + "...." + address.Substring(38, 4);
        }
        print("Wallet Texts are set!: " + address);

        chainManager.Button_GetClanOf();
        chainManager.Button_TokenBalanceUpdate();
        chainManager.Button_DAOBalanceUpdate();
    }
    public void WriteTokenBalance(double balance)
    {
        foreach (var text in tokenBalanceTexts)
        {
            text.text = ((int)balance).ToString() + " STICK";
        }
    }
    public void WriteDAOBalance(double balance)
    {
        foreach (var text in daoBalanceTexts)
        {
            text.text = ((int)balance).ToString() + " STICK";
        }
    }
    public void WriteClanInfo(ClanInfo info)
    {
        clanInfo = info;

        // Update Clan Name texts
        foreach (var text in clanNameTexts)
        {
            text.text = info.name;
        }

        // Display in the clan canvas
        clanInfoNameText.text = info.name;
        clanInfoIDText.text = info.id.ToString();
        clanInfoMottoText.text = info.motto;
        clanInfoDescriptionText.text = info.description;
    }
    
    // Check Item Balance
    public void Button_ItemCheckBalance()
    {
        chainManager.Button_ItemBalanceOfBatch(
            new List<string> { accountAddress }, new List<BigInteger> { 1, 2, 3, 4, 5 }
        );
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
}
