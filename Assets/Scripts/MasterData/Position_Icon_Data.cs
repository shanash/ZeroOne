[System.Serializable]
public class Position_Icon_Data : System.IDisposable
{
	///	<summary>
	///	포지션 타입
	///	</summary>
	public POSITION_TYPE position_type {get; set;}
	///	<summary>
	///	이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon {get; set;}

	private bool disposed = false;

	public Position_Icon_Data()
	{
		position_type = POSITION_TYPE.NONE;
		name_kr = string.Empty;
		icon = string.Empty;
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

