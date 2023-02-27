using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;

public class Item : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] TMP_InputField mintAmountInput;

    public void Button_MintItem()
    {
        FindObjectOfType<BlockchainManager>().Button_ItemMint(
            (BigInteger)id, BigInteger.Parse(mintAmountInput.text)
        );
    }
}
