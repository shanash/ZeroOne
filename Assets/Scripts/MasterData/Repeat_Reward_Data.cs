public class Repeat_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 고유 인덱스
	///	</summary>
	public readonly int repeat_reward_id;
	///	<summary>
	///	보상 그룹 아이디
	///	</summary>
	public readonly int repeat_reward_group_id;
	///	<summary>
	///	타입
	///	</summary>
	public readonly ITEM_TYPE item_type;
	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public readonly int item_id;
	///	<summary>
	///	최소 수량
	///	</summary>
	public readonly int min_count;
	///	<summary>
	///	최대 수량
	///	</summary>
	public readonly int max_count;

	private bool disposed = false;

	public Repeat_Reward_Data(Raw_Repeat_Reward_Data raw_data)
	{
		repeat_reward_id = raw_data.repeat_reward_id;
		repeat_reward_group_id = raw_data.repeat_reward_group_id;
		item_type = raw_data.item_type;
		item_id = raw_data.item_id;
		min_count = raw_data.min_count;
		max_count = raw_data.max_count;
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
		sb.AppendFormat("[repeat_reward_id] = <color=yellow>{0}</color>", repeat_reward_id).AppendLine();
		sb.AppendFormat("[repeat_reward_group_id] = <color=yellow>{0}</color>", repeat_reward_group_id).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[min_count] = <color=yellow>{0}</color>", min_count).AppendLine();
		sb.AppendFormat("[max_count] = <color=yellow>{0}</color>", max_count).AppendLine();
		return sb.ToString();
	}
}

