#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class L2d_Love_State_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	ID
	///	</summary>
	public int id => _id;
	int _id;

	///	<summary>
	///	상호작용이 진행되는 L2d 스킨 아이디
	///	</summary>
	public int l2d_id => _l2d_id;
	int _l2d_id;

	///	<summary>
	///	캐릭터 호감도 타입
	///	</summary>
	public LOVE_LEVEL_TYPE love_level_type => _love_level_type;
	LOVE_LEVEL_TYPE _love_level_type;

	///	<summary>
	///	기본 상태 아이디
	///	</summary>
	public int state_id => _state_id;
	int _state_id;

	private bool disposed = false;

	public L2d_Love_State_Data(Raw_L2d_Love_State_Data raw_data)
	{
		_id = raw_data.id;
		_l2d_id = raw_data.l2d_id;
		_love_level_type = raw_data.love_level_type;
		_state_id = raw_data.state_id;
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
		sb.AppendFormat("[id] = <color=yellow>{0}</color>", id).AppendLine();
		sb.AppendFormat("[l2d_id] = <color=yellow>{0}</color>", l2d_id).AppendLine();
		sb.AppendFormat("[love_level_type] = <color=yellow>{0}</color>", love_level_type).AppendLine();
		sb.AppendFormat("[state_id] = <color=yellow>{0}</color>", state_id).AppendLine();
		return sb.ToString();
	}
}

