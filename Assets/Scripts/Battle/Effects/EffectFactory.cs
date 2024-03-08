using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{

    [SerializeField, Tooltip("Left Center")]
    Transform Left_Center;
    [SerializeField, Tooltip("Right Center")]
    Transform Right_Center;
    [SerializeField, Tooltip("World Zero")]
    Transform World_Zero;

    List<EffectBase> Used_Effect_List = new List<EffectBase>();

    float Effect_Speed_Multiple = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];

    public EffectBase CreateEffect(string path, float scale = 1f)
    {
        return CreateEffect(path, this.transform, scale);
    }

    public EffectBase CreateEffect(string path, Transform parent, float scale = 1f)
    {
        EffectBase effect = null;

        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(path, parent);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        effect = obj.GetComponent<EffectBase>();
        effect.SetEffectFactory(this);
        effect.SetEffectSpeedMultiple(Effect_Speed_Multiple);

        Used_Effect_List.Add(effect);
        return effect;
    }

    public void UnusedEffectBase(EffectBase obj)
    {
        Used_Effect_List.Remove(obj);
        GameObjectPoolManager.Instance.UnusedGameObject(obj.gameObject);
    }

    public void SetEffectSpeedMultiple(float multiple)
    {
        Effect_Speed_Multiple = multiple;
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Effect_List[i].SetEffectSpeedMultiple(multiple);
        }
    }

    /// <summary>
    /// 모든 이펙트 삭제<br/>
    /// 지속성 효과 이펙트는 삭제하지 않는다.
    /// </summary>
    public void ClearAllEffects()
    {
        List<EffectBase> remove_effects = new List<EffectBase>();
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var effect = Used_Effect_List[i];
            if (effect is SkillEffect_Duration_Base)
            {
                continue;
            }
            remove_effects.Add(effect);
        }

        for (int i = 0; i < remove_effects.Count; i++)
        {
            UnusedEffectBase(remove_effects[i]);
        }
    }


    public Transform GetLeftCenter() { return Left_Center; }
    public Transform GetRightCenter() { return Right_Center; }

    public Transform GetWorldZero() { return World_Zero; }

    public void OnPauseAndHide()
    {
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Effect_List[i].OnPuase();
            Used_Effect_List[i].Show(false);
        }
    }
    public void OnResumeAndShow()
    {
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Effect_List[i].Show(true);
            Used_Effect_List[i].OnResume();
        }
    }


    public void OnPause()
    {
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Effect_List[i].OnPuase();
        }
    }
    public void OnResume()
    {
        int cnt = Used_Effect_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Effect_List[i].OnResume();
        }
    }
}
