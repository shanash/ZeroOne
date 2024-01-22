public class Character_Piece_Data : System.IDisposable
{
	///	<summary>
	///	캐릭터 아이디
	///	</summary>
	public readonly int player_character_id;
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
	///	완전체 필요 개수
	///	</summary>
	public readonly int make_need;
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Character_Piece_Data(Raw_Character_Piece_Data raw_data)
	{
		player_character_id = raw_data.player_character_id;
		name_kr = raw_data.name_kr;
		tooltip_text = raw_data.tooltip_text;
		sell_price = raw_data.sell_price;
		make_need = raw_data.make_need;
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
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[make_need] = <color=yellow>{0}</color>", make_need).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

