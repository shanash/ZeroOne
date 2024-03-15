using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ChangeStartConversation : MonoBehaviour
{
    public string newConversationID = "NewConversationID";

    private DialogueSystemTrigger dialogueSystemTrigger;

    private void Start()
    {
        ChangeStartConversationID();
    }

    public void ChangeStartConversationID()
    {
        // Dialogue System Trigger 컴포넌트를 가져옵니다.
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
        if (dialogueSystemTrigger == null)
        {
            Debug.LogError("Dialogue System Trigger를 찾을 수 없습니다.");
        }

        if (dialogueSystemTrigger != null)
        {
            // 대화 시작 ID를 변경합니다.
            dialogueSystemTrigger.conversation = newConversationID;
            StartConversation();
        }
        else
        {
            Debug.LogError("Dialogue System Trigger가 없으므로 대화를 변경할 수 없습니다.");
        }
    }
    public void StartConversation()
    {
        if (dialogueSystemTrigger != null)
        {
            dialogueSystemTrigger.OnUse();
        }
        else
        {
            Debug.LogError("Dialogue System Trigger가 없으므로 대화를 변경할 수 없습니다.");
        }
    }
}
