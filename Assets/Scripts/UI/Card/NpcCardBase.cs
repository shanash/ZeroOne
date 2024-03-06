using FluffyDuck.Util;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NpcCardBase : UIBase, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("BG Box")]
    protected Image BG_Box;

    [SerializeField, Tooltip("Npc Icon")]
    protected Image Npc_Icon;

    [SerializeField, Tooltip("Boss Tag")]
    protected Image Boss_Tag;

    [SerializeField, Tooltip("Warning Tag")]
    protected Image Warning_Tag;

    protected Npc_Data Data;
    protected bool Is_Boss;

    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);

    public Action<Rect, Npc_Data> OnStartLongPress;
    public Action OnFinishLongPress;
    Coroutine CheckForLongPress = null;

    public void SetNpcID(int npc_id)
    {
        SetNpcData(MasterDataManager.Instance.Get_NpcData(npc_id));
    }

    public void SetNpcData(Npc_Data data)
    {
        Data = data;
        UpdateNpcIcon();
    }

    public void SetBoss(bool boss)
    {
        Is_Boss = boss;
        Boss_Tag.gameObject.SetActive(Is_Boss);
    }

    protected void UpdateNpcIcon()
    {
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) =>
        {
            Npc_Icon.sprite = spr;
        });
    }

    public int GetNpcID()
    {
        if (Data != null)
        {
            return Data.npc_data_id;
        }
        return 0;
    }
    public override void Spawned()
    {
        base.Spawned();
        this.transform.localScale = Init_Scale;
    }
    public override void Despawned()
    {
        base.Despawned();
        SetBoss(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopCoroutine(ref CheckForLongPress);
        CheckForLongPress = StartCoroutine(CoCheckForLongPress());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool is_stopped = StopCoroutine(ref CheckForLongPress);

        // 제대로 OnStartLongPress가 실행되었어야지
        // (CheckForLongPress == null) (is_stopped == false)
        // OnFinishLongPress을 호출
        if (!is_stopped)
        {
            OnFinishLongPress?.Invoke();
        }
    }

    IEnumerator CoCheckForLongPress()
    {
        float elapsed_time = 0;
        while(Tooltip.PRESS_TIME_FOR_SHOW > elapsed_time)
        {
            yield return null;
            elapsed_time += Time.deltaTime;
        }

        var rt = this.GetComponent<RectTransform>();
        OnStartLongPress?.Invoke(GameObjectUtils.GetScreenRect(rt), Data);
        CheckForLongPress = null;
    }

    bool StopCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            return true;
        }

        return false;
    }
}
