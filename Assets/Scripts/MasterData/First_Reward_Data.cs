using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class First_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 고유 인덱스
	///	</summary>
	public int first_reward_id {get; set;}
	///	<summary>
	///	보상 그룹 아이디
	///	</summary>
	public int first_reward_group_id {get; set;}
	///	<summary>
	///	타입
	///	</summary>
	public ITEM_TYPE item_type {get; set;}
	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	지급 수량
	///	</summary>
	public string item_count {get; set;}

	private bool disposed = false;

	public First_Reward_Data()
	{
		first_reward_id = 0;
		first_reward_group_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		item_count = string.Empty;
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

