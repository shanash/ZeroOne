using Gpm.Ui;

public class StatusItemData : InfiniteScrollData
{
    public string Subject { get; private set; }
    public string Value { get; private set; }

    public StatusItemData(string subject, string value)
    {
        Subject = subject;
        Value = value;
    }
}
