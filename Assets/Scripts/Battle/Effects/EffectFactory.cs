using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{

    [SerializeField, Tooltip("Left Center")]
    Transform Left_Center;
    [SerializeField, Tooltip("Right Center")]
    Transform Right_Center;

    List<EffectBase> Used_Effect_List = new List<EffectBase>();

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
        Used_Effect_List.Add(effect);
        return effect;
    }

    public void UnusedEffectBase(EffectBase obj)
    {
        Used_Effect_List.Remove(obj);
        GameObjectPoolManager.Instance.UnusedGameObject(obj.gameObject);
    }

    public Transform GetLeftCenter() { return Left_Center; }
    public Transform GetRightCenter() { return Right_Center; }

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
