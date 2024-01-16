[System.Serializable]
public class Item_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id {get; set;}
	///	<summary>
	///	아이템 타입
	///	</summary>
	public GOODS_TYPE goods_type {get; set;}
	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num {get; set;}
	///	<summary>
	///	int 파라메터
	///	</summary>
	public int int_var1 {get; set;}
	///	<summary>
	///	int 파라메터
	///	</summary>
	public int int_var2 {get; set;}
	///	<summary>
	///	소비 시간(분)
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time {get; set;}
	///	<summary>
	///	소비기한
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public int schedule_id {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Item_Data()
	{
		item_id = 0;
		name_id = string.Empty;
		goods_type = GOODS_TYPE.NONE;
		max_num = 0;
		int_var1 = 0;
		int_var2 = 0;
		expire_time = 0;
		schedule_id = 0;
		icon_path = string.Empty;
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
		sb.AppendFormat("[goods_type] = <color=yellow>{0}</color>", goods_type).AppendLine();
		sb.AppendFormat("[max_num] = <color=yellow>{0}</color>", max_num).AppendLine();
		sb.AppendFormat("[int_var1] = <color=yellow>{0}</color>", int_var1).AppendLine();
		sb.AppendFormat("[int_var2] = <color=yellow>{0}</color>", int_var2).AppendLine();
		sb.AppendFormat("[expire_time] = <color=yellow>{0}</color>", expire_time).AppendLine();
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

