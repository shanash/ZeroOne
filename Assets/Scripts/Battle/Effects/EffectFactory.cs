using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{

    [SerializeField, Tooltip("Left Center")]
    Transform Left_Center;
    [SerializeField, Tooltip("Right Center")]
    Transform Right_Center;

    List<EffectBase> Used_Effect_List = new List<EffectBase>();

    float Effect_Speed_Multiple = 1f;

    public EffectBase CreateEffect(string path)
    {
        return CreateEffect(path, this.transform);
    }

    public EffectBase CreateEffect(string path, Transform parent)
    {
        EffectBase effect = null;

        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(path, parent);
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

    public Transform GetLeftCenter() { return Left_Center; }
    public Transform GetRightCenter() { return Right_Center; }

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
