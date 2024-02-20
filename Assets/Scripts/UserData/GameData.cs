using FluffyDuck.Util;
using System.Collections.Generic;


public class GameData : Singleton<GameData>
{
    List<ManagerBase> User_Data_Manager_List;

    GameData() { }

    protected override void Initialize()
    {
        User_Data_Manager_List = new List<ManagerBase>();
        InitGameData();
    }

    void InitGameData()
    {
        //  player data
        {
            var mng = new UserPlayerInfoDataManager(USER_DATA_MANAGER_TYPE.USER_PLAYER_INFO_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  goods data
        {
            var mng = new UserGoodsDataManager(USER_DATA_MANAGER_TYPE.USER_GOODS_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }
        //  item data
        {
            var mng = new UserItemDataManager(USER_DATA_MANAGER_TYPE.USER_ITEM_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  charge item data
        {
            var mng = new UserChargeItemDataManager(USER_DATA_MANAGER_TYPE.USER_CHARGE_ITEM_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  hero skill data
        {
            var mng = new UserHeroSkillDataManager(USER_DATA_MANAGER_TYPE.USER_HERO_SKILL_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  hero data
        {
            var mng = new UserHeroDataManager(USER_DATA_MANAGER_TYPE.USER_HERO_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  hero deck mount data
        {
            var mng = new UserDeckDataManager(USER_DATA_MANAGER_TYPE.USER_DECK_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  memorial data
        {
            var mng = new UserMemorialDataManager(USER_DATA_MANAGER_TYPE.USER_MEMORIAL_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  memorial data
        {
            var mng = new UserL2dDataManager(USER_DATA_MANAGER_TYPE.USER_L2D_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }

        //  story stage data
        {
            var mng = new UserStoryStageDataManager(USER_DATA_MANAGER_TYPE.USER_STORY_STAGE_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }
        //  boss stage data
        {
            var mng = new UserBossStageDataManager(USER_DATA_MANAGER_TYPE.USER_BOSS_STAGE_DATA_MANAGER);
            if (!mng.Load())
            {
                mng.InitDataManager();
            }
            User_Data_Manager_List.Add(mng);
        }
    }

    T FindUserDataManager<T>(USER_DATA_MANAGER_TYPE utype) where T : ManagerBase
    {
        return (T)User_Data_Manager_List.Find(x => x.GetManagerType() == utype);
    }

    public UserPlayerInfoDataManager GetUserGameInfoDataManager()
    {
        return FindUserDataManager<UserPlayerInfoDataManager>(USER_DATA_MANAGER_TYPE.USER_PLAYER_INFO_DATA_MANAGER);
    }
    public UserHeroDataManager GetUserHeroDataManager()
    {
        return FindUserDataManager<UserHeroDataManager>(USER_DATA_MANAGER_TYPE.USER_HERO_DATA_MANAGER);
    }
    public UserDeckDataManager GetUserHeroDeckMountDataManager()
    {
        return FindUserDataManager<UserDeckDataManager>(USER_DATA_MANAGER_TYPE.USER_DECK_DATA_MANAGER);
    }

    public UserMemorialDataManager GetUserMemorialDataManager()
    {
        return FindUserDataManager<UserMemorialDataManager>(USER_DATA_MANAGER_TYPE.USER_MEMORIAL_DATA_MANAGER);
    }

    public UserL2dDataManager GetUserL2DDataManager()
    {
        return FindUserDataManager<UserL2dDataManager>(USER_DATA_MANAGER_TYPE.USER_L2D_DATA_MANAGER);
    }

    public UserStoryStageDataManager GetUserStoryStageDataManager()
    {
        return FindUserDataManager<UserStoryStageDataManager>(USER_DATA_MANAGER_TYPE.USER_STORY_STAGE_DATA_MANAGER);
    }

    public UserGoodsDataManager GetUserGoodsDataManager()
    {
        return FindUserDataManager<UserGoodsDataManager>(USER_DATA_MANAGER_TYPE.USER_GOODS_DATA_MANAGER);
    }

    public UserItemDataManager GetUserItemDataManager()
    {
        return FindUserDataManager<UserItemDataManager>(USER_DATA_MANAGER_TYPE.USER_ITEM_DATA_MANAGER);
    }

    public UserChargeItemDataManager GetUserChargeItemDataManager()
    {
        return FindUserDataManager<UserChargeItemDataManager>(USER_DATA_MANAGER_TYPE.USER_CHARGE_ITEM_DATA_MANAGER);
    }
    public UserHeroSkillDataManager GetUserHeroSkillDataManager()
    {
        return FindUserDataManager<UserHeroSkillDataManager>(USER_DATA_MANAGER_TYPE.USER_HERO_SKILL_DATA_MANAGER);
    }

    public UserBossStageDataManager GetUserBossStageDataManager()
    {
        return FindUserDataManager<UserBossStageDataManager>(USER_DATA_MANAGER_TYPE.USER_BOSS_STAGE_DATA_MANAGER);
    }

    public void Save()
    {
        int cnt = User_Data_Manager_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Data_Manager_List[i].Save();
        }
    }
}
