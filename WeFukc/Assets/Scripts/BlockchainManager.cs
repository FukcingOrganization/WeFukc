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
    private BigInteger desiredChainID = 421613;
    string[] contracts = new string[] {
      "0xd5DdaF6df51220a17E2292cF4A64ae6510c5415e", // 0- Boss
      "0x977c94Afe8C3B80beF6f2386Fd7d7B016754407A", // 1- Clan
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


    #region LORD 
    // Objects
    public TMPro.TMP_InputField lordPriceInput;
    public TMPro.TextMeshProUGUI lordSupplyText;
    public TMPro.TextMeshProUGUI lordCurrentCostText;
    #endregion

    #region CLAN 
    // Objects - Input
    public TMPro.TMP_InputField ClanIDSearchInput;          // Search Clan
    public TMPro.TMP_InputField CreateClanLordIDInput;      // Create Clan
    public TMPro.TMP_InputField CreateClanNameInput;            
    public TMPro.TMP_InputField CreateClanDescriptionInput;
    public TMPro.TMP_InputField CreateClanMottoInput;
    public TMPro.TMP_InputField CreateClanLogoInput;
    public TMPro.TMP_InputField ClanSetMemberAddressInput;  // Set Member
    public TMPro.TMP_InputField ClanSetExecutorAddressInput;// Set Executor
    public TMPro.TMP_InputField ClanSetModAddressInput;     // Set Mod
    public TMPro.TMP_InputField ClanGiveMemberPointAddressInput;    // Give Member Point - Address
    public TMPro.TMP_InputField ClanGiveMemberPointPointInput;      // Give Member Point - Point
    // Objects - View
    public TMPro.TextMeshProUGUI clanInfoNameText;
    public TMPro.TextMeshProUGUI clanInfoIDText;
    public TMPro.TextMeshProUGUI clanInfoMottoText;
    public TMPro.TextMeshProUGUI clanInfoDescriptionText;


    #endregion



    #region DELETE 
    public TMPro.TextMeshProUGUI tokenBalanceText;
    public TMPro.TextMeshProUGUI nftBalanceText;
    public TMPro.TextMeshProUGUI itemBalanceText;

    public TMPro.TextMeshProUGUI nftAllowanceText;
    public TMPro.TextMeshProUGUI itemAllowanceText;
    public TMPro.TextMeshProUGUI mintNFTButtonText;
    public TMPro.TextMeshProUGUI mintItemButtonText;
    

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
    }

    

    //******    BUTTONS    ******//
    // LORD
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
    public void Button_UpdateLordSupply(){ StartCoroutine(UpdateLordSupply()); }

    // Clan
    public void Button_CreateClan() { StartCoroutine(CreateClanCall()); }
    public void Button_DeclareClan() { StartCoroutine(DeclareClanCall()); }
    public void Button_SetMember(bool assignAsMember) { StartCoroutine(SetMemberCall(assignAsMember)); }
    public void Button_SetExecutor(bool assignAsExecutor) { StartCoroutine(SetExecutorCall(assignAsExecutor)); }
    public void Button_SetMod(bool assignAsMod) { StartCoroutine(SetModCall(assignAsMod)); }
    public void Button_GiveMemberPoint(bool isDecreasing)
    {
        BigInteger point = BigInteger.Parse(ClanGiveMemberPointPointInput.text, System.Globalization.CultureInfo.InvariantCulture);
        
        if (point == 0) { print("PLEASE ENTER SOME POINT !!"); return; }
        StartCoroutine(GiveMemberPointCall(point, isDecreasing)); 
    }

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





    //******    BLOCKCHAIN FUNCTIONS    ******//

    //------ CLAN CONTRACT ------//
    // Write
    private IEnumerator CreateClanCall()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Creating Clan - Clan Contract: " + contracts[1]); // Clan Contract
        // Paramaters
        print("Lord ID: " + BigInteger.Parse(CreateClanLordIDInput.text));
        print("Clan Name: " + CreateClanNameInput.text);
        print("Clan Description: " + CreateClanDescriptionInput.text);
        print("Clan Logo URI: " + CreateClanLogoInput.text);
        print("Clan Motto: " + CreateClanMottoInput.text);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.CreateClanFunction
            {
                LordID = BigInteger.Parse(CreateClanLordIDInput.text),
                ClanName = CreateClanNameInput.text,
                ClanDescription = CreateClanDescriptionInput.text,
                ClanLogoURI = CreateClanLogoInput.text,
                ClanMotto = CreateClanMottoInput.text
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.CreateClanFunction
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
        print("Clan ID: " + BigInteger.Parse(ClanIDSearchInput.text));

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.DeclareClanFunction
            {
                ClanID = BigInteger.Parse(ClanIDSearchInput.text)
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.DeclareClanFunction
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
        print("Clan ID: " + BigInteger.Parse(clanInfoIDText.text));
        print("Member Address: " + ClanSetMemberAddressInput.text);
        print("Set As: " + assignAsMember);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.SetMemberFunction
            {
                ClanID = BigInteger.Parse(clanInfoIDText.text), // Get the displayed clan ID
                Address = ClanSetMemberAddressInput.text,
                SetAsMember = assignAsMember
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.SetMemberFunction
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
        print("Clan ID: " + BigInteger.Parse(clanInfoIDText.text));
        print("Executor Address: " + ClanSetExecutorAddressInput.text);
        print("Set As: " + assignAsExecutor);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.SetClanExecutorFunction
            {
                ClanID = BigInteger.Parse(clanInfoIDText.text), // Get the displayed clan ID
                Address = ClanSetExecutorAddressInput.text,
                SetAsExecutor = assignAsExecutor
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.SetClanExecutorFunction
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
        print("Clan ID: " + BigInteger.Parse(clanInfoIDText.text));
        print("Mod Address: " + ClanSetModAddressInput.text);
        print("Set As: " + assignAsMod);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.SetClanModFunction
            {
                ClanID = BigInteger.Parse(clanInfoIDText.text), // Get the displayed clan ID
                Address = ClanSetModAddressInput.text,
                SetAsMod = assignAsMod
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.SetClanModFunction
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
        print("Clan ID: " + BigInteger.Parse(clanInfoIDText.text));
        print("Member Address: " + ClanGiveMemberPointAddressInput.text);
        print("Point: " + point);
        print("Is decreasing?: " + isDecreasing);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Clan.ContractDefinition.GiveMemberPointFunction
            {
                ClanID = BigInteger.Parse(clanInfoIDText.text), // Get the displayed clan ID
                MemberAddress = ClanGiveMemberPointAddressInput.text,
                Point = point,
                IsDecreasing = isDecreasing
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Clan.ContractDefinition.GiveMemberPointFunction
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
    // Claim Member Reward
    // Update Clan Info
    // Transfer Clan Leadership
    // Disband Clan

    //------ LORD CONTRACT ------//

    // Write
    private IEnumerator LordMintCall(BigInteger _amountToSend)
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Lord Contract: " + contracts[7]); // Lord Contract
        print("Given BigInteger: " + _amountToSend.ToString());

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var callFunction = new Contracts.Contracts.Lord.ContractDefinition.LordMintFunction
            {
                AmountToSend = _amountToSend
            };

            yield return contractTransactionUnityRequest.SignAndSendTransaction<
                Contracts.Contracts.Lord.ContractDefinition.LordMintFunction
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
    // Mint License
    // DAO Vote

    // Read
    public IEnumerator UpdateLordSupply()
    {
        print("Wallet: " + _selectedAccountAddress);
        print("Lord Contract: " + contracts[7]);

        var contractTransactionUnityRequest = GetContractTransactionUnityRequest();

        if (contractTransactionUnityRequest != null)
        {
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.Lord.ContractDefinition.TotalSupplyFunction,
                Contracts.Contracts.Lord.ContractDefinition.TotalSupplyOutputDTO>(
                GetUnityRpcRequestClientFactory(), _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.Lord.ContractDefinition
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

    //------ DAO CONTRACT ------//
    // Write
    // New Porposal
    // Vote
    // Spending Proposal Claim Functions

    //------ ROUND CONTRACT ------//
    // Write
    // Fund Boss
    // Defund Boss
    // Player Reward Claim
    // Backer Reward Claim

    //------ COMMUNITY CONTRACT ------//
    // Write
    // Claim Reward

    //------ ITEM CONTRACT ------//
    // Write
    // Mint
    // Burn



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
        levelManager.SetConnectedAccount(accountAddress);

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
