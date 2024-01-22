public class Goods_Data : System.IDisposable
{
	///	<summary>
	///	재화 타입
	///	</summary>
	public readonly GOODS_TYPE goods_type;
	///	<summary>
	///	이름 string ID
	///	</summary>
	public readonly string name_id;
	///	<summary>
	///	최대값
	///	</summary>
	public readonly double max_bound;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Goods_Data(Raw_Goods_Data raw_data)
	{
		goods_type = raw_data.goods_type;
		name_id = raw_data.name_id;
		max_bound = raw_data.max_bound;
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
		sb.AppendFormat("[goods_type] = <color=yellow>{0}</color>", goods_type).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[max_bound] = <color=yellow>{0}</color>", max_bound).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

