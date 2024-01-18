public class First_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 고유 인덱스
	///	</summary>
	public readonly int first_reward_id;
	///	<summary>
	///	보상 그룹 아이디
	///	</summary>
	public readonly int first_reward_group_id;
	///	<summary>
	///	타입
	///	</summary>
	public readonly ITEM_TYPE item_type;
	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public readonly int item_id;
	///	<summary>
	///	지급 수량
	///	</summary>
	public readonly string item_count;

	private bool disposed = false;

	public First_Reward_Data(Raw_First_Reward_Data raw_data)
	{
		first_reward_id = raw_data.first_reward_id;
		first_reward_group_id = raw_data.first_reward_group_id;
		item_type = raw_data.item_type;
		item_id = raw_data.item_id;
		item_count = raw_data.item_count;
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
		sb.AppendFormat("[first_reward_id] = <color=yellow>{0}</color>", first_reward_id).AppendLine();
		sb.AppendFormat("[first_reward_group_id] = <color=yellow>{0}</color>", first_reward_group_id).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[item_count] = <color=yellow>{0}</color>", item_count).AppendLine();
		return sb.ToString();
	}
}

