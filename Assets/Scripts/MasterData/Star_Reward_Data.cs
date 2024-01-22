﻿using FluffyDuck.Util;
using System.Linq;

public class Star_Reward_Data : System.IDisposable
{
	///	<summary>
	///	보상 고유 인덱스
	///	</summary>
	public int star_reward_id => _star_reward_id;
	int _star_reward_id;

	///	<summary>
	///	보상 그룹 아이디
	///	</summary>
	public int star_reward_group_id => _star_reward_group_id;
	int _star_reward_group_id;

	///	<summary>
	///	별 포인트
	///	</summary>
	public int star_point => _star_point;
	int _star_point;

	///	<summary>
	///	타입
	///	</summary>
	public ITEM_TYPE item_type => _item_type;
	ITEM_TYPE _item_type;

	///	<summary>
	///	아이템 인덱스
	///	</summary>
	public int item_id => _item_id;
	int _item_id;

	///	<summary>
	///	지급 수량
	///	</summary>
	public int item_count => _item_count;
	int _item_count;

	private bool disposed = false;

	public Star_Reward_Data(Raw_Star_Reward_Data raw_data)
	{
		_star_reward_id = raw_data.star_reward_id;
		_star_reward_group_id = raw_data.star_reward_group_id;
		_star_point = raw_data.star_point;
		_item_type = raw_data.item_type;
		_item_id = raw_data.item_id;
		_item_count = raw_data.item_count;
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

