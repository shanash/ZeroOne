public class Position_Icon_Data : System.IDisposable
{
	///	<summary>
	///	포지션 타입
	///	</summary>
	public readonly POSITION_TYPE position_type;
	///	<summary>
	///	이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon;

	private bool disposed = false;

	public Position_Icon_Data(Raw_Position_Icon_Data raw_data)
	{
		position_type = raw_data.position_type;
		name_kr = raw_data.name_kr;
		icon = raw_data.icon;
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
		sb.AppendFormat("[position_type] = <color=yellow>{0}</color>", position_type).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		return sb.ToString();
	}
}

