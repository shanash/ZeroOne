using Gpm.Ui;
using UnityEngine.Events;
using UnityEngine.UI;

public class RelationshipToggleItemData : InfiniteScrollData
{
    public int Index { get; private set; }
    public string Subject { get; private set; }
    public ToggleGroup Toggle_Group { get; private set; }

    public bool Enable { get; set; }

    public bool Selected { get; set; }

    public UnityAction<int, bool> OnToggleChanged { get; private set; }

    public RelationshipToggleItemData(int index, string subject, ToggleGroup toggle_group, bool enable, UnityAction<int, bool> on_toggle_changed)
    {
        Index = index;
        Subject = subject;
        Toggle_Group = toggle_group;
        Enable = enable;
        OnToggleChanged = on_toggle_changed;
        Selected = false;
    }
}
