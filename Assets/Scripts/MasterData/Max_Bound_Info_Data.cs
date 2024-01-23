public class Max_Bound_Info_Data : System.IDisposable
{
	///	<summary>
	///	재화 타입
	///	</summary>
	public readonly REWARD_TYPE reward_type;
	///	<summary>
	///	최대값
	///	</summary>
	public readonly double base_max;

	private bool disposed = false;

	public Max_Bound_Info_Data(Raw_Max_Bound_Info_Data raw_data)
	{
		reward_type = raw_data.reward_type;
		base_max = raw_data.base_max;
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
		sb.AppendFormat("[reward_type] = <color=yellow>{0}</color>", reward_type).AppendLine();
		sb.AppendFormat("[base_max] = <color=yellow>{0}</color>", base_max).AppendLine();
		return sb.ToString();
	}
}

