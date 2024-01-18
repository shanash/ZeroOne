public class Player_Character_Level_Data : System.IDisposable
{
	///	<summary>
	///	ID
	///	</summary>
	public readonly int player_character_level_id;
	///	<summary>
	///	레벨
	///	</summary>
	public readonly int level;
	///	<summary>
	///	누적 경험치
	///	</summary>
	public readonly double accum_exp;
	///	<summary>
	///	필요 경험치
	///	</summary>
	public readonly double need_exp;

	private bool disposed = false;

	public Player_Character_Level_Data(Raw_Player_Character_Level_Data raw_data)
	{
		player_character_level_id = raw_data.player_character_level_id;
		level = raw_data.level;
		accum_exp = raw_data.accum_exp;
		need_exp = raw_data.need_exp;
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
		sb.AppendFormat("[player_character_level_id] = <color=yellow>{0}</color>", player_character_level_id).AppendLine();
		sb.AppendFormat("[level] = <color=yellow>{0}</color>", level).AppendLine();
		sb.AppendFormat("[accum_exp] = <color=yellow>{0}</color>", accum_exp).AppendLine();
		sb.AppendFormat("[need_exp] = <color=yellow>{0}</color>", need_exp).AppendLine();
		return sb.ToString();
	}
}

