public class Sta_Potion_Data : System.IDisposable
{
	///	<summary>
	///	스테미너 회복 물약 id
	///	</summary>
	public readonly int sta_potion_id;
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
	///	사용 효과
	///	</summary>
	public readonly int use_effect;
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Sta_Potion_Data(Raw_Sta_Potion_Data raw_data)
	{
		sta_potion_id = raw_data.sta_potion_id;
		name_kr = raw_data.name_kr;
		tooltip_text = raw_data.tooltip_text;
		sell_price = raw_data.sell_price;
		use_effect = raw_data.use_effect;
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
		sb.AppendFormat("[sta_potion_id] = <color=yellow>{0}</color>", sta_potion_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[use_effect] = <color=yellow>{0}</color>", use_effect).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

