using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHeroSkillDataManager : ManagerBase
{
    List<UserHeroSkillData> User_Hero_Skill_Data_List = new List<UserHeroSkillData>();
    public UserHeroSkillDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }
}
