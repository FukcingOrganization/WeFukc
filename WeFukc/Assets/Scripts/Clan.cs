using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clan : MonoBehaviour
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
    public int totalClanPoints { get; set; }
    public int clanPoint { get; set; }
    public int totalMemberPoints { get; set; }
    public List<Member> members { get; set; }

    public Clan() { }
}
