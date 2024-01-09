[System.Serializable]
public class Item_Type_Data : System.IDisposable
{
    ///	<summary>
    ///	아이템 타입
    ///	</summary>
    public ITEM_TYPE item_type { get; set; }
    ///	<summary>
    ///	이름
    ///	</summary>
    public string name_kr { get; set; }
    ///	<summary>
    ///	툴팁
    ///	</summary>
    public string tooltip_text { get; set; }
    ///	<summary>
    ///	판매 가능 여부
    ///	</summary>
    public bool sellable { get; set; }
    ///	<summary>
    ///	최대 보유 한도
    ///	</summary>
    public double max_bounds { get; set; }
    ///	<summary>
    ///	아이콘
    ///	</summary>
    public string icon_path { get; set; }

    private bool disposed = false;

    public Item_Type_Data()
    {
        item_type = ITEM_TYPE.NONE;
        name_kr = string.Empty;
        tooltip_text = string.Empty;
        sellable = false;
        icon_path = string.Empty;
    }

    public void Dispose()
    {
        Dispose(true);
        System.GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // todo dispose resouces
            }
            disposed = true;
        }
    }
    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
        sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
        sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
        sb.AppendFormat("[sellable] = <color=yellow>{0}</color>", sellable).AppendLine();
        sb.AppendFormat("[max_bounds] = <color=yellow>{0}</color>", max_bounds).AppendLine();
        sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
        return sb.ToString();
    }
}

