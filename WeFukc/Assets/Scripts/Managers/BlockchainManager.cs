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

public class BlockchainManager : MonoBehaviour
{
    public static BlockchainManager instance;

    // --- Essentials --- //
    bool _isMetamaskInitialised;
    string _selectedAccountAddress = "";
    string _currentContractAddress = "";
    private BigInteger _currentChainId;
    private BigInteger desiredChainID = 421613; // Arbi Goerli

    #region Contract Addresses
    string bossContractAddress = "0xd5DdaF6df51220a17E2292cF4A64ae6510c5415e";
    string clanContractAddress = "0xAe7F338bEc15Aa8aC310A0D2139fcdf3D7E3a447";
    string licenseContractAddress = "0xfBeede26a1F88745c2091a39E47fd10eFC7e8E5F";
    string communityContractAddress = "0xdC0F3C29b0D2a615cB7b46e59D7F7271962730F9";
    string DAOContractAddress = "0x40436Cb71F18594185ef6098631aADDB6d35Ca0B";
    string executorContractAddress = "0xB52f5ff2aa6Ed6017cc7816A9E3F995e0c4A85BD";
    string itemContractAddress = "0x1727bAaa06c10519762d8233301f3f1E9e59c17c";
    string lordContractAddress = "0x6D11053Bb0546eE4E1B856702A96a8b17341e6d0";
    string rentContractAddress = "0x267dB8A829Ad924792a5926a12b32a9Def003021";
    string roundContractAddress = "0x4FD38fF7367bD2a716A278C82A8D7ae580492760";
    string tokenContractAddress = "0x1A573AF12D8B317Ff9A3057c569d6E3EC2fc3504";
    #endregion

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
    public void Button_ClanViewMemberReward() 
    { 
        BigInteger clanID = BigInteger.Parse(clanClaimMemberRewardClanIDInput.text);
        BigInteger roundNumber = BigInteger.Parse(clanClaimMemberRewardRoundInput.text);

        if (clanID == 0 || roundNumber == 0) { print("Invalid Input !!!"); }

        StartCoroutine(ViewMemberRewardCall(clanID, roundNumber)); 
    }
    public void Button_ClanViewInfo() { StartCoroutine(ViewClanInfoCall(int.Parse(clanIDSearchInput.text))); }

    // Round

    // DAO
    public void Button_DAOnewProposal() { StartCoroutine(NewProposalCall()); }
    public void Button_DAOvote(BigInteger _proposalID, bool _approve) { 
        // Check if the account has enough DAO tokens
        StartCoroutine(VoteCall(_proposalID, _approve)); 
    }

    // Item
    public void Button_ItemMint(BigInteger id, BigInteger amount) { StartCoroutine(MintItemCall(id, amount)); }
    public void Button_ItemBurn(List<BigInteger> ids, List<BigInteger> amounts) { StartCoroutine(BurnItemsCall(ids, amounts)); }
    public void Button_ItemBalanceOf(BigInteger id) { StartCoroutine(ItemBalanceOfCall(id)); }
    public void Button_ItemCheckMintAllowance() { StartCoroutine(ItemMintAllowanceCheckCall()); }
    public void Button_ItemIncreaseTokenAllowance() { StartCoroutine(ItemMintAllowanceCall()); }



    /*
     * 
     *              CHANGE ALL REFERENCES NON_BUTTON FUNCTION TO COROUTINE FUNCTIONS
     *              
     * */


