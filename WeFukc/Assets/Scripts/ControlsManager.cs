using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject RebindPanel;
    [Header("Keys_Text")]
    [SerializeField] private TextMeshProUGUI Jump_Key_Text;
    [SerializeField] private TextMeshProUGUI Left_Key_Text;
    [SerializeField] private TextMeshProUGUI Right_Key_Text;
    [SerializeField] private TextMeshProUGUI Punch_Key_Text;
    [SerializeField] private TextMeshProUGUI Kick_Key_Text;
    [SerializeField] private TextMeshProUGUI Combo_Key_Text;
    [SerializeField] private TextMeshProUGUI Interaction_Key_Text;
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
        Interaction_Key_Text.text = input.Player.Interaction.GetBindingDisplayString();
        Defense_Key_Text.text = input.Player.Defense.GetBindingDisplayString();
    }

    public void Rebind_Jump()
    {
        RebindPanel.SetActive(true);
        input.Player.Jump.Disable();
        input.Player.Jump.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Jump.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Jump.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Move_Left()
    {
        RebindPanel.SetActive(true);
        input.Player.Move.Disable();
        input.Player.Move.PerformInteractiveRebinding(2)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Move.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Move.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Move_Right()
    {
        RebindPanel.SetActive(true);
        input.Player.Move.Disable();
        input.Player.Move.PerformInteractiveRebinding(1)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Move.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Move.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Punch()
    {
        RebindPanel.SetActive(true);
        input.Player.Punch.Disable();
        input.Player.Punch.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Punch.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Punch.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Kick()
    {
        RebindPanel.SetActive(true);
        input.Player.Kick.Disable();
        input.Player.Kick.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Kick.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Kick.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Combo()
    {
        RebindPanel.SetActive(true);
        input.Player.Combo.Disable();
        input.Player.Combo.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Combo.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Combo.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Interaction()
    {
        RebindPanel.SetActive(true);
        input.Player.Interaction.Disable();
        input.Player.Interaction.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Interaction.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Interaction.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void Rebind_Defense()
    {
        RebindPanel.SetActive(true);
        input.Player.Defense.Disable();
        input.Player.Defense.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(op =>
            {
                input.Player.Defense.Enable();
                RebindPanel.SetActive(false);
            })
            .OnComplete(callback =>
            {
                callback.Dispose();
                input.Player.Defense.Enable();
                RebindPanel.SetActive(false);
                UpdateKeyText();
            }).Start();
    }

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            input.LoadBindingOverridesFromJson(rebinds);
        }
        UpdateKeyText();
    }

    public void SaveRebinds()
    {
        var rebinds = input.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
