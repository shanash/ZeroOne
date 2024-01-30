#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Player_Character_Love_Level_Data : System.IDisposable
{
	///	<summary>
	///	레벨
	///	</summary>
	public int level => _level;
	int _level;

	///	<summary>
	///	누적 경험치
	///	</summary>
	public double accum_exp => _accum_exp;
	double _accum_exp;

	///	<summary>
	///	필요 경험치
	///	</summary>
	public double need_exp => _need_exp;
	double _need_exp;

	///	<summary>
	///	필요 경험치
	///	</summary>
	public LOVE_LEVEL_TYPE love_level_type => _love_level_type;
	LOVE_LEVEL_TYPE _love_level_type;

	private bool disposed = false;

	public Player_Character_Love_Level_Data(Raw_Player_Character_Love_Level_Data raw_data)
	{
		_level = raw_data.level;
		_accum_exp = raw_data.accum_exp;
		_need_exp = raw_data.need_exp;
		_love_level_type = raw_data.love_level_type;
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
		sb.AppendFormat("[love_level_type] = <color=yellow>{0}</color>", love_level_type).AppendLine();
		return sb.ToString();
	}
}

