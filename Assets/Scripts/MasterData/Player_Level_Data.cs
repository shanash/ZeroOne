﻿public class Player_Level_Data : System.IDisposable
{
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

	public Player_Level_Data(Raw_Player_Level_Data raw_data)
	{
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
		sb.AppendFormat("[level] = <color=yellow>{0}</color>", level).AppendLine();
		sb.AppendFormat("[accum_exp] = <color=yellow>{0}</color>", accum_exp).AppendLine();
		sb.AppendFormat("[need_exp] = <color=yellow>{0}</color>", need_exp).AppendLine();
		return sb.ToString();
	}
}

