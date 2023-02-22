using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

using Nethereum.Unity.Metamask;     // for GetContractTransactionUnityRequest
using Nethereum.Unity.Contracts;    // for GetContractTransactionUnityRequest
using Nethereum.Unity.Rpc;          // for GetUnityRpcRequestClientFactory
using Nethereum.RPC.HostWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

using ContractDefinitions.Contracts.testToken.ContractDefinition;
using ContractDefinitions.Contracts.testNFT.ContractDefinition;
using ContractDefinitions.Contracts.testItem.ContractDefinition;
/*
    using Contracts.Contracts.Boss;
    using Contracts.Contracts.License;
    using Contracts.Contracts.Community;
    using Contracts.Contracts.DAO;
    using Contracts.Contracts.Executors;
    using Contracts.Contracts.Items;
    using Contracts.Contracts.Lord;
    using Contracts.Contracts.Rent;
    using Contracts.Contracts.Round;
    using Contracts.Contracts.Token;
*/

public class BlockchainManager : MonoBehaviour
{
    public static BlockchainManager instance;

    // --- Essentials --- //
    bool _isMetamaskInitialised;
    string _selectedAccountAddress = "";
    string _currentContractAddress = "";
    private BigInteger _currentChainId;
    private BigInteger desiredChainID = 421613; // Arbi Goerli
    string[] contracts = new string[] {
      "0xd5DdaF6df51220a17E2292cF4A64ae6510c5415e", // 0- Boss
      "0xAe7F338bEc15Aa8aC310A0D2139fcdf3D7E3a447", // 1- Clan
      "0xfBeede26a1F88745c2091a39E47fd10eFC7e8E5F", // 2- License
      "0xdC0F3C29b0D2a615cB7b46e59D7F7271962730F9", // 3- Community
      "0x40436Cb71F18594185ef6098631aADDB6d35Ca0B", // 4- DAO
      "0xB52f5ff2aa6Ed6017cc7816A9E3F995e0c4A85BD", // 5- Executor
      "0x1727bAaa06c10519762d8233301f3f1E9e59c17c", // 6- Item
      "0x6D11053Bb0546eE4E1B856702A96a8b17341e6d0", // 7- Lord
      "0x267dB8A829Ad924792a5926a12b32a9Def003021", // 8- Rent
      "0x4FD38fF7367bD2a716A278C82A8D7ae580492760", // 9- Round
      "0x0000000000000000000000000000000000000000", //
      "0x1A573AF12D8B317Ff9A3057c569d6E3EC2fc3504", // 11- Token
      "0x0000000000000000000000000000000000000000"  //
    };

    // --- References --- //
    LevelManager levelManager;
    BlockchainReader chainReader;


    #region LORD
    [Header("== Lord Objects ==")]
    // Objects
    [SerializeField] TMP_InputField lordPriceInput;
    [SerializeField] TextMeshProUGUI lordSupplyText;
    [SerializeField] TextMeshProUGUI lordCurrentCostText;
    #endregion

    #region CLAN 
    [Header("== Clan Objects ==")]
    // Objects - Input
    [SerializeField] TMP_InputField clanIDSearchInput;          // Search Clan
    [SerializeField] TMP_InputField createClanLordIDInput;      // Create Clan
    [SerializeField] TMP_InputField createClanNameInput;
    [SerializeField] TMP_InputField createClanDescriptionInput;
    [SerializeField] TMP_InputField createClanMottoInput;
    [SerializeField] TMP_InputField createClanLogoInput;
    [SerializeField] TMP_InputField clanSetMemberAddressInput;  // Set Member
    [SerializeField] TMP_InputField clanSetExecutorAddressInput;// Set Executor
    [SerializeField] TMP_InputField clanSetModAddressInput;     // Set Mod
    [SerializeField] TMP_InputField clanGiveMemberPointAddressInput;    // Give Member Point - Address
    [SerializeField] TMP_InputField clanGiveMemberPointPointInput;      // Give Member Point - Point
    [SerializeField] TMP_InputField clanClaimMemberRewardRoundInput;    // Claim Member Reward - Round
    [SerializeField] TMP_InputField clanClaimMemberRewardClanIDInput;   // Claim Member Reward - ClanID
    [SerializeField] TMP_InputField clanTransferLeadershipAddressInput; // Transfer Leadership - New Address
    [SerializeField] TMP_InputField clanUpdateInfoNameInput;  // Update Info - Name
    [SerializeField] TMP_InputField clanUpdateInfoDescInput;  // Update Info - Description
    [SerializeField] TMP_InputField clanUpdateInfoMottoInput; // Update Info - Motto
    [SerializeField] TMP_InputField clanUpdateInfoLogoInput;  // Update Info - Logo
    // Texts
    [SerializeField] TextMeshProUGUI clanAvailableMemberRewardText; // Clam Member Reward


    #endregion

    #region ROUND
    [Header("== Round Objects ==")]
    // Objects
    [SerializeField] TMP_InputField backerRoundInput;
    [SerializeField] TextMeshProUGUI[] backerRewards = new TextMeshProUGUI[11];
    [SerializeField] TextMeshProUGUI[] backerFunds = new TextMeshProUGUI[11];
    [SerializeField] TextMeshProUGUI[] totalFunds = new TextMeshProUGUI[11];
    [SerializeField] TextMeshProUGUI[] numOfBackers = new TextMeshProUGUI[11];
    #endregion


    #region DAO
    [Header("== DAO Objects ==")]
    // Objects
    [SerializeField] TMP_InputField proposalDescription;
    [SerializeField] TextMeshProUGUI tetet;
    [SerializeField] TMP_Dropdown proposalTypeInput;
    #endregion

