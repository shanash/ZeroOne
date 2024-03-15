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

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }

        SetEnableEscKeyExit(false);
        string conversation_id = (string)data[0];
        if (string.IsNullOrEmpty(conversation_id))
        {
            return false;
        }
        Dialogue_Trigger.conversation = conversation_id;
        Dialogue_Trigger.OnUse();
        
        return true;
    }


    public void OnClickSkip()
    {
        System_Ctrl.StopAllConversations();
        
    }

    public void SkipDialogueCallback()
    {
        Dialogue_Trigger.conversation = string.Empty;
        HidePopup();
    }

}
