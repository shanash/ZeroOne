#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Item_Piece_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	아이템 ID
	///	</summary>
	public int item_piece_id => _item_piece_id;
	int _item_piece_id;

	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	조각낼 대상 아이디
	///	</summary>
	public int target_id => _target_id;
	int _target_id;

	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num => _max_num;
	int _max_num;

	///	<summary>
	///	1개 제작을 하기 위한 수량
	///	</summary>
	public int make_count => _make_count;
	int _make_count;

	///	<summary>
	///	소비 시간(분)
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time => _expire_time;
	int _expire_time;

	///	<summary>
	///	소비기한
	///	> scheduleTable과 연결
	///	> 값이 0 이면, 소비 기한 없음
	///	</summary>
	public int expire_schedule_id => _expire_schedule_id;
	int _expire_schedule_id;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Item_Piece_Data(Raw_Item_Piece_Data raw_data)
	{
		_item_piece_id = raw_data.item_piece_id;
		_name_id = raw_data.name_id;
		_target_id = raw_data.target_id;
		_max_num = raw_data.max_num;
		_make_count = raw_data.make_count;
		_expire_time = raw_data.expire_time;
		_expire_schedule_id = raw_data.expire_schedule_id;
		_icon_path = raw_data.icon_path;
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
		sb.AppendFormat("[item_piece_id] = <color=yellow>{0}</color>", item_piece_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[target_id] = <color=yellow>{0}</color>", target_id).AppendLine();
		sb.AppendFormat("[max_num] = <color=yellow>{0}</color>", max_num).AppendLine();
		sb.AppendFormat("[make_count] = <color=yellow>{0}</color>", make_count).AppendLine();
		sb.AppendFormat("[expire_time] = <color=yellow>{0}</color>", expire_time).AppendLine();
		sb.AppendFormat("[expire_schedule_id] = <color=yellow>{0}</color>", expire_schedule_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

