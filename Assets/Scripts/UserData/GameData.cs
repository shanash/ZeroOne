using System;
using System.Collections;
using System.Collections.Generic;


public class GameData : IDisposable
{
    static GameData _instance = null;
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameData();
                _instance.InitGameData();
            }
            return _instance;
        }
    }

    List<ManagerBase> User_Data_Manager_List = new List<ManagerBase>();
    bool Is_Init;

    private bool disposed = false;
    public GameData()
    {

    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //  관리되는 자원 해제

            }
            disposed = true;
        }
    }

    void InitGameData()
    {
        if (!Is_Init)
        {
            //  player data
            {
                var mng = new UserGameInfoDataManager(USER_DATA_MANAGER_TYPE.USER_GAME_INFO_DATA_MANAGER);
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
            //  story stage data
            {
                var mng = new UserStoryStageDataManager(USER_DATA_MANAGER_TYPE.USER_STORY_STAGE_DATA_MANAGER);
                if (!mng.Load())
                {
                    mng.InitDataManager();
                }
                User_Data_Manager_List.Add(mng);
            }

            Is_Init = true;
        }
    }

    T FindUserDataManager<T>(USER_DATA_MANAGER_TYPE utype) where T : ManagerBase
    {
        return (T)User_Data_Manager_List.Find(x => x.GetManagerType() == utype);
    }

    public UserGameInfoDataManager GetUserGameInfoDataManager()
    {
        return FindUserDataManager<UserGameInfoDataManager>(USER_DATA_MANAGER_TYPE.USER_GAME_INFO_DATA_MANAGER);
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
    public UserStoryStageDataManager GetUserStoryStageDataManager()
    {
        return FindUserDataManager<UserStoryStageDataManager>(USER_DATA_MANAGER_TYPE.USER_STORY_STAGE_DATA_MANAGER);
    }
}
