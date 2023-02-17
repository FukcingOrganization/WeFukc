using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanInfo : MonoBehaviour
{
    public string leaderAddress { get; set; }
    public int lordID { get; set; }
    public int firstRound { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string motto { get; set; }
    public string logoURI { get; set; }
    public bool canExecutorsSignalRebellion { get; set; }
    public bool canExecutorsSetPoint { get; set; }
    public bool canModsSetMembers { get; set; }
    public bool isDisbanded { get; set; }
    public member[] members { get; set; }

    public ClanInfo(string leaderAddress, int lordID, int firstRound, int id,
        string name, string description, string motto, string logoURI,
        bool canExecutorsSignalRebellion, bool canExecutorsSetPoint, 
        bool canModsSetMembers, bool isDisbanded
    ) {
        this.leaderAddress = leaderAddress;
        this.lordID = lordID;
        this.firstRound = firstRound;
        this.id = id;
        this.name = name;
        this.description = description;
        this.motto = motto;
        this.logoURI = logoURI;
        this.canExecutorsSignalRebellion = canExecutorsSignalRebellion;
        this.canExecutorsSetPoint = canExecutorsSetPoint;
        this.canModsSetMembers = canModsSetMembers;
        this.isDisbanded = isDisbanded;
    }
}
