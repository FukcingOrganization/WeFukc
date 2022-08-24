using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [Header("Keys_Text")]
    [SerializeField] private TextMeshProUGUI Jump_Key_Text;
    [SerializeField] private TextMeshProUGUI Left_Key_Text;
    [SerializeField] private TextMeshProUGUI Right_Key_Text;
    [SerializeField] private TextMeshProUGUI Punch_Key_Text;
    [SerializeField] private TextMeshProUGUI Kick_Key_Text;
    [SerializeField] private TextMeshProUGUI Combo_Key_Text;
    [SerializeField] private TextMeshProUGUI Ýnteraction_Key_Text;
    [SerializeField] private TextMeshProUGUI Defense_Key_Text;


    private void Awake()
    {
        input = new PlayerInput();
        UpdateKeyText();
    }

    private void UpdateKeyText()
    {
        Jump_Key_Text.text = input.Player.Jump.GetBindingDisplayString();
        Left_Key_Text.text = input.Player.Move.GetBindingDisplayString(2);
        Right_Key_Text.text = input.Player.Move.GetBindingDisplayString(1);
        Punch_Key_Text.text = input.Player.Punch.GetBindingDisplayString();
        Kick_Key_Text.text = input.Player.Kick.GetBindingDisplayString();
        Combo_Key_Text.text = input.Player.Combo.GetBindingDisplayString();
        Ýnteraction_Key_Text.text = input.Player.Ýnteraction.GetBindingDisplayString();
        Defense_Key_Text.text = input.Player.Defense.GetBindingDisplayString();
    }
}
