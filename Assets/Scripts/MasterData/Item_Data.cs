#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Item_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id => _item_id;
	int _item_id;

	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	아이템 타입
	///	</summary>
	public ITEM_TYPE_V2 item_type => _item_type;
	ITEM_TYPE_V2 _item_type;

	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num => _max_num;
	int _max_num;

	///	<summary>
	///	int 파라메터
	///	</summary>
	public int int_var1 => _int_var1;
	int _int_var1;

	///	<summary>
	///	int 파라메터
	///	경험치 물약일 경우, 물약 1개를 사용하는데 필요한 비용(골드)값이 입력
	///	</summary>
	public int int_var2 => _int_var2;
	int _int_var2;

	///	<summary>
	///	소비 시간(분)
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time => _expire_time;
	int _expire_time;

	///	<summary>
	///	소비기한
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public int schedule_id => _schedule_id;
	int _schedule_id;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Item_Data(Raw_Item_Data raw_data)
	{
		_item_id = raw_data.item_id;
		_name_id = raw_data.name_id;
		_item_type = raw_data.item_type;
		_max_num = raw_data.max_num;
		_int_var1 = raw_data.int_var1;
		_int_var2 = raw_data.int_var2;
		_expire_time = raw_data.expire_time;
		_schedule_id = raw_data.schedule_id;
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
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[max_num] = <color=yellow>{0}</color>", max_num).AppendLine();
		sb.AppendFormat("[int_var1] = <color=yellow>{0}</color>", int_var1).AppendLine();
		sb.AppendFormat("[int_var2] = <color=yellow>{0}</color>", int_var2).AppendLine();
		sb.AppendFormat("[expire_time] = <color=yellow>{0}</color>", expire_time).AppendLine();
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

