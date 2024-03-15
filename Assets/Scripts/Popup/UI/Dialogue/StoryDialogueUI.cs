using Cysharp.Threading.Tasks;
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

    [SerializeField]
    GameObject Go_Ctrl;

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

        Debug.Log($"conversation_id : {conversation_id}");
        /*
        Dialogue_Trigger.conversation = conversation_id;
        Dialogue_Trigger.OnUse();
        */

        WaitForEnableContainer(conversation_id).Forget();

        return true;
    }

    async UniTaskVoid WaitForEnableContainer(string name)
    {
        await UniTask.WaitUntil(() => Go_Ctrl.activeInHierarchy);
        System_Ctrl.Awake();
        Dialogue_Trigger.conversation = name;
        Dialogue_Trigger.OnUse();
        System_Ctrl.StartConversation(name);
        System_Ctrl.Start();
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
