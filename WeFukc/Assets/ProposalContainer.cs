using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ProposalContainer : MonoBehaviour
{
    public BigInteger proposalID { get; set; }
    public string description { get; set; }
    public string endingTime { get; set; }
    public BigInteger votes { get; set; }
    public BigInteger participationRate { get; set; }

    public void Button_DAOvote(bool _isApproving)
    {
        FindObjectOfType<BlockchainManager>().Button_DAOvote(proposalID, _isApproving);
    }
}
