
using DocumentFormat.OpenXml.Office2010.Excel;

public class Producer : FactoryProductBase
{
    public static Producer Create(int player_character_id, MEMORIAL_TYPE type)
    {
        return Factory.Create<Producer>(player_character_id, type);
    }
    Producer() { }

    protected override bool Initialize(params object[] args)
    {
        return true;
    }
}
