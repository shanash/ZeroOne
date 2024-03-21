using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarNode_V2 : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Bar BG")]
    Transform Bar_BG;

    [SerializeField, Tooltip("Red Bar")]
    SpriteRenderer Life_Shield_Bar;

    [SerializeField, Tooltip("Blue Bar(Life Bar)")]
    SpriteRenderer Life_Bar;

    [SerializeField, Tooltip("Buff Icon Container")]
    Transform Buff_Icon_Container;

    const float Fill_Width = 3f;

    Coroutine Flush_Coroutine;

    Coroutine Show_Coroutine;

    HeroBase_V2 Hero;
    /// <summary>
    /// 지속성 효과 아이콘 리스트
    /// </summary>
    List<EnemyDurationSkillIconNode> Used_Duration_Skill_Icons = new List<EnemyDurationSkillIconNode>();

    public void SetHeroBaseV2(HeroBase_V2 hero)
    {
        this.Hero = hero;
        hero.Slot_Events += SkillSlotEventCallback;
    }
    public void ShowLifeBar(float duration)
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = StartCoroutine(StartShowLifeBar(duration));
    }

    public void HideLifeBar()
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = null;
        Bar_BG.gameObject.SetActive(false);
    }


    IEnumerator StartShowLifeBar(float delay)
    {
        Bar_BG.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        Bar_BG.gameObject.SetActive(false);
        Show_Coroutine = null;
    }

    void SkillSlotEventCallback(SKILL_SLOT_EVENT_TYPE etype)
    {
        if (etype == SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE)
        {
            UpdateDurationSkillIcons();
        }
    }
    /// <summary>
    /// 지속성 효과 아이콘 추가
    /// </summary>
    void UpdateDurationSkillIcons()
    {
        if (Hero == null)
        {
            return;
        }
        ClearDurationSkillIcons();
        float interval = 0.7f;

        var pool = GameObjectPoolManager.Instance;
        var dur_list = Hero.GetSkillManager().GetDurationSkillDataList();
        int cnt = dur_list.Count;
        int col = 0;
        int row = 0;
        float x, y;
        for (int i = 0; i < cnt; i++)
        {
            col = i % 5;
            row = i / 5;
            x = col * interval;
            y = row * interval;

            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/EnemyDurationSkillIconNode", Buff_Icon_Container);
            obj.transform.localPosition = new Vector3(x, y, 0);
            var node = obj.GetComponent<EnemyDurationSkillIconNode>();
            node.SetBattleDurationSkillData(dur_list[i]);
            Used_Duration_Skill_Icons.Add(node);
        }
    }

    void ClearDurationSkillIcons()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Duration_Skill_Icons.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Duration_Skill_Icons[i].gameObject);
        }
        Used_Duration_Skill_Icons.Clear();
    }

    /// <summary>
    /// 체력 게이지 비율
    /// </summary>
    /// <param name="per"></param>
    public void SetLifePercent(float per, bool show)
    {
        SetBarPercent(Life_Bar, per);
        if (show)
        {
            ShowLifeBar(2f);
        }
    }

    /// <summary>
    /// 체력 보호막 게이지 비율
    /// </summary>
    /// <param name="per"></param>
    public void SetLifeShieldPercent(float per)
    {
        SetBarPercent(Life_Shield_Bar, per);
        ShowLifeBar(2f);
    }

    /// <summary>
    /// 지징된 바 게이지의 비율 계산하여 업데이트
    /// </summary>
    /// <param name="bar"></param>
    /// <param name="per"></param>
    void SetBarPercent(SpriteRenderer bar, float per)
    {
        float p = Mathf.Clamp01(per) * Fill_Width;
        bar.size = new Vector2(p, bar.size.y);
    }


    public void Spawned()
    {
        SetBarPercent(Life_Shield_Bar, 0f);
        SetBarPercent(Life_Bar, 1f);
        
        HideLifeBar();
    }
    public void Despawned()
    {
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }
        Flush_Coroutine = null;
        HideLifeBar();
    }

    
}
