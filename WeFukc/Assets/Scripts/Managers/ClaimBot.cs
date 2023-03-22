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
using Nethereum.Unity.FeeSuggestions;   // added for fee suggestion

public class ClaimBot : MonoBehaviour
{
    string _selectedAccountAddress = "";

    string ethRPC = "https://mainnet.infura.io/v3/1a1dd7e492854a259e3d84f724f624f0";
    string arbiRPC = "https://arbitrum-mainnet.infura.io/v3/1a1dd7e492854a259e3d84f724f624f0";
    string goerliRPC = "https://arbitrum-goerli.infura.io/v3/1a1dd7e492854a259e3d84f724f624f0";
    string umutPRC = "http://65.109.72.5:8547";
    BigInteger arbiChainID = 42161;
    BigInteger arbiGoerliChainID = 421613;

    /*
     *  NOT: HACKLENEN ADRES? BURAYA KOYMA. ONUN TÜM ??LEMLER?N? MANUEL YAPIYORUM.
     */
    string[] keys = new string[] {
        "0xc66c057e46aa87b9f95f6f7825a6c0a93486a18a4a49f6ca90e4461226cca9a2",
        "0x6cef3124ff3e5a47d67eef9501ebf30dc3b80773a835b1bd7871125a3622f7ea"
    };

    string[] addresses = new string[] {
        "0x85be25d0Ef53959dB27D42df1f7da57549154D5f",
        "0x022993d3edaB7056b48E1417224d340E7a3Eae6b"
    };

    string claimContract = "0x67a24CE4321aB3aF51c2D0a4801c3E111D88C9d9";
    string tokenContract = "0x912CE59144191C1204E64559FE8253a0e49E6548";
    string testClaimContract = "0x39a81A5807956F6FCD717a041dE69C9fdFCb11Bc";    // TEST
    string testTokenContract = "0x945EcD6452ABa01390A2Fa511A195d29f06fFCb7";    // TEST
    string blockNumContract = "0xf7C320186dce8aC3646dd65ff8e5D7F3dD7B5F9e";
    BigInteger targetBlockNumber = 16885565;

    bool readLoop = true;
    float checkDelay = 15f;

    [SerializeField] TextMeshProUGUI blockNumberText;
    [SerializeField] TextMeshProUGUI targetNumberText;
    [SerializeField] TextMeshProUGUI logPrefab;
    [SerializeField] GameObject logPanel;

    enum MessageStatus
    {
        Good,
        Normal,
        Bad
    }

    private void Start()
    {
        targetNumberText.text = targetBlockNumber.ToString();
    }
    public void StartClaim()
    {
        StartCoroutine(GetBlockNumber());
    }

    private IEnumerator GetBlockNumber()
    {
        InsertLog("Block numaralari aliniyor...", MessageStatus.Normal, true);
        while (readLoop)
        {
            Debug.Log(checkDelay + " saniye icinde tekrar kontrol ediliyor...");
            yield return new WaitForSeconds(checkDelay);

            EthBlockNumberUnityRequest blockNumberRequest = new EthBlockNumberUnityRequest(ethRPC);    // ETH mainnet
            yield return blockNumberRequest.SendRequest();

            BigInteger blockNum = blockNumberRequest.Result.Value;

            blockNumberText.text = blockNum.ToString();  // write the block number

            //var arbiNumReq = new ContractQueryUnityRequestFactory(arbiRPC);
            
            var queryRequest = new QueryUnityRequest<
                Contracts.Contracts.GetBlockNumber.ContractDefinition.GibMeTheNumberFunction,
                Contracts.Contracts.GetBlockNumber.ContractDefinition.GibMeTheNumberOutputDTO>(
                umutPRC, _selectedAccountAddress
            );

            yield return queryRequest.Query(new Contracts.Contracts.GetBlockNumber.ContractDefinition
                .GibMeTheNumberFunction(){ }, blockNumContract);

            //Getting the dto response already decoded
            BigInteger arbiBlock = queryRequest.Result.ReturnValue1;
            blockNumberText.text = blockNum.ToString() + " / " + arbiBlock.ToString();

            if (arbiBlock >= targetBlockNumber)
            {
                readLoop = false;   // stop reading block number
                blockNumberText.color = Color.red;

                StartCoroutine(sendETH());

                for (int i = 0; i < keys.Length - 1; i++)   // DE???T?R HACKLEEN ADRES ÇIKINCA
                {
                    StartCoroutine(ClaimARB(keys[i], addresses[i]));
                }
            }

            // Change frequency while approaching to the target
            if (arbiBlock >= targetBlockNumber - 1) 
            {
                if (checkDelay == 0.2f) continue;
                checkDelay = 0.2f;
                InsertLog("SON 12 SANIYE !!", MessageStatus.Normal, true);
            }
            else if (arbiBlock >= targetBlockNumber - 2)
            {
                if (checkDelay == 1f) continue;
                checkDelay = 1f;
                InsertLog("Son 24 saniye!", MessageStatus.Normal, true);
            }
            else if (arbiBlock >= targetBlockNumber - 3)
            {
                if (checkDelay == 5f) continue;
                checkDelay = 5f;
                InsertLog("Son 36 saniye!", MessageStatus.Normal, true);
            }
            else if (arbiBlock >= targetBlockNumber - 5)
            {
                if (checkDelay == 10f) continue;
                checkDelay = 10f;
                InsertLog("Son 60 saniye!", MessageStatus.Normal, true);
            }
        }
    }

