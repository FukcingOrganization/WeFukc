using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InformationManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [Header("Keys_Text")]
    [SerializeField] private TextMeshProUGUI Movement_info_Text;
    [SerializeField] private TextMeshProUGUI Fight1_info_Text;
    [SerializeField] private TextMeshProUGUI Fight2_info_Text;
    [SerializeField] private TextMeshProUGUI Fight3_info_Text;
    [SerializeField] private TextMeshProUGUI Fight4_info_Text;
    [SerializeField] private TextMeshProUGUI Elevator_info_Text;

    private void Awake()
    {
        input = new PlayerInput();

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            input.LoadBindingOverridesFromJson(rebinds);
        }
        //Color String
        var cs = "#127369";
        Movement_info_Text.text = "<color="+cs+">" + input.Player.Move.GetBindingDisplayString(1) + " -></color> Move Right\n<color=" + cs + ">" + input.Player.Move.GetBindingDisplayString(2) + " -></color> Move Left\n<color=" + cs + ">" + input.Player.Jump.GetBindingDisplayString() + " -></color> Jump";
        Fight1_info_Text.text = "<color=" + cs + ">" + input.Player.Punch.GetBindingDisplayString() + " -></color> Punch \n<color=" + cs + ">" + input.Player.Kick.GetBindingDisplayString() + " -></color> Kick";
        Fight2_info_Text.text = "<color="+cs+ ">* </color>All actions reduce your stamina \n<color=" + cs + ">" + input.Player.Defense.GetBindingDisplayString() + " -></color> Defense and Boost Stamina";
        Fight3_info_Text.text = "<color=" + cs + ">Fast Punch -> </color>Press "+ input.Player.Punch.GetBindingDisplayString() + " while moving \n<color=" + cs + ">Flying Kick -> </color>Press "+ input.Player.Kick.GetBindingDisplayString() + " while moving";
        Fight4_info_Text.text = "<color=" + cs + ">Turning Kick -> </color>"+ input.Player.Combo.GetBindingDisplayString() + " + " + input.Player.Kick.GetBindingDisplayString();
        Elevator_info_Text.text = "Press <color=" + cs + ">"+ input.Player.Interaction.GetBindingDisplayString() + "</color> ->";
    }
}
