using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using System;

public class ProposalContainer : MonoBehaviour
{
    public int id { get; set; }
    public string description { get; set; }
    public int startTime { get; set; }
    public int endingTime { get; set; }
    public int status { get; set; }
    public int forVotes { get; set; }
    public int againstVotes { get; set; }
    public int participants { get; set; }

    // Total votes ??

    [SerializeField] TextMeshProUGUI idText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI endingTimeText;
    [SerializeField] TextMeshProUGUI forVoteText;
    [SerializeField] TextMeshProUGUI againstVoteText;
    [SerializeField] TextMeshProUGUI participantsText;
    [SerializeField] TextMeshProUGUI resultText;


    int[] proposalTypeLenghts = new int[] { // TEST - Change it with final value
        60,         // 1 min
        3600,       // 1 hour
        86400,      // 1 day
        259200,     // 3 days
        60,         // 1 min
        20,         // 20 secs
        259200      // 3 days
    }; // TEST - Change it with final value

    bool justEnded;

    void Update()
    {
        // If time has done or not set, skip display
        if (justEnded || endingTime == 0) { return; }

        DisplayEndingTime();
    }

    private void DisplayEndingTime()
    {
        DateTime endDate = UnixTimeStampToDateTime((double)endingTime);

        if (DateTime.Now > endDate)
        {
            justEnded = true;
            // move this one to the completed panel
        }

        int remainingSeconds = (endDate - DateTime.Now).Seconds;
        string remaining = "";

        if (remainingSeconds > 86400)
            remaining = " (in " + (endDate - DateTime.Now).Days + " days)";
        else if (remainingSeconds > 3600)
            remaining = " (in " + (endDate - DateTime.Now).Hours + " hours)";
        else if (remainingSeconds > 60)
            remaining = " (in " + (endDate - DateTime.Now).Minutes + " minutes)";
        else
            remaining = " (in " + remainingSeconds + " seconds)";

        endingTimeText.text = endDate.ToString("MMMM dd, yyyy") + remaining;
    }

    public void Button_DAOvote(bool _isApproving)
    {
        StartCoroutine(FindObjectOfType<BlockchainManager>().VoteCall(this, _isApproving));
    }

    public void OnProposalUpdate(Contracts.Contracts.StickDAO.
        ContractDefinition.ProposalsOutputDTO info
    ) {
        id = (int)info.Id;
        description = info.Description;
        startTime = (int)info.StartTime;
        endingTime = (int)info.StartTime + proposalTypeLenghts[(int)info.ProposalType];
        status = info.Status;
        forVotes = (int)info.YayCount;
        againstVotes = (int)info.NayCount;
        participants = (int)info.Participants;

        DisplayInfo();
    }

    public void DisplayInfo()
    {
        idText.text = id.ToString();
        descriptionText.text = description;
        forVoteText.text = forVotes.ToString();
        againstVoteText.text = againstVotes.ToString();
        participantsText.text = participants.ToString();

        if (status == 3) { resultText.text = "RECEJTED";  resultText.color = Color.red; }
        if (status == 2) { resultText.text = "PASSED";  resultText.color = Color.green; }
    }

    // Tools to use
    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
