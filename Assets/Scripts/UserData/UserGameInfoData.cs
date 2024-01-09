using FluffyDuck.Util;

public class UserGameInfoData : UserDataBase
{
    public string Nickname { get; protected set; } = string.Empty;
    public SecureVar<int> Level { get; protected set; } = null;

    public SecureVar<int> Exp { get; protected set; } = null;

    public UserGameInfoData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Nickname = string.Empty;
        Level.Set(1);
        Exp.Set(0);
    }

    protected override void Destroy()
    {

    }

    protected override void InitSecureVars()
    {
        if (Level == null)
        {
            Level = new SecureVar<int>();
        }
        if (Exp == null)
        {
            Exp = new SecureVar<int>();
        }
    }

    protected override void InitMasterData()
    {

    }

    public int GetLevel()
    {
        return Level.Get();
    }
    public int GetExp()
    {
        return Exp.Get();
    }

    public void SetNickname(string nick)
    {
        if (string.IsNullOrEmpty(nick))
        {
            return;
        }
        Nickname = nick;
        Is_Update_Data = true;
    }

    public ERROR_CODE AddExp(int xp)
    {
        if (xp < 0)
        {
            return ERROR_CODE.FAILED;
        }

        int _exp = GetExp();
        _exp += xp;

        //  level check

        Exp.Set(_exp);

        return ERROR_CODE.SUCCESS;
    }
}
