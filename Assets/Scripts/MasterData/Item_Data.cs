public class Item_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public readonly int item_id;
	///	<summary>
	///	이름 string ID
	///	</summary>
	public readonly string name_id;
	///	<summary>
	///	아이템 타입
	///	</summary>
	public readonly ITEM_TYPE_V2 item_type;
	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public readonly int max_num;
	///	<summary>
	///	int 파라메터
	///	</summary>
	public readonly int int_var1;
	///	<summary>
	///	int 파라메터
	///	</summary>
	public readonly int int_var2;
	///	<summary>
	///	소비 시간(분)
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public readonly int expire_time;
	///	<summary>
	///	소비기한
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public readonly int schedule_id;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Item_Data(Raw_Item_Data raw_data)
	{
		item_id = raw_data.item_id;
		name_id = raw_data.name_id;
		item_type = raw_data.item_type;
		max_num = raw_data.max_num;
		int_var1 = raw_data.int_var1;
		int_var2 = raw_data.int_var2;
		expire_time = raw_data.expire_time;
		schedule_id = raw_data.schedule_id;
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

