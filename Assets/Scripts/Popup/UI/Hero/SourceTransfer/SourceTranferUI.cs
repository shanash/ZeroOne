using FluffyDuck.UI;

public class SourceTranferUI : PopupBase
{
    protected override void Initialize()
    {
        base.Initialize();

        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
    }

    public override void Spawned()
    {
        base.Spawned();

        Initialize();
    }
}
