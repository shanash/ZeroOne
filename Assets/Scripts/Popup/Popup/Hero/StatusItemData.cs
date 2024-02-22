using Gpm.Ui;

public class StatusItemData : InfiniteScrollData
{
    public string Subject { get; private set; }
    public string Value { get; private set; }

    public bool IsFocus { get; private set; }

    public StatusItemData(string subject, string value, bool is_focus = false)
    {
        Subject = subject;
        Value = value;
        IsFocus = is_focus;
    }
}