    #region DELETE 
    [Header("== DELETE ==")]
    TextMeshProUGUI tokenBalanceText;
    TextMeshProUGUI nftBalanceText;
    TextMeshProUGUI itemBalanceText;

    TextMeshProUGUI nftAllowanceText;
    TextMeshProUGUI itemAllowanceText;
    TextMeshProUGUI mintNFTButtonText;
    TextMeshProUGUI mintItemButtonText;
    

    private BigInteger nftAllowance;
    private BigInteger itemAllowance;


    string tokenContract = "0x57400f3692Cb51b698774ca63451E5aD73490f1b";
    string nftContract = "0x34063824bAf9863379d6C059C1D3653c2e18acDe";
    string itemContract = "0xcfDDbf89c06DdC3cffA3e5137321249cf67784cc";
    double mintAmount = 5.3;
    double currentAllowance;
    #endregion

    

    /* Notes
        - Always give much higer (like 10x) allowance when you increase compared to when you check allowance.
    Because, it doesn't give perfect number, gives less. Makes you increase it twice!
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
        levelManager = FindObjectOfType<LevelManager>();
        chainReader = FindObjectOfType<BlockchainReader>();
    }
    void Update()
    {
        if (levelManager == null) { levelManager = FindObjectOfType<LevelManager>(); }
        if (chainReader == null) { chainReader = FindObjectOfType<BlockchainReader>(); }
    }



    //******    BUTTONS    ******//
    // Other
    public void Button_TokenBalanceUpdate() { StartCoroutine(TokenBalanceOfCall()); }
    public void Button_DAOBalanceUpdate() { StartCoroutine(DAOBalanceOfCall()); }

    // Lord
    public void Button_LordMint()
    {
        print("string: " + lordPriceInput.text);

        Double price = Double.Parse(lordPriceInput.text, System.Globalization.CultureInfo.InvariantCulture);

        if (price < 0.05)
        {
            print("Price is lower than minimum(0.05 ETH) !!");
            return;
        }

        print("Lord Mint Price to send: " + price.ToString());

        StartCoroutine(LordMintCall(ToWei(price)));
    }
    public void Button_LordUpdateSupply(){ StartCoroutine(GetLordSupply()); }
    public void Button_LordID(BigInteger index){ StartCoroutine(LordIDCall(index)); }

    // Clan
    public void Button_ClanCreate() { StartCoroutine(CreateClanCall()); }
    public void Button_ClanDeclare() { StartCoroutine(DeclareClanCall()); }
    public void Button_ClanSetMember(bool assignAsMember) { StartCoroutine(SetMemberCall(assignAsMember)); }
    public void Button_ClanSetExecutor(bool assignAsExecutor) { StartCoroutine(SetExecutorCall(assignAsExecutor)); }
    public void Button_ClanSetMod(bool assignAsMod) { StartCoroutine(SetModCall(assignAsMod)); }
    public void Button_ClanGiveMemberPoint(bool isDecreasing)
    {
        BigInteger point = BigInteger.Parse(clanGiveMemberPointPointInput.text, System.Globalization.CultureInfo.InvariantCulture);
        
        if (point == 0) { print("PLEASE ENTER SOME POINT !!"); return; }
        StartCoroutine(GiveMemberPointCall(point, isDecreasing)); 
    }
    public void Button_ClanClaimMemberReward() { StartCoroutine(ClaimMemberRewardCall()); }
    public void Button_ClanTransferLeadership() { StartCoroutine(TransferLeadershipCall()); }
    public void Button_ClanDisband() { StartCoroutine(DisbandCall()); }
    public void Button_ClanUpdateInfo() { StartCoroutine(UpdateClanInfoCall()); }
    public void Button_MintLicense(LordContainer lord, BigInteger _amount) { StartCoroutine(MintLicenseCall(lord, _amount)); }
    public void Button_LordDAOvote(LordContainer lord, BigInteger _proposalID, bool _isApproving) { StartCoroutine(LordDAOvoteCall(lord, _proposalID, _isApproving)); }
    public void Button_ClanViewMemberReward() 
    { 
        BigInteger clanID = BigInteger.Parse(clanClaimMemberRewardClanIDInput.text);
        BigInteger roundNumber = BigInteger.Parse(clanClaimMemberRewardRoundInput.text);

        if (clanID == 0 || roundNumber == 0) { print("Invalid Input !!!"); }

        StartCoroutine(ViewMemberRewardCall(clanID, roundNumber)); 
    }
    public void Button_ClanViewInfo() { StartCoroutine(ViewClanInfoCall(int.Parse(clanIDSearchInput.text))); }

    // Round
    public void Button_RoundGetBackerInfo() { StartCoroutine(GetBackerRewardCall()); }

    // DAO
    public void Button_DAOnewProposal() { StartCoroutine(NewProposalCall()); }
    public void Button_DAOvote(BigInteger _proposalID, bool _approve) { 
        // Check if the account has enough DAO tokens
        StartCoroutine(VoteCall(_proposalID, _approve)); 
    }
    public void Button_RoundFund(bool fund, BigInteger level, BigInteger bossID, BigInteger amount)
    {
        if (fund)
            StartCoroutine(FundBossCall(level, bossID, amount));
        else
            StartCoroutine(DefundBossCall(level, bossID, amount));
    }

    // Item
    public void Button_ItemMint(BigInteger id, BigInteger amount) { StartCoroutine(MintItemCall(id, amount)); }
    public void Button_ItemBurn(List<BigInteger> ids, List<BigInteger> amounts) { StartCoroutine(BurnItemsCall(ids, amounts)); }
    public void Button_ItemBalanceOfBatch(List<string> accounts, List<BigInteger> ids) { StartCoroutine(BalanceOfBatchCall(accounts, ids)); }
    public void Button_ItemCheckMintAllowance() { StartCoroutine(ItemMintAllowanceCheckCall()); }
    public void Button_ItemIncreaseTokenAllowance() { StartCoroutine(ItemMintAllowanceCall()); }

