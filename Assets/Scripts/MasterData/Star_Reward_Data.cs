public class Star_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 고유 인덱스
	///	</summary>
	public readonly int star_reward_id;
	///	<summary>
	///	보상 그룹 아이디
	///	</summary>
	public readonly int star_reward_group_id;
	///	<summary>
	///	별 포인트
	///	</summary>
	public readonly int star_point;
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
	public readonly int item_count;

	private bool disposed = false;

	public Star_Reward_Data(Raw_Star_Reward_Data raw_data)
	{
		star_reward_id = raw_data.star_reward_id;
		star_reward_group_id = raw_data.star_reward_group_id;
		star_point = raw_data.star_point;
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
		sb.AppendFormat("[star_reward_id] = <color=yellow>{0}</color>", star_reward_id).AppendLine();
		sb.AppendFormat("[star_reward_group_id] = <color=yellow>{0}</color>", star_reward_group_id).AppendLine();
		sb.AppendFormat("[star_point] = <color=yellow>{0}</color>", star_point).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[item_count] = <color=yellow>{0}</color>", item_count).AppendLine();
		return sb.ToString();
	}
}

