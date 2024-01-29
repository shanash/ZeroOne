#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Star_Upgrade_Data : System.IDisposable
{
	///	<summary>
	///	업그레이드 이전 별성
	///	</summary>
	public int current_star_grade => _current_star_grade;
	int _current_star_grade;

	///	<summary>
	///	필요 캐릭터 조각 수량
	///	</summary>
	public int need_char_piece => _need_char_piece;
	int _need_char_piece;

	///	<summary>
	///	업그레이드 비용
	///	</summary>
	public int need_gold => _need_gold;
	int _need_gold;

	private bool disposed = false;

	public Star_Upgrade_Data(Raw_Star_Upgrade_Data raw_data)
	{
		_current_star_grade = raw_data.current_star_grade;
		_need_char_piece = raw_data.need_char_piece;
		_need_gold = raw_data.need_gold;
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
		sb.AppendFormat("[current_star_grade] = <color=yellow>{0}</color>", current_star_grade).AppendLine();
		sb.AppendFormat("[need_char_piece] = <color=yellow>{0}</color>", need_char_piece).AppendLine();
		sb.AppendFormat("[need_gold] = <color=yellow>{0}</color>", need_gold).AppendLine();
		return sb.ToString();
	}
}

