public class Item_Type_Data : System.IDisposable
{
	///	<summary>
	///	아이템 타입
	///	</summary>
	public readonly ITEM_TYPE item_type;
	///	<summary>
	///	이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	툴팁
	///	</summary>
	public readonly string tooltip_text;
	///	<summary>
	///	판매 가능 여부
	///	</summary>
	public readonly bool sellable;
	///	<summary>
	///	최대 보유 한도
	///	</summary>
	public readonly double max_bounds;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Item_Type_Data(Raw_Item_Type_Data raw_data)
	{
		item_type = raw_data.item_type;
		name_kr = raw_data.name_kr;
		tooltip_text = raw_data.tooltip_text;
		sellable = raw_data.sellable;
		max_bounds = raw_data.max_bounds;
		icon_path = raw_data.icon_path;
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

