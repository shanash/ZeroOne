using FluffyDuck.UI;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDialogueUI : PopupBase
{

    [SerializeField, Tooltip("Dialogue System")]
    DialogueSystemTrigger Dialogue_Trigger;

    [SerializeField, Tooltip("Conversation Controller")]
    ConversationControl Con_Ctrl;

    [SerializeField, Tooltip("Dialogue System Controller")]
    DialogueSystemController System_Ctrl;

    public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;

    bool Use_Skip;

    string Conversation_ID;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }
        Use_Skip = false;
        SetEnableEscKeyExit(false);
        Conversation_ID = (string)data[0];
        if (string.IsNullOrEmpty(Conversation_ID))
        {
            return false;
        }

        return true;
    }


    protected override void ShowPopupEndCallback()
    {
        Dialogue_Trigger.conversation = Conversation_ID;
        Dialogue_Trigger.OnUse();
    }



    public void OnClickSkip()
    {
        if (Use_Skip)
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        System_Ctrl.StopAllConversations();
        Use_Skip = true;
    }

    public void SkipDialogueCallback()
    {
        Closed_Delegate?.Invoke();
        HideAndDestroyPopup();
    }

    public override void Spawned()
    {
        base.Spawned();
        Use_Skip = false;
    }
}
