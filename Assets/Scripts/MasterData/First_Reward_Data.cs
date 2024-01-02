using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class First_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 그룹 인덱스
	///	</summary>
	public int frist_reward_id {get; set;}
	///	<summary>
	///	타입
	///	</summary>
	public ITEM_TYPE item_type {get; set;}
	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public string item_index {get; set;}
	///	<summary>
	///	지급 수량
	///	</summary>
	public string count {get; set;}

	private bool disposed = false;

	public First_Reward_Data()
	{
		frist_reward_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_index = string.Empty;
		count = string.Empty;
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
		sb.AppendFormat("[frist_reward_id] = <color=yellow>{0}</color>", frist_reward_id).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_index] = <color=yellow>{0}</color>", item_index).AppendLine();
		sb.AppendFormat("[count] = <color=yellow>{0}</color>", count).AppendLine();
		return sb.ToString();
	}
}

