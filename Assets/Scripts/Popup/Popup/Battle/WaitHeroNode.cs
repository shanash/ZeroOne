using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitHeroNode : UIBase
{
    UIHeroBase Hero;
    UserHeroData User_Data;
    public void SetUserHeroData(UserHeroData data)
    {
        User_Data = data;
        SpawnUIHero();
    }

    void SpawnUIHero()
    {
        var obj = GameObjectPoolManager.Instance.GetGameObject(User_Data.GetPlayerCharacterData().sd_prefab_path, this.transform);
        obj.transform.localPosition = Vector2.zero;
        Hero = obj.GetComponent<UIHeroBase>();
        MainThreadDispatcher.Instance.AddAction(() =>
        {
            Hero.PlayAnimation(HERO_PLAY_ANIMATION_TYPE.WIN_01);
        });
    }
}
