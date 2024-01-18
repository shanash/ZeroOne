public class Expendable_Item_Data : System.IDisposable
{
	///	<summary>
	///	소모용 아이템 id
	///	</summary>
	public readonly int expendable_item_id;
	///	<summary>
	///	이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	툴팁
	///	</summary>
	public readonly string tooltip_text;
	///	<summary>
	///	판매가격
	///	</summary>
	public readonly int sell_price;
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Expendable_Item_Data(Raw_Expendable_Item_Data raw_data)
	{
		expendable_item_id = raw_data.expendable_item_id;
		name_kr = raw_data.name_kr;
		tooltip_text = raw_data.tooltip_text;
		sell_price = raw_data.sell_price;
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
		sb.AppendFormat("[expendable_item_id] = <color=yellow>{0}</color>", expendable_item_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

