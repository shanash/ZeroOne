using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Stage_Reward_Data : System.IDisposable
{
	///	<summary>
	///	stage reward id
	///	</summary>
	public int stage_reward_id {get; set;}
	///	<summary>
	///	reward group id
	///	</summary>
	public int stage_reward_group_id {get; set;}
	///	<summary>
	///	아이템 타입
	///	</summary>
	public ITEM_TYPE item_type {get; set;}
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	아이템 개수 최소
	///	</summary>
	public int min_count {get; set;}
	///	<summary>
	///	아이템 개수 최최대
	///	</summary>
	public int max_count {get; set;}
	///	<summary>
	///	확률
	///	확률은 개별 확률로 정한다.(각 아이템별 확률)
	///	백만분율 사용
	///	(필요시 만분율로 변경 가능)
	///	</summary>
	public int rate {get; set;}
	///	<summary>
	///	순서
	///	</summary>
	public int order {get; set;}

	private bool disposed = false;

	public Stage_Reward_Data()
	{
		stage_reward_id = 0;
		stage_reward_group_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		min_count = 0;
		max_count = 0;
		rate = 0;
		order = 0;
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
		sb.AppendFormat("[stage_reward_id] = <color=yellow>{0}</color>", stage_reward_id).AppendLine();
		sb.AppendFormat("[stage_reward_group_id] = <color=yellow>{0}</color>", stage_reward_group_id).AppendLine();
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[min_count] = <color=yellow>{0}</color>", min_count).AppendLine();
		sb.AppendFormat("[max_count] = <color=yellow>{0}</color>", max_count).AppendLine();
		sb.AppendFormat("[rate] = <color=yellow>{0}</color>", rate).AppendLine();
		sb.AppendFormat("[order] = <color=yellow>{0}</color>", order).AppendLine();
		return sb.ToString();
	}
}