    private IEnumerator sendETH()
    {
        decimal amount = 0.006m;
        string shortAddress = addresses[0]; // Hacklenen adrss

        InsertLog("Hackelenen adrese transfer icin para aktar?l?yor.", MessageStatus.Normal, true);

        var ethTransfer = new EthTransferUnityRequest(umutPRC, keys[0], arbiChainID);

        // #### GET FEE
        InsertLog("Fee önerileri aliniyor. (eth transfer)" + " (" + shortAddress + ")", MessageStatus.Normal, false);
        Debug.Log("Time Preference");
        TimePreferenceFeeSuggestionUnityRequestStrategy feeSuggestion =
            new TimePreferenceFeeSuggestionUnityRequestStrategy(umutPRC);

        yield return feeSuggestion.SuggestFees();

        if (feeSuggestion.Exception != null)
        {
            Debug.Log(feeSuggestion.Exception.Message);
            yield break;
        }

        //lets get the first one so it is higher priority
        Debug.Log(feeSuggestion.Result.Length);
        if (feeSuggestion.Result.Length > 0)
        {
            Debug.Log(feeSuggestion.Result[0].MaxFeePerGas);
            Debug.Log(feeSuggestion.Result[0].MaxPriorityFeePerGas);
        }
        Nethereum.RPC.Fee1559Suggestions.Fee1559 feeSend = feeSuggestion.Result[0];

        yield return ethTransfer.TransferEther(
            "0x022993d3edaB7056b48E1417224d340E7a3Eae6b",   // HACKED ADDRESS
            amount,
            feeSend.MaxPriorityFeePerGas.Value,
            feeSend.MaxFeePerGas.Value);

        if (ethTransfer.Exception != null)
        {
            InsertLog("HATA:" + " ETH gonderlimedi! (" + shortAddress + ") " + ethTransfer.Exception, MessageStatus.Bad, true);
            yield break;
        }
        else
        {
            InsertLog("BASARILI:" + " ETHER SEND TO (" + shortAddress + ") -- TX: " + ethTransfer.Result, MessageStatus.Good, true);
            StartCoroutine(ClaimARB(keys[1], addresses[1]));
        }
    }

    private IEnumerator ClaimARB(string key, string address)
    {
        bool success = false;
        do
        {
            string shortAddress = address.Substring(0, 6);            
            
            InsertLog(address + " için islemler baslatiliyor.", MessageStatus.Normal, true);
            InsertLog("Cüzdana RPC ile baglaniliyor." + " (" + shortAddress + ")", MessageStatus.Normal, false);
            // #### Get Request
            TransactionSignedUnityRequest transaction =
                new TransactionSignedUnityRequest(umutPRC, key, arbiChainID);

            // #### GET FEE
            InsertLog("Fee önerileri aliniyor." + " (" + shortAddress + ")", MessageStatus.Normal, false);
            Debug.Log("Time Preference");
            TimePreferenceFeeSuggestionUnityRequestStrategy timePreferenceFeeSuggestion =
                new TimePreferenceFeeSuggestionUnityRequestStrategy(umutPRC);

            yield return timePreferenceFeeSuggestion.SuggestFees();

            if (timePreferenceFeeSuggestion.Exception != null)
            {
                Debug.Log(timePreferenceFeeSuggestion.Exception.Message);
                yield break;
            }

            //lets get the first one so it is higher priority
            Debug.Log(timePreferenceFeeSuggestion.Result.Length);
            if (timePreferenceFeeSuggestion.Result.Length > 0)
            {
                Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxFeePerGas);
                Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxPriorityFeePerGas);
            }
            Nethereum.RPC.Fee1559Suggestions.Fee1559 fee = timePreferenceFeeSuggestion.Result[0];

