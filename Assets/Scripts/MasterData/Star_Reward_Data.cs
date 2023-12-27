using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Star_Reward_Data : System.IDisposable
{
	///	<summary>
	///	star reward id
	///	</summary>
	public int star_reward_id {get; set;}
	///	<summary>
	///	reward group id
	///	</summary>
	public int star_reward_group_id {get; set;}
	///	<summary>
	///	별 개수
	///	</summary>
	public int star_count {get; set;}
	///	<summary>
	///	아이템 타입
	///	</summary>
	public ITEM_TYPE item_type {get; set;}
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	아이템 개수
	///	</summary>
	public int item_count {get; set;}

	private bool disposed = false;

	public Star_Reward_Data()
	{
		star_reward_id = 0;
		star_reward_group_id = 0;
		star_count = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		item_count = 0;
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
		sb.AppendFormat("[star_count] = <color=yellow>{0}</color>", star_count).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[item_count] = <color=yellow>{0}</color>", item_count).AppendLine();
		return sb.ToString();
	}
}

