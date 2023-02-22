using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Member : MonoBehaviour {
    [SerializeField] TextMeshProUGUI addressText;
    [SerializeField] TextMeshProUGUI roleText;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI shareText;
    
    public string address { get; set; }
    public bool isActive { get; set; }
    public string role { get; set; }
    public int point { get; set; }
    public double share { get; set; }

    public Member(string address, string role, int point, double share)
    {
        this.address = address;
        this.role = role;
        this.point = point;
        this.share = share;

        addressText.text = address;
        roleText.text = role;
        pointText.text = point.ToString();
        shareText.text = share.ToString() + "%";
    }
}
