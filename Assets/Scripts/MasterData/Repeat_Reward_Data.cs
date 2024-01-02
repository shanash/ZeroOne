using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Repeat_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 인덱스
	///	</summary>
	public int repeat_reward_id {get; set;}
	///	<summary>
	///	타입
	///	</summary>
	public ITEM_TYPE item_type {get; set;}
	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public int item_index {get; set;}
	///	<summary>
	///	최소 수량
	///	</summary>
	public int min_count {get; set;}
	///	<summary>
	///	최대 수량
	///	</summary>
	public int max_count {get; set;}

	private bool disposed = false;

	public Repeat_Reward_Data()
	{
		repeat_reward_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_index = 0;
		min_count = 0;
		max_count = 0;
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
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_index] = <color=yellow>{0}</color>", item_index).AppendLine();
		sb.AppendFormat("[min_count] = <color=yellow>{0}</color>", min_count).AppendLine();
		sb.AppendFormat("[max_count] = <color=yellow>{0}</color>", max_count).AppendLine();
		return sb.ToString();
	}
}