    #region DELETE 
    public void UpdateBalanceButton()
    {
        StartCoroutine(UpdateTokenBalance());
        StartCoroutine(UpdateNFTBalance());
        StartCoroutine(UpdateItemBalance());
    }
    public void MintTokenButton() { StartCoroutine(MintToken()); }
    public void MintNFTButton()
    {
        // If not enough allowance, then increase it first
        if (!checkAllowance(nftContract)) { StartCoroutine(IncreaseAllowance(nftContract)); return; }

        // If there is enough allowance, then mint
        StartCoroutine(MintNFT());
    }
    public void MintItemButton()
    {
        // If not enough allowance, then increase it first
        if (!checkAllowance(itemContract)) { StartCoroutine(IncreaseAllowance(itemContract)); return; }

        // If there is enough allowance, then mint
        StartCoroutine(MintItem());
    }
    #endregion



    /*
     * 
     * 
     * 
     *              UPDATE Round and Clan Contract Definitions
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     *              CONVERT ALL TOKEN RELATED VALUES TO WEI !!!!
     * 
     * 
     * 
     * 
     * 
     * 
     * */


    //******    BLOCKCHAIN FUNCTIONS    ******//

    // General
    public IEnumerator TokenBalanceOfCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of - Token Contract: " + contracts[11]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Token.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.Token.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Token.ContractDefinition
                .BalanceOfFunction()
            {
                Account = _selectedAccountAddress
            }, contracts[11]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            chainReader.DisplayTokenBalance(FromWei(balance));
            print("Balance of " + _selectedAccountAddress + " : " + FromWei(balance));
        }
    }
    public IEnumerator DAOBalanceOfCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of - DAO Contract: " + contracts[4]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Token.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.Token.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Token.ContractDefinition
                .BalanceOfFunction()
            {
                Account = _selectedAccountAddress
            }, contracts[4]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            chainReader.DisplayDAOBalance(FromWei(balance));
            print("Balance of " + _selectedAccountAddress + " : " + FromWei(balance));
        }
    }
    private IEnumerator ItemMintAllowanceCheckCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Item Contract Allowance - Token Contract: " + contracts[11]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Token.ContractDefinition.AllowanceFunction,
                Contracts.Contracts.Token.ContractDefinition.AllowanceOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Token.ContractDefinition
                .AllowanceFunction()
            {
                Owner = _selectedAccountAddress,
                Spender = contracts[6]
            }, contracts[11]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var allowance = FromWei(dtoResult.ReturnValue1);

            if (allowance > 1000000000) // If the allowance is more than 1 billion token
            {
                chainReader.WriteItemMintAllowance(true);
            }
            print("Allowance Of " + _selectedAccountAddress + " : " + allowance);
        }
    }

    //------ CLAN CONTRACT ------//
    // Write
    private IEnumerator CreateClanCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Creating Clan - Clan Contract: " + contracts[1]); // Clan Contract
        // Paramaters
        print("Lord ID: " + BigInteger.Parse(createClanLordIDInput.text));
        print("Clan Name: " + createClanNameInput.text);
        print("Clan Description: " + createClanDescriptionInput.text);
        print("Clan Logo URI: " + createClanLogoInput.text);
        print("Clan Motto: " + createClanMottoInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.CreateClanFunction
            {
                LordID = BigInteger.Parse(createClanLordIDInput.text),
                ClanName = createClanNameInput.text,
                ClanDescription = createClanDescriptionInput.text,
                ClanLogoURI = createClanLogoInput.text,
                ClanMotto = createClanMottoInput.text
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.CreateClanFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator DeclareClanCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Declaring Clan - Clan Contract: " + contracts[1]); // Clan Contract
        // Paramaters
        print("Clan ID: " + BigInteger.Parse(clanIDSearchInput.text));

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.DeclareClanFunction
            {
                ClanID = BigInteger.Parse(clanIDSearchInput.text)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.DeclareClanFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator SetMemberCall(bool assignAsMember)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Set Member - Clan Contract: " + contracts[1]); // Clan Contract
        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);
        print("Member Address: " + clanSetMemberAddressInput.text);
        print("Set As: " + assignAsMember);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.SetMemberFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id, // Get the displayed clan ID
                Address = clanSetMemberAddressInput.text,
                SetAsMember = assignAsMember
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.SetMemberFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator SetExecutorCall(bool assignAsExecutor)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Set Executor - Clan Contract: " + contracts[1]); // Clan Contract
        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);
        print("Executor Address: " + clanSetExecutorAddressInput.text);
        print("Set As: " + assignAsExecutor);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.SetClanExecutorFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id, // Get the displayed clan ID
                Address = clanSetExecutorAddressInput.text,
                SetAsExecutor = assignAsExecutor
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.SetClanExecutorFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator SetModCall(bool assignAsMod)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Set Mod - Clan Contract: " + contracts[1]); // Clan Contract
        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);
        print("Mod Address: " + clanSetModAddressInput.text);
        print("Set As: " + assignAsMod);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.SetClanModFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id, // Get the displayed clan ID
                Address = clanSetModAddressInput.text,
                SetAsMod = assignAsMod
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.SetClanModFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator GiveMemberPointCall(BigInteger point, bool isDecreasing)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Give Member Point - Clan Contract: " + contracts[1]); // Clan Contract

        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);
        print("Member Address: " + clanGiveMemberPointAddressInput.text);
        print("Point: " + point);
        print("Is decreasing?: " + isDecreasing);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.GiveMemberPointFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id, // Get the displayed clan ID
                MemberAddress = clanGiveMemberPointAddressInput.text,
                Point = point,
                IsDecreasing = isDecreasing
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.GiveMemberPointFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator ClaimMemberRewardCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Claim Member Reward - Clan Contract: " + contracts[1]); // Clan Contract

        // Parameters
        print("Clan ID: " + BigInteger.Parse(clanClaimMemberRewardClanIDInput.text));
        print("Round Number: " + BigInteger.Parse(clanClaimMemberRewardRoundInput.text));

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.MemberRewardClaimFunction
            {
                ClanID = BigInteger.Parse(clanClaimMemberRewardClanIDInput.text), // Get the displayed clan ID
                RoundNumber = BigInteger.Parse(clanClaimMemberRewardRoundInput.text)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.MemberRewardClaimFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator UpdateClanInfoCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Update Clan Info - Clan Contract: " + contracts[1]); // Clan Contract

        // Parameters
        print("Clan ID: " + BigInteger.Parse(clanClaimMemberRewardClanIDInput.text));
        print("Name: " + clanUpdateInfoNameInput.text);
        print("Description: " + clanUpdateInfoDescInput.text);
        print("Motto: " + clanUpdateInfoMottoInput.text);
        print("Logo URI: " + clanUpdateInfoLogoInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.UpdateClanInfoFunction
            {
                ClanID = BigInteger.Parse(clanClaimMemberRewardClanIDInput.text), // Get the displayed clan ID
                NewName = clanUpdateInfoNameInput.text,
                NewDescription = clanUpdateInfoDescInput.text,
                NewMotto = clanUpdateInfoMottoInput.text,
                NewLogoURI = clanUpdateInfoLogoInput.text,
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.UpdateClanInfoFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator TransferLeadershipCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Transfer Leadership - Clan Contract: " + contracts[1]); // Clan Contract

        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);
        print("New Leader Address: " + clanTransferLeadershipAddressInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.TransferLeadershipFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id, // Get the displayed clan ID
                NewLeader = clanTransferLeadershipAddressInput.text
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.TransferLeadershipFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator DisbandCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Disband - Clan Contract: " + contracts[1]); // Clan Contract

        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.DisbandClanFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id // Get the displayed clan ID
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickClan.ContractDefinition.DisbandClanFunction
            >(callFunction, contracts[1]);  // Clan Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }

    // Read
    private IEnumerator ViewMemberRewardCall(BigInteger clanID, BigInteger roundNumber)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Member Reward - Clan Contract: " + contracts[1]);
        // Parameters
        print("Clan ID: " + clanID);
        print("Round Number: " + roundNumber);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickClan.ContractDefinition.ViewMemberRewardFunction,
                Contracts.Contracts.StickClan.ContractDefinition.ViewMemberRewardOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickClan.ContractDefinition
                .ViewMemberRewardFunction()
            {
                ClanID = clanID,
                RoundNumber = roundNumber
            }, contracts[1]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var reward = dtoResult.ReturnValue1;

            clanAvailableMemberRewardText.text = reward.ToString();
            print("Available Member Reward: " + reward);
        }
    }
    public IEnumerator WalletClanCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Clan Of - Clan Contract: " + contracts[1]);
        // Parameters
        print("Wallet: " + _selectedAccountAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickClan.ContractDefinition.GetClanOfFunction,
                Contracts.Contracts.StickClan.ContractDefinition.GetClanOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickClan.ContractDefinition
                .GetClanOfFunction()
            {
                Address = _selectedAccountAddress
            }, contracts[1]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var clanID = dtoResult.ReturnValue1;

            if (clanID > 0) // If the account has a clan
            {
                chainReader.walletClan.id = (int)clanID;  // Save the account's clan ID
                ViewClanInfoCall((int)clanID);
            }
            print("Clan ID of " + _selectedAccountAddress + " : " + clanID);
        }
    }
    public IEnumerator ViewClanInfoCall(int clanID)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Clan Info - Clan Contract: " + contracts[1]);
        // Parameters
        print("Clan ID: " + clanID);

        StartCoroutine(GetClanPointsCall(clanID));  // Start the point call as well

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickClan.ContractDefinition.ViewClanInfoFunction,
                Contracts.Contracts.StickClan.ContractDefinition.ViewClanInfoOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickClan.ContractDefinition
                .ViewClanInfoFunction()
            {
                ClanID = clanID
            }, contracts[1]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;

            Clan clan;
            if (clanID == chainReader.walletClan.id) { clan = chainReader.walletClan; }
            else { clan = chainReader.displayClan; }

            clan.leaderAddress = dtoResult.ReturnValue1;
            clan.lordID = (int)dtoResult.ReturnValue2;
            clan.firstRound = (int)dtoResult.ReturnValue3;
            clan.name = dtoResult.ReturnValue4;
            clan.description = dtoResult.ReturnValue5;
            clan.motto = dtoResult.ReturnValue6;
            clan.logoURI = dtoResult.ReturnValue7;
            clan.canExecutorsSignalRebellion = dtoResult.ReturnValue8;
            clan.canExecutorsSetPoint = dtoResult.ReturnValue9;
            clan.canModsSetMembers = dtoResult.ReturnValue10;
            clan.isDisbanded = dtoResult.ReturnValue11;

            chainReader.DisplayClanInfo(clan);   // Send it to the reader to display

            print("Clan ID: " + clan.id);
            print("Clan Name: " + clan.name);
        }
    }
    private IEnumerator GetClanPointsCall(int clanID)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Clan Points - Clan Contract: " + contracts[1]);
        // Parameters
        print("Clan ID: " + clanID);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickClan.ContractDefinition.GetClanPointsFunction,
                Contracts.Contracts.StickClan.ContractDefinition.GetClanPointsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickClan.ContractDefinition
                .GetClanPointsFunction()
            {
                ClanID = clanID
            }, contracts[1]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;

            Clan clan;
            if (clanID == chainReader.walletClan.id) { clan = chainReader.walletClan; }
            else { clan = chainReader.displayClan; }

            // Save the points
            clan.totalClanPoints = (int)dtoResult.ReturnValue1;
            clan.clanPoint = (int)dtoResult.ReturnValue2;
            clan.totalMemberPoints = (int)dtoResult.ReturnValue3;

            // Save the member info
            List<string> memberAddresses = dtoResult.ReturnValue5;
            List<BigInteger> memberPoints = dtoResult.ReturnValue6;
            List<bool> memberActive = dtoResult.ReturnValue7;
            List<bool> memberExecutor = dtoResult.ReturnValue8;
            List<bool> memberMod = dtoResult.ReturnValue9;

            int memberCount = dtoResult.ReturnValue5.Count;

            for (int i = 0; i < memberCount; i++)
            {
                if (memberActive[i])
                {
                    // Determine the role
                    string role;
                    if (memberAddresses[i] == clan.leaderAddress) { role = "Leader"; }
                    else if (memberExecutor[i]) { role = "Executor"; }
                    else if (memberMod[i]) { role = "Mod"; }
                    else { role = "Member"; }

                    // Calculate the share
                    double share = ((int)memberPoints[i] / clan.totalMemberPoints) * 100;

                    // Add the member
                    clan.members.Add(new Member(
                        memberAddresses[i], role, (int)memberPoints[i], share
                    ));
                }
            }

            chainReader.DisplayClanPoints(clan);

            print("Clan ID: " + clan.id);
            print("Clan Name: " + clan.name);
            print("Clan Point: " + clan.clanPoint);
        }
    }

    

    //------ LORD CONTRACT ------//
    // Write
    private IEnumerator LordMintCall(BigInteger _amountToSend)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Lord Mint - Lord Contract: " + contracts[7]); // Lord Contract
        print("Given BigInteger: " + _amountToSend.ToString());

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickLord.ContractDefinition.LordMintFunction
            {
                AmountToSend = _amountToSend
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickLord.ContractDefinition.LordMintFunction
            >(callFunction, contracts[7]);  // Lord Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    public IEnumerator MintLicenseCall(LordContainer lord, BigInteger _amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Mint License - Lord Contract: " + contracts[7]); // Lord Contract
        print("Amount: " + _amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickLord.ContractDefinition.MintClanLicenseFunction
            {
                Amount = _amount
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickLord.ContractDefinition.MintClanLicenseFunction
            >(callFunction, contracts[7]);  // Lord Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
                lord.MintSuccess();
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    public IEnumerator LordDAOvoteCall(LordContainer lord, BigInteger _proposalID, bool _isApproving)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("DAO Vote - Lord Contract: " + contracts[7]); // Lord Contract
        print("Lord ID: " + lord.lordID);
        print("Proposal ID: " + _proposalID);
        print("is Approving?: " + _isApproving);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickLord.ContractDefinition.DAOvoteFunction
            {
                LordID = lord.lordID,
                ProposalID = _proposalID,
                IsApproving = _isApproving
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.StickLord.ContractDefinition.DAOvoteFunction
            >(callFunction, contracts[7]);  // Lord Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
                lord.VoteSuccess();
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    // LATER: Update Name and Description
    // LATER: Update License

    // Read
    public IEnumerator GetLordSupply()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Lord Contract: " + contracts[7]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickLord.ContractDefinition.TotalSupplyFunction,
                Contracts.Contracts.StickLord.ContractDefinition.TotalSupplyOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickLord.ContractDefinition
                .TotalSupplyFunction()
            {  }, contracts[7]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var supply = dtoResult.ReturnValue1;
                                // Base         // After 50th   *   mint cost increase
            lordSupplyText.text = (0.05 + (((double)supply - 50) * 0.001)).ToString();
            lordCurrentCostText.text = supply.ToString() + "/256";
            print("Lord Supply: " + supply);
        }
    }
    // Lord Name and Description
    public IEnumerator LordBalanceOfCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of - Lord Contract: " + contracts[7]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickLord.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.StickLord.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickLord.ContractDefinition
                .BalanceOfFunction()
            { }, contracts[7]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            chainReader.OnLordBalanceReturn((int)balance);
            print("Balance: " + balance);
        }
    }
    public IEnumerator LordIDCall(BigInteger index)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("ID call - Lord Contract: " + contracts[7]);
        print("Index: " + index);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickLord.ContractDefinition.TokenOfOwnerByIndexFunction,
                Contracts.Contracts.StickLord.ContractDefinition.TokenOfOwnerByIndexOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickLord.ContractDefinition
                .TokenOfOwnerByIndexFunction()
            {
                Owner = _selectedAccountAddress,
                Index = index
            }, contracts[7]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var id = dtoResult.ReturnValue1;

            chainReader.OnLordIDReturn(new Lord((int)id));
            print("Owned ID: " + (int)id);
        }
    }
    public IEnumerator LordNumberOfClansCall(Lord lord)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Number Of Clans - Lord Contract: " + contracts[7]);
        print("Lord ID: " + lord.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickLord.ContractDefinition.ViewNumberOfClansFunction,
                Contracts.Contracts.StickLord.ContractDefinition.ViewNumberOfClansOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickLord.ContractDefinition
                .ViewNumberOfClansFunction()
            {
                LordID = lord.id
            }, contracts[7]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var numOfClan = dtoResult.ReturnValue1;

            lord.ClanNumberSet((int)numOfClan);
            print("Number Of Clan of id(" + lord.id + "): " + numOfClan);
        }
    }
    public IEnumerator LordNumberOfLicenseCall(Lord lord)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Number Of License - License Contract: " + contracts[2]);
        print("Lord ID: " + lord.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.License.ContractDefinition.NumOfActiveLicenseFunction,
                Contracts.Contracts.License.ContractDefinition.NumOfActiveLicenseOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.License.ContractDefinition
                .NumOfActiveLicenseFunction()
            {
                ReturnValue1 = lord.id
            }, contracts[2]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var numOfLicense = dtoResult.ReturnValue1;

            lord.LicenseNumberSet((int)numOfLicense);
            print("Number Of License of id(" + lord.id + "): " + numOfLicense);
        }
    }
    public IEnumerator LordCollectedTaxesCall(Lord lord)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Collected Taxes - Clan Contract: " + contracts[1]);
        print("Lord ID: " + lord.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickClan.ContractDefinition.CollectedTaxesFunction,
                Contracts.Contracts.StickClan.ContractDefinition.CollectedTaxesOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickClan.ContractDefinition
                .CollectedTaxesFunction()
            {
                ReturnValue1 = lord.id
            }, contracts[1]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var collectedTaxes = dtoResult.ReturnValue1;

            lord.CollectedTaxSet((int)collectedTaxes);
            print("Collected Taxes of id(" + lord.id + "): " + collectedTaxes);
        }
    }



    //------ DAO CONTRACT ------//
    // Write
    private IEnumerator NewProposalCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("New Proposal - DAO Contract: " + contracts[4]); // DAO Contract

        string selectedText = proposalTypeInput.options[proposalTypeInput.value].text;
        print("Selected: " + selectedText);

        BigInteger type;
        switch (selectedText)
        {
            case "1 Hour":
                type = 1;
                break;
            case "1 Day":
                type = 2;
                break;
            case "3 Days":
                type = 3;
                break;
            default:
                type = 3;
                break;
        }

        print("Description: " + proposalDescription.text);
        print("Proposal Type: " + type);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.DAO.ContractDefinition.NewProposalFunction
            {
                Description = proposalDescription.text,
                ProposalType = type
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.DAO.ContractDefinition.NewProposalFunction
            >(callFunction, contracts[4]);  // DAO Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator VoteCall(BigInteger _proposalID, bool _approve)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Vote - DAO Contract: " + contracts[4]); // DAO Contract
        print("Proposal ID: " + _proposalID);
        print("Approve?: " + _approve);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.DAO.ContractDefinition.VoteFunction
            {
                ProposalID = _proposalID,
                IsApproving = _approve
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.DAO.ContractDefinition.VoteFunction
            >(callFunction, contracts[4]);  // DAO Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    // LATER: Spending Proposal Claim Functions

    // Read
    public IEnumerator GetDAOBalanceCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("DAO Contract: " + contracts[4]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.DAO.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.DAO.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.DAO.ContractDefinition
                .BalanceOfFunction()
            {
                Account = _selectedAccountAddress
            }, contracts[4]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var DAObalance = dtoResult.ReturnValue1;
            print("DAO Balance: " + DAObalance);
        }
    }



    //------ ROUND CONTRACT ------//
    // Write
    private IEnumerator FundBossCall(BigInteger level, BigInteger bossID, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Fund Boss - Round Contract: " + contracts[9]); // Round Contract

        print("Level: " + level);
        print("Boss ID: " + bossID);
        print("Amount: " + amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Round.ContractDefinition.FundBossFunction
            {
                LevelNumber = level,
                BossID = bossID,
                FundAmount = ToWei((double)amount)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Round.ContractDefinition.FundBossFunction
            >(callFunction, contracts[9]);  // Round Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator DefundBossCall(BigInteger level, BigInteger bossID, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Defund Boss - Round Contract: " + contracts[9]); // Round Contract

        print("Level: " + level);
        print("Boss ID: " + bossID);
        print("Amount: " + amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Round.ContractDefinition.FundBossFunction
            {
                LevelNumber = level,
                BossID = bossID,
                FundAmount = ToWei((double)amount)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Round.ContractDefinition.FundBossFunction
            >(callFunction, contracts[9]);  // Round Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator ClaimBackerRewardCall(BigInteger roundNumber)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Claim Backer Reward- Round Contract: " + contracts[9]); // Round Contract

        print("Round: " + roundNumber);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Round.ContractDefinition.ClaimBackerRewardFunction
            {
                RoundNumber = roundNumber
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Round.ContractDefinition.ClaimBackerRewardFunction
            >(callFunction, contracts[9]);  // Round Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }


    // public List<byte[]> GetProof(byte[] hashLeaf) --> hashLeaf ??
    // 

    // Read
    private IEnumerator GetBackerRewardCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Backer Reward - Round Contract: " + contracts[9]);
        // Parameters
        print("Round Number: " + backerRoundInput.text);

        BigInteger roundNumber = BigInteger.Parse(backerRoundInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Round.ContractDefinition.GetBackerRewardsFunction,
                Contracts.Contracts.Round.ContractDefinition.GetBackerRewardsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Round.ContractDefinition
                .GetBackerRewardsFunction()
            {
                RoundNumber = roundNumber
            }, contracts[9]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var res = dtoResult.ReturnValue1;

            // ADD : Call Display the clan name function and view the clan info function
            // GET THE REST OF THE VARS
            // TEST:
            print("Clan Name: ");
        }
    }



    //------ ITEM CONTRACT ------//
    // Write
    private IEnumerator MintItemCall(BigInteger id, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Mint - Items Contract: " + contracts[6]); // Item Contract

        print("id: " + id);
        print("Amount: " + amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Items.ContractDefinition.MintFunction
            {
                Id = id,
                Amount = amount,
                Data = new byte[0]
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Items.ContractDefinition.MintFunction
            >(callFunction, contracts[6]);  // Item Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator BurnItemsCall(List<BigInteger> ids, List<BigInteger> amounts)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Mint - Items Contract: " + contracts[6]); // Item Contract

        print("id: " + ids);
        print("Amount: " + amounts);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Items.ContractDefinition.BurnBatchFunction
            {
                Account = _selectedAccountAddress,
                Ids = ids,
                Values = amounts
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Items.ContractDefinition.BurnBatchFunction
            >(callFunction, contracts[6]);  // Item Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator ItemMintAllowanceCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Giving Token Allowance to Item Cont - Token Contract: " + contracts[11]); // Token Contract
        print("Item Contract: " + contracts[6]); // Item Contract

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Token.ContractDefinition.IncreaseAllowanceFunction
            {
                Spender = contracts[6], // item contract
                AddedValue = ToWei(1000000000)  // 1 billion allowance                
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Token.ContractDefinition.IncreaseAllowanceFunction
            >(callFunction, contracts[11]);  // Item Contract

            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
                chainReader.WriteItemMintAllowance(true);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }

    // Read
    private IEnumerator BalanceOfBatchCall(List<string> accounts, List<BigInteger> ids)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of Batch - Items Contract: " + contracts[6]); // Item Contract

        foreach (string account in accounts) { print("Account: " + account); }
        foreach (BigInteger id in ids) { print("Id: " + id); }

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Items.ContractDefinition.BalanceOfBatchFunction,
                Contracts.Contracts.Items.ContractDefinition.BalanceOfBatchOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Items.ContractDefinition
                .BalanceOfBatchFunction()
            {
                Accounts = accounts,
                Ids = ids
            }, contracts[6]);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;

            chainReader.WriteItemBalance(dtoResult.ReturnValue1);
            print("Item Balance has sent to the blockchainReader!");
        }
    }





    // Read Function
    private IEnumerator UpdateTokenBalance()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + tokenContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null) {
            var queryRequest = new QueryUnityRequest<
                ContractDefinitions.Contracts.testToken.ContractDefinition.BalanceOfFunction,
                ContractDefinitions.Contracts.testToken.ContractDefinition.BalanceOfOutputDTOBase>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress);

            yield return queryRequest.Query(new ContractDefinitions.Contracts.testToken.ContractDefinition
                .BalanceOfFunction(){ Account = _selectedAccountAddress }, tokenContract
            );

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            tokenBalanceText.text = FromWei(balance).ToString(); // 2 decimals will be shown
            print("Token Balance: " + balance);
        }
    }
    private IEnumerator UpdateNFTBalance()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + nftContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                ContractDefinitions.Contracts.testNFT.ContractDefinition.BalanceOfFunction,
                ContractDefinitions.Contracts.testNFT.ContractDefinition.BalanceOfOutputDTOBase>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress);

            yield return queryRequest.Query(new ContractDefinitions.Contracts.testNFT.ContractDefinition
                .BalanceOfFunction()
            { Owner = _selectedAccountAddress }, nftContract);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            nftBalanceText.text = balance.ToString(); // 2 decimals will be shown
            print("NFT Balance: " + balance);
        }
    }
    private IEnumerator UpdateItemBalance()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + itemContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                ContractDefinitions.Contracts.testItem.ContractDefinition.BalanceOfFunction,
                ContractDefinitions.Contracts.testItem.ContractDefinition.BalanceOfOutputDTOBase>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress);

            yield return queryRequest.Query(new ContractDefinitions.Contracts.testItem.ContractDefinition
                .BalanceOfFunction()
            { Account = _selectedAccountAddress, Id = 2 }, itemContract);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var balance = dtoResult.ReturnValue1;

            itemBalanceText.text = balance.ToString(); // 2 decimals will be shown
            print("Item Balance: " + balance);
        }
    }
    private IEnumerator ReadAllowance(string contractAddress)
    {
        yield return new WaitForSeconds(10);    // TEST: Checking with connect button, wait for the connection

        print("Checking Allowance for: " + contractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<AllowanceFunction, AllowanceOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress);

            yield return queryRequest.Query(new AllowanceFunction()
            { Owner = _selectedAccountAddress, Spender = contractAddress }, tokenContract);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var allowance = dtoResult.ReturnValue1;

            if (contractAddress == nftContract)
            {
                nftAllowance = allowance;
                nftAllowanceText.text = allowance.ToString(); // 2 decimals will be shown
                print("NFT contract allowance: " + allowance.ToString());
            }
            else
            {
                itemAllowance = allowance;
                itemAllowanceText.text = allowance.ToString(); // 2 decimals will be shown
                print("Item contract allowance: " + allowance.ToString());
            }
        }
    }

    // Write Functions
    private IEnumerator MintToken() {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + tokenContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null) {

            var mintFunction = new ContractDefinitions.Contracts.testToken.ContractDefinition.MintFunction {
                To = _selectedAccountAddress,
                Amount = ToWei(mintAmount)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                ContractDefinitions.Contracts.testToken.ContractDefinition.MintFunction>(mintFunction, tokenContract);
            if (contractTransactionUnityRequest.Exception == null) {
                print(contractTransactionUnityRequest.Result);
            }
            else {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator MintNFT()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + nftContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var mintFunction = new ContractDefinitions.Contracts.testNFT.ContractDefinition.MintFunction { };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                ContractDefinitions.Contracts.testNFT.ContractDefinition.MintFunction>(
                    mintFunction, nftContract);
            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator MintItem()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Contract: " + itemContract);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var mintFunction = new MintItemFunction
            {
                Id = 2,
                Amount = 1
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<MintItemFunction>(mintFunction, itemContract);
            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }
    private IEnumerator IncreaseAllowance(string contractAddress)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Increase allowance for: " + contractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {

            var increaseAllowanceFunction = new IncreaseAllowanceFunction
            {
                Spender = contractAddress,
                AddedValue = ToWei(10000000)    // increase the allowance 10m
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<IncreaseAllowanceFunction>(
                increaseAllowanceFunction, tokenContract);
            if (contractTransactionUnityRequest.Exception == null)
            {
                print(contractTransactionUnityRequest.Result);
                if (contractAddress == nftContract)
                    mintNFTButtonText.text = "Mint NFT";
                else
                    mintItemButtonText.text = "Mint Item";
            }
            else
            {
                print(contractTransactionUnityRequest.Exception.Message);
            }
        }
    }

    // Allowance Functions
    private bool checkAllowance(string contractAddress)
    {
        print("Check Allowance: " + contractAddress);
        StartCoroutine(ReadAllowance(contractAddress));

        // If the allowance more than 1m token, than go ahead
        if (contractAddress == nftContract && nftAllowance > ToWei(1000000))
        {
            print("Allowance TRUE for NFT Contract!");
            return true;
        }
        else if (contractAddress == itemContract && itemAllowance > ToWei(1000000))
        {
            print("Allowance TRUE for Item Contract!");
            return true;
        }
        else
        {
            print("Allowance FALSE !!!! Contract: " + contractAddress);
            return false;
        }
    }






    //******  TOOLS TO USE  ******//
    // Essentials
    public void ConnectWalletButton() {
        if (MetamaskInterop.IsMetamaskAvailable()) {
            MetamaskInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));

            StartCoroutine(AddChain());
        }
        else {
            print("Metamask is not available, please install it");
        }
        /**
        if (!checkAllowance(nftContract))
            mintNFTButtonText.text = "Approve";
        else
            mintNFTButtonText.text = "Mint NFT";

        if (!checkAllowance(itemContract))
            mintItemButtonText.text = "Approve";
        else
            mintItemButtonText.text = "Mint Item";
        */
    }
    private IEnumerator AddChain() {
        var addRequest = new WalletAddEthereumChainUnityRequest(GetUnityRpcRequestClientFactory());
        
        var chainParams = new AddEthereumChainParameter 
        {
            ChainId = new HexBigInteger("0x66EED"), // In hexadecimal !!
            ChainName = "Arbitrum Goerli Testnet",
            RpcUrls = new List<string> { "https://goerli-rollup.arbitrum.io/rpc" },
            BlockExplorerUrls = new List<string> { "https://goerli.arbiscan.io/" },
            NativeCurrency = new NativeCurrency 
            {
                Name = "Ether",
                Symbol = "ETH",
                Decimals = 18
            }
        };

        yield return addRequest.SendRequest(chainParams);
        print("Chain Added! " + addRequest.Result);
    }
    public void EthereumEnabled(string addressSelected) {
        if (!_isMetamaskInitialised) {
            MetamaskInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
            MetamaskInterop.GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));
            _isMetamaskInitialised = true;
        }
        NewAccountSelected(addressSelected);
    }
    private void DisplayError(string errorMessage) { }
    private void NewAccountSelected(string accountAddress) {
        print("New Account executed: " + accountAddress);

        _selectedAccountAddress = accountAddress;
        levelManager.walletConnected(accountAddress);
        chainReader.OnWalletChange(accountAddress);

        print("Sent!");
    }
    private void ChainChanged(string chainId) {
        _currentChainId = new HexBigInteger(chainId).Value;
        try {
            //simple workaround to show suported configured chains
            print("Current Chain: " + _currentChainId.ToString());
            StartCoroutine(GetBlockNumber());

           if (_currentChainId != desiredChainID) { print("Adding Chain... "); StartCoroutine(AddChain()); }
        }
        catch (Exception ex) {
            DisplayError(ex.Message);
        }
    }
    private IEnumerator GetBlockNumber() {
        var blockNumberRequest = new EthBlockNumberUnityRequest(GetUnityRpcRequestClientFactory());
        yield return blockNumberRequest.SendRequest();
        print("Block Number: " + blockNumberRequest.Result.Value);
    }
    public IContractTransactionUnityRequest GetContractTransactionUnityRequest() {
        if (MetamaskInterop.IsMetamaskAvailable()) {
            return new MetamaskTransactionUnityRequest(_selectedAccountAddress, GetUnityRpcRequestClientFactory());
        }
        else {
            print("Metamask is not available, please install it");
            return null;
        }
    }
    public IUnityRpcRequestClientFactory GetUnityRpcRequestClientFactory() {
        if (MetamaskInterop.IsMetamaskAvailable())        {
            return new MetamaskRequestRpcClientFactory(_selectedAccountAddress, null, 1000);
        }
        else {
            print("Metamask is not available, please install it");
            return null;
        }
    }

    // Getters and Setters
    public string GetConnectedAddress() { return _selectedAccountAddress; }

    // Conversion Tools
    private static BigInteger ToWei(double value) { return (BigInteger)(value * Math.Pow(10, 18)); }
    private static double FromWei(BigInteger value) { return ((double)value / Math.Pow(10, 18)); }
}