    //******    BLOCKCHAIN FUNCTIONS    ******//
    // General
    public IEnumerator TokenBalanceOfCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of - Token Contract: " + tokenContractAddress);

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
            }, tokenContractAddress);

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
        print("Balance Of - DAO Contract: " + DAOContractAddress);

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
            }, DAOContractAddress);

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
        print("Item Contract Allowance - Token Contract: " + tokenContractAddress);

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
                Spender = itemContractAddress
            }, tokenContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var allowance = FromWei(dtoResult.ReturnValue1);

            if (allowance > 1000000000) // If the allowance is more than 1 billion token
            {
                print("Allowance is given!");
                chainReader.WriteItemMintAllowance(true);
            }
            print("Allowance Of " + _selectedAccountAddress + " : " + allowance);
        }
    }
    public IEnumerator BossSupplyCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Boss Supply Call - Boss Contract: " + bossContractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Boss.ContractDefinition.TotalSupplyFunction,
                Contracts.Contracts.Boss.ContractDefinition.TotalSupplyOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Boss.ContractDefinition
                .TotalSupplyFunction()
            { }, bossContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var supply = dtoResult.ReturnValue1;

            chainReader.OnBossSupplyReturn((int)supply);
            
            print("Current Boss Supply: " + supply);
        }
    }

    //------ CLAN CONTRACT ------//
    // Write
    private IEnumerator CreateClanCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Creating Clan - Clan Contract: " + clanContractAddress);
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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Declaring Clan - Clan Contract: " + clanContractAddress);
        // Paramaters
        print("Clan ID: " + BigInteger.Parse(clanIDSearchInput.text));

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.DeclareClanFunction
            {
                ClanID = BigInteger.Parse(clanIDSearchInput.text)
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Set Member - Clan Contract: " + clanContractAddress);
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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Set Executor - Clan Contract: " + clanContractAddress);
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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Set Mod - Clan Contract: " + clanContractAddress);
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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Give Member Point - Clan Contract: " + clanContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Claim Member Reward - Clan Contract: " + clanContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Update Clan Info - Clan Contract: " + clanContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Transfer Leadership - Clan Contract: " + clanContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("Disband - Clan Contract: " + clanContractAddress);

        // Parameters
        print("Clan ID: " + chainReader.displayClan.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickClan.ContractDefinition.DisbandClanFunction
            {
                ClanID = (BigInteger)chainReader.displayClan.id // Get the displayed clan ID
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, clanContractAddress);

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
        print("View Member Reward - Clan Contract: " + clanContractAddress);
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
            }, clanContractAddress);

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
        print("Get Clan Of - Clan Contract: " + clanContractAddress);

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
            }, clanContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var clanID = dtoResult.ReturnValue1;

            // If the account has a clan and info not set yet
            if (clanID > 0 && !chainReader.clanInfoSet) 
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
        print("View Clan Info - Clan Contract: " + clanContractAddress);
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
            }, clanContractAddress);

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
        print("Get Clan Points - Clan Contract: " + clanContractAddress);
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
            }, clanContractAddress);

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
        print("Lord Mint - Lord Contract: " + lordContractAddress);
        print("Given BigInteger: " + _amountToSend.ToString());

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickLord.ContractDefinition.LordMintFunction
            {
                AmountToSend = _amountToSend
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, lordContractAddress);

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
        print("Mint License - Lord Contract: " + lordContractAddress);
        print("Amount: " + _amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickLord.ContractDefinition.MintClanLicenseFunction
            {
                Amount = _amount
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, lordContractAddress);

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
        print("DAO Vote - Lord Contract: " + lordContractAddress);
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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, lordContractAddress);

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

    // Read
    public IEnumerator GetLordSupply()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Lord Contract: " + lordContractAddress);

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
            {  }, lordContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var supply = dtoResult.ReturnValue1;
                                // Base         // After 50th   *   mint cost increase
            lordSupplyText.text = (0.05 + (((double)supply - 50) * 0.001)).ToString();
            lordCurrentCostText.text = supply.ToString() + "/256";
            print("Lord Supply: " + supply);
        }
    }
    public IEnumerator LordBalanceOfCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of - Lord Contract: " + lordContractAddress);

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
            { }, lordContractAddress);

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
        print("ID call - Lord Contract: " + lordContractAddress);
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
            }, lordContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var id = dtoResult.ReturnValue1;

            chainReader.OnLordIDReturn((int)id);
            print("Owned ID: " + (int)id);
        }
    }
    public IEnumerator LordNumberOfClansCall(Lord lord)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Number Of Clans - Lord Contract: " + lordContractAddress);
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
            }, lordContractAddress);

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
        print("Number Of License - License Contract: " + licenseContractAddress);
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
            }, licenseContractAddress);

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
        print("Collected Taxes - Clan Contract: " + clanContractAddress);
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
            }, clanContractAddress);

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
        print("New Proposal - DAO Contract: " + DAOContractAddress);

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
            var callFunction = new Contracts.Contracts.StickDAO.ContractDefinition.NewProposalFunction
            {
                Description = proposalDescription.text,
                ProposalType = type
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, DAOContractAddress);

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
        print("Vote - DAO Contract: " + DAOContractAddress);
        print("Proposal ID: " + _proposalID);
        print("Approve?: " + _approve);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickDAO.ContractDefinition.VoteFunction
            {
                ProposalID = _proposalID,
                IsApproving = _approve
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, DAOContractAddress);

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
        print("Balance Of - DAO Contract: " + DAOContractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickDAO.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.StickDAO.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickDAO.ContractDefinition
                .BalanceOfFunction()
            {
                Account = _selectedAccountAddress
            }, DAOContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var DAObalance = dtoResult.ReturnValue1;
            print("DAO Balance: " + DAObalance);
        }
    }
    public IEnumerator GetLastProposalBasics(BigInteger proposalAmount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Last Proposal Basics - DAO Contract: " + DAOContractAddress);
        print("Proposal Amount: " + proposalAmount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickDAO.ContractDefinition.ViewLastProposalsBasicsFunction,
                Contracts.Contracts.StickDAO.ContractDefinition.ViewLastProposalsBasicsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickDAO.ContractDefinition
                .ViewLastProposalsBasicsFunction()
            {
                ProposalAmount = proposalAmount
            }, DAOContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var amount = dtoResult.ReturnValue1.Count;

            chainReader.OnProposalReturn(
                dtoResult.ReturnValue1, 
                dtoResult.ReturnValue2, 
                dtoResult.ReturnValue3, 
                dtoResult.ReturnValue4, 
                dtoResult.ReturnValue5
            );

            print("Received Amount: " + amount);
            print("Last Proposal ID: " + dtoResult.ReturnValue1[amount - 1]);
            print("Last Proposal Description: " + dtoResult.ReturnValue2[amount - 1]);
            print("Last Proposal Start Time: " + dtoResult.ReturnValue3[amount - 1]);
            print("Last Proposal Ending Time: " + dtoResult.ReturnValue4[amount - 1]);
            print("Last Proposal Status: " + dtoResult.ReturnValue5[amount - 1]);
        }
    }
    public IEnumerator GetLastProposalNumbers(BigInteger proposalAmount, List<ProposalContainer> props)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Last Proposal Numbers - DAO Contract: " + DAOContractAddress);
        print("Proposal Amount: " + proposalAmount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickDAO.ContractDefinition.ViewLastProposalsNumbersFunction,
                Contracts.Contracts.StickDAO.ContractDefinition.ViewLastProposalsNumbersOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickDAO.ContractDefinition
                .ViewLastProposalsNumbersFunction()
            {
                ProposalAmount = proposalAmount
            }, DAOContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var amount = dtoResult.ReturnValue1.Count;

            for (int i = 0; i < amount; i++)
            {
                props[i].participants = (int)dtoResult.ReturnValue1[i];
                props[i].forVotes = (int)dtoResult.ReturnValue3[i];
                props[i].againstVotes = (int)dtoResult.ReturnValue2[i] - props[i].forVotes;

                props[i].DisplayInfo();
            }

            print("Received Amount: " + amount);
            print("Last Proposal participants: " + dtoResult.ReturnValue1[amount - 1]);
            print("Last Proposal total votes: " + dtoResult.ReturnValue2[amount - 1]);
            print("Last Proposal for votes: " + dtoResult.ReturnValue3[amount - 1]);
        }
    }



    //------ ROUND CONTRACT ------//
    // Write
    public IEnumerator FundBossCall(BigInteger level, BigInteger bossID, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Fund Boss - Round Contract: " + roundContractAddress);

        print("Level: " + level);
        print("Boss ID: " + bossID);
        print("Amount: " + amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickRound.ContractDefinition.FundBossFunction
            {
                LevelNumber = level,
                BossID = bossID,
                FundAmount = ToWei((double)amount)
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, roundContractAddress);

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
    public IEnumerator DefundBossCall(BigInteger level, BigInteger bossID, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Defund Boss - Round Contract: " + roundContractAddress);

        print("Level: " + level);
        print("Boss ID: " + bossID);
        print("Amount: " + amount);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickRound.ContractDefinition.FundBossFunction
            {
                LevelNumber = level,
                BossID = bossID,
                FundAmount = ToWei((double)amount)
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, roundContractAddress);

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
        print("Claim Backer Reward- Round Contract: " + roundContractAddress);

        print("Round: " + roundNumber);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.StickRound.ContractDefinition.ClaimBackerRewardFunction
            {
                RoundNumber = roundNumber
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, roundContractAddress);

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
    public IEnumerator GetCurrentBackerRewardCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Backer Reward - Round Contract: " + roundContractAddress);

        BigInteger roundNumber = BigInteger.Parse(backerRoundInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickRound.ContractDefinition.ViewCurrentBackerRewardsFunction,
                Contracts.Contracts.StickRound.ContractDefinition.ViewCurrentBackerRewardsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickRound.ContractDefinition
                .ViewCurrentBackerRewardsFunction()
            { }, roundContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;

            chainReader.OnBackerRewardReturn(dtoResult.ReturnValue1);
            print("Level 1 Backer Reward: " + dtoResult.ReturnValue1[0]);
        }
    }
    public IEnumerator GetCurrentRoundNumberCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Current Round Number - Round Contract: " + roundContractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickRound.ContractDefinition.ViewRoundNumberFunction,
                Contracts.Contracts.StickRound.ContractDefinition.ViewRoundNumberOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickRound.ContractDefinition
                .ViewRoundNumberFunction()
            { }, roundContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            chainReader.currentRound = (int)dtoResult.ReturnValue1;

            print("Current Round: " + dtoResult.ReturnValue1);
        }
    }
    public IEnumerator GetLevelCandidatesCall(BigInteger roundNumber, BigInteger levelNumber)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Election (getting candidates) - Round Contract: " + roundContractAddress);
        // Paramters
        print("Round number:" + roundNumber);
        print("Level number:" + levelNumber);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickRound.ContractDefinition.ViewElectionFunction,
                Contracts.Contracts.StickRound.ContractDefinition.ViewElectionOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickRound.ContractDefinition
                .ViewElectionFunction()
            { }, roundContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            chainReader.OnLevelCandidateReturnReturn((int)levelNumber, dtoResult.ReturnValue1);

            print("Round " + roundNumber + ", level " + levelNumber + " results:");
            foreach (BigInteger candidateID in dtoResult.ReturnValue1)
            {
                print("Candidate: " + candidateID);
            }
        }
    }
    public IEnumerator GetCandidateFunds(BigInteger roundNumber, BossContainer candidate)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Candidate Funds - Round Contract: " + roundContractAddress);
        // Paramters
        print("Round number:" + roundNumber);
        print("Level number:" + candidate.selectedLevel);
        print("Candidate ID:" + candidate.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickRound.ContractDefinition.ViewCandidateFundsFunction,
                Contracts.Contracts.StickRound.ContractDefinition.ViewCandidateFundsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickRound.ContractDefinition
                .ViewCandidateFundsFunction()
            {
                RoundNumber = roundNumber,
                LevelNumber = candidate.selectedLevel,
                CandidateID = candidate.id,
            }, roundContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            candidate.OnAllFundsReturn((double)dtoResult.ReturnValue1);

            print("Round: " + roundNumber + "  level: " + candidate.selectedLevel + "  Candidate: " + candidate.id);
            print("Fund: " + dtoResult.ReturnValue1);
        }
    }
    public IEnumerator GetElectionUserFunds(BigInteger roundNumber, BossContainer candidate)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("View Candidate Funds - Round Contract: " + roundContractAddress);
        // Paramters
        print("Round number:" + roundNumber);
        print("Level number:" + candidate.selectedLevel);
        print("Candidate ID:" + candidate.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.StickRound.ContractDefinition.ViewCandidateFundsFunction,
                Contracts.Contracts.StickRound.ContractDefinition.ViewCandidateFundsOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.StickRound.ContractDefinition
                .ViewCandidateFundsFunction()
            {
                RoundNumber = roundNumber,
                LevelNumber = candidate.selectedLevel,
                CandidateID = candidate.id,
            }, roundContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            candidate.OnAllFundsReturn((double)dtoResult.ReturnValue1);

            print("Round: " + roundNumber + "  level: " + candidate.selectedLevel + "  Candidate: " + candidate.id);
            print("Fund: " + dtoResult.ReturnValue1);
        }
    }


    //------ BOSS CONTRACT ------//
    // Read
    public IEnumerator GetBossRektCall(BossContainer boss)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Get Boss Rekt - Boss Contract: " + bossContractAddress);
        print("Boss ID: " + boss.id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Boss.ContractDefinition.NumOfRektFunction,
                Contracts.Contracts.Boss.ContractDefinition.NumOfRektOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Boss.ContractDefinition
                .NumOfRektFunction()
            {
                ReturnValue1 = boss.id
            }, bossContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            var numOfRekt = dtoResult.ReturnValue1;

            boss.OnBossRektReturn((int)numOfRekt);
            print("Number Of Rekt of id(" + boss.id + "): " + numOfRekt);

        }
    }

    // public List<byte[]> GetProof(byte[] hashLeaf) --> hashLeaf ??
    // 



    //------ ITEM CONTRACT ------//
    // Write
    private IEnumerator MintItemCall(BigInteger id, BigInteger amount)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Mint - Items Contract: " + itemContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, itemContractAddress);

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
        print("Mint - Items Contract: " + itemContractAddress);

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

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, itemContractAddress);

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
        print("Giving Token Allowance to Item Cont - Token Contract: " + tokenContractAddress);
        print("Item Contract: " + itemContractAddress);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Token.ContractDefinition.IncreaseAllowanceFunction
            {
                Spender = itemContractAddress,
                AddedValue = ToWei(1000000000)  // 1 billion allowance                
            };

            yield return contractTransactionUnityRequest.
                SignAndSendTransaction(callFunction, tokenContractAddress);

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
    private IEnumerator ItemBalanceOfCall(BigInteger id)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Balance Of Batch - Items Contract: " + itemContractAddress);
        print("Item ID: " + id);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Items.ContractDefinition.BalanceOfFunction,
                Contracts.Contracts.Items.ContractDefinition.BalanceOfOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Items.ContractDefinition
                .BalanceOfFunction()
            {
                Account = _selectedAccountAddress,
                Id = id
            }, itemContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            int balance = (int)dtoResult.ReturnValue1;

            chainReader.WriteItemBalance((int)id, balance);
            print("Item (ID: " + id + ") Balance :" + balance);
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
