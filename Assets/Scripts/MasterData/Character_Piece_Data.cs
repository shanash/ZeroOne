[System.Serializable]
public class Character_Piece_Data : System.IDisposable
{
	///	<summary>
	///	캐릭터 아이디
	///	</summary>
	public int player_character_id {get; set;}
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
	///	완전체 필요 개수
	///	</summary>
	public int make_need {get; set;}
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Character_Piece_Data()
	{
		player_character_id = 0;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sell_price = 0;
		make_need = 0;
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
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[make_need] = <color=yellow>{0}</color>", make_need).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

