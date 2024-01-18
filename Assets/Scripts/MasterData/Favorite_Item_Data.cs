public class Favorite_Item_Data : System.IDisposable
{
	///	<summary>
	///	인덱스
	///	</summary>
	public readonly int favorite_item_id;
	///	<summary>
	///	한글명
	///	</summary>
	public readonly string item_name_kr;
	///	<summary>
	///	사용 효과
	///	</summary>
	public readonly int use_effect;
	///	<summary>
	///	툴팁 TEXT
	///	</summary>
	public readonly string item_tooltip_text;
	///	<summary>
	///	판매 가격
	///	</summary>
	public readonly int sell_price;
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Favorite_Item_Data(Raw_Favorite_Item_Data raw_data)
	{
		favorite_item_id = raw_data.favorite_item_id;
		item_name_kr = raw_data.item_name_kr;
		use_effect = raw_data.use_effect;
		item_tooltip_text = raw_data.item_tooltip_text;
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
		sb.AppendFormat("[favorite_item_id] = <color=yellow>{0}</color>", favorite_item_id).AppendLine();
		sb.AppendFormat("[item_name_kr] = <color=yellow>{0}</color>", item_name_kr).AppendLine();
		sb.AppendFormat("[use_effect] = <color=yellow>{0}</color>", use_effect).AppendLine();
		sb.AppendFormat("[item_tooltip_text] = <color=yellow>{0}</color>", item_tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

