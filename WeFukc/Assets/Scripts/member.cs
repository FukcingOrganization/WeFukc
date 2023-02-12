using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class member : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI addressText;
    [SerializeField] TMPro.TextMeshProUGUI roleText;
    [SerializeField] TMPro.TextMeshProUGUI pointText;
    [SerializeField] TMPro.TextMeshProUGUI shareText;
    
    public string address { get; set; }
    public string role { get; set; }
    public int point { get; set; }
    public int share { get; set; }

    public member(string address, string role, int point, int share)
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
