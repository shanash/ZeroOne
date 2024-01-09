using UnityEngine;


public enum FLAG_TYPE
{
    NONE = 0,
    TEAM_FLAG,
    DEATH_FLAG
}

public class BaseFlagNode : MonoBehaviour
{
    [SerializeField, Tooltip("Flag Type")]
    protected FLAG_TYPE Flag_Type = FLAG_TYPE.NONE;

    protected StageProceedingManager Proceed_Mng;
    public void SetStageProceedingManager(StageProceedingManager mng)
    {
        Proceed_Mng = mng;
    }

    public FLAG_TYPE GetFlagType() { return Flag_Type; }


}
