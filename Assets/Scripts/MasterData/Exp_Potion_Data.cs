[System.Serializable]
public class Exp_Potion_Data : System.IDisposable
{
	///	<summary>
	///	경험치 물약 id
	///	</summary>
	public int exp_potion_id {get; set;}
	///	<summary>
	///	이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	툴팁
	///	</summary>
	public string tooltip_text {get; set;}
	///	<summary>
	///	판매가격
	///	</summary>
	public int sell_price {get; set;}
	///	<summary>
	///	사용 효과
	///	</summary>
	public int use_effect {get; set;}
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Exp_Potion_Data()
	{
		exp_potion_id = 0;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sell_price = 0;
		use_effect = 0;
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
		sb.AppendFormat("[exp_potion_id] = <color=yellow>{0}</color>", exp_potion_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[use_effect] = <color=yellow>{0}</color>", use_effect).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