            // #### Set function and fee
            Contracts.Contracts.IClaimContract.ContractDefinition.ClaimFunction callFunction =
                new Contracts.Contracts.IClaimContract.ContractDefinition.ClaimFunction
                {
                    MaxPriorityFeePerGas = fee.MaxPriorityFeePerGas,
                    MaxFeePerGas = fee.MaxFeePerGas
                };

            // #### Send Transaction
            InsertLog("Claim ediliyor." + " (" + shortAddress + ")", MessageStatus.Normal, true);
            yield return transaction.SignAndSendTransaction(callFunction, testClaimContract);

            if (transaction.Exception != null)
            {
                InsertLog("HATA:" + " (" + shortAddress + ") " + transaction.Exception, MessageStatus.Bad, true);
            }
            else
            {
                InsertLog("BASARILI:" + " (" + shortAddress + ") -- TX: " + transaction.Result, MessageStatus.Good, true);
                success = true;

                if (address == addresses[1]) { StartCoroutine(SendARB(key, address)); }
            }
        }
        while (!success);        
    }

    private IEnumerator SendARB(string key, string address)
    {
        InsertLog(address + " tokenlar? temiz hesaba aktariliyor.", MessageStatus.Normal, true);
        string shortAddress = address.Substring(0, 6);
        InsertLog(address + " tokenlar? temiz hesaba aktariliyor.", MessageStatus.Normal, true);
        InsertLog("Cüzdana RPC ile baglaniliyor." + " (" + shortAddress + ")", MessageStatus.Normal, false);
        // #### Get Request
        TransactionSignedUnityRequest transaction =
            new TransactionSignedUnityRequest(umutPRC, key, arbiChainID);

        // #### GET FEE
        InsertLog("Fee önerileri aliniyor." + " (" + shortAddress + ")", MessageStatus.Normal, false);
        Debug.Log("Time Preference");
        TimePreferenceFeeSuggestionUnityRequestStrategy timePreferenceFeeSuggestion =
            new TimePreferenceFeeSuggestionUnityRequestStrategy(umutPRC);

        yield return timePreferenceFeeSuggestion.SuggestFees();

        if (timePreferenceFeeSuggestion.Exception != null)
        {
            Debug.Log(timePreferenceFeeSuggestion.Exception.Message);
            yield break;
        }

        //lets get the first one so it is higher priority
        Debug.Log(timePreferenceFeeSuggestion.Result.Length);
        if (timePreferenceFeeSuggestion.Result.Length > 0)
        {
            Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxFeePerGas);
            Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxPriorityFeePerGas);
        }
        Nethereum.RPC.Fee1559Suggestions.Fee1559 fee = timePreferenceFeeSuggestion.Result[0];

        // #### Set function and fee
        Contracts.Contracts.Token.ContractDefinition.TransferFunction callFunction =
            new Contracts.Contracts.Token.ContractDefinition.TransferFunction
            {
                To = "0x1bC6F80DA87Bdab3C94d7B1b7a513FE4Fc2254f5",
                Amount = ToWei(1500),
                MaxPriorityFeePerGas = fee.MaxPriorityFeePerGas,
                MaxFeePerGas = fee.MaxFeePerGas
            };

        // #### Send Transaction
        InsertLog("Transfer ediliyor." + " (" + shortAddress + ")", MessageStatus.Normal, true);
        yield return transaction.SignAndSendTransaction(callFunction, testTokenContract);

        if (transaction.Exception != null)
        {
            InsertLog("HATA:" + " (" + shortAddress + ") " + transaction.Exception, MessageStatus.Bad, true);
        }
        else
        {
            InsertLog("BASARILI:" + " (" + shortAddress + ") Tokenlar guvende! -- TX: " + transaction.Result, MessageStatus.Good, true);
        }
    }

    void InsertLog(string message, MessageStatus status, bool leavOnScreen) 
    { 
        StartCoroutine(LogRoutine(message, status, leavOnScreen)); 
    }

    IEnumerator LogRoutine(string message, MessageStatus status, bool leavOnScreen)
    {
        // Create the log and write the message
        TextMeshProUGUI newLog = Instantiate(logPrefab, logPanel.transform);
        newLog.text = message;

        // Change color
        if (status == MessageStatus.Good) { newLog.color = Color.green; }
        else if (status == MessageStatus.Bad) { newLog.color = Color.red; }

        if (leavOnScreen) { yield break; }

        // Delete after 10 seconds
        yield return new WaitForSeconds(10f);
        Destroy(newLog);
    }

    // Conversion Tools
    private static BigInteger ToWei(double value) { return (BigInteger)(value * Math.Pow(10, 18)); }
    private static double FromWei(BigInteger value) { return ((double)value / Math.Pow(10, 18)); }

}
