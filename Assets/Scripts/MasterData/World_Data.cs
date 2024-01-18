public class World_Data : System.IDisposable
{
	///	<summary>
	///	월드 인덱스
	///	</summary>
	public readonly int world_id;
	///	<summary>
	///	월드 명칭
	///	</summary>
	public readonly string world_name;

	private bool disposed = false;

	public World_Data(Raw_World_Data raw_data)
	{
		world_id = raw_data.world_id;
		world_name = raw_data.world_name;
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
		sb.AppendFormat("[world_id] = <color=yellow>{0}</color>", world_id).AppendLine();
		sb.AppendFormat("[world_name] = <color=yellow>{0}</color>", world_name).AppendLine();
		return sb.ToString();
	}
}

