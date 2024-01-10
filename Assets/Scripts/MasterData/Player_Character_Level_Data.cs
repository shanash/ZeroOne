using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Level_Data : System.IDisposable
{
	///	<summary>
	///	ID
	///	</summary>
	public int player_character_level_id {get; set;}
	///	<summary>
	///	레벨
	///	</summary>
	public int level {get; set;}
	///	<summary>
	///	누적 경험치
	///	</summary>
	public double accum_exp {get; set;}
	///	<summary>
	///	필요 경험치
	///	</summary>
	public double need_exp {get; set;}

	private bool disposed = false;

	public Player_Character_Level_Data()
	{
		player_character_level_id = 0;
		level = 0;
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

