#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class L2d_Char_Skin_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	L2d 스킨 아이디
	///	</summary>
	public int l2d_id => _l2d_id;
	int _l2d_id;

	///	<summary>
	///	스킨 프리팹 경로
	///	</summary>
	public string l2d_skin_path => _l2d_skin_path;
	string _l2d_skin_path;

	///	<summary>
	///	스킨 전용 배경 경로
	///	</summary>
	public string l2d_bg_path => _l2d_bg_path;
	string _l2d_bg_path;

	///	<summary>
	///	스킨 전용 인트로 경로
	///	</summary>
	public string l2d_intro_path => _l2d_intro_path;
	string _l2d_intro_path;

	private bool disposed = false;

	public L2d_Char_Skin_Data(Raw_L2d_Char_Skin_Data raw_data)
	{
		_l2d_id = raw_data.l2d_id;
		_l2d_skin_path = raw_data.l2d_skin_path;
		_l2d_bg_path = raw_data.l2d_bg_path;
		_l2d_intro_path = raw_data.l2d_intro_path;
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
		sb.AppendFormat("[l2d_id] = <color=yellow>{0}</color>", l2d_id).AppendLine();
		sb.AppendFormat("[l2d_skin_path] = <color=yellow>{0}</color>", l2d_skin_path).AppendLine();
		sb.AppendFormat("[l2d_bg_path] = <color=yellow>{0}</color>", l2d_bg_path).AppendLine();
		sb.AppendFormat("[l2d_intro_path] = <color=yellow>{0}</color>", l2d_intro_path).AppendLine();
		return sb.ToString();
	}
}

