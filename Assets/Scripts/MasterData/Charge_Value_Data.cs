﻿using FluffyDuck.Util;
using System.Linq;

public class Charge_Value_Data : System.IDisposable
{
	///	<summary>
	///	보상 타입
	///	</summary>
	public REWARD_TYPE reward_type => _reward_type;
	REWARD_TYPE _reward_type;

	///	<summary>
	///	충전 타입
	///	</summary>
	public CHARGE_TYPE charge_type => _charge_type;
	CHARGE_TYPE _charge_type;

	///	<summary>
	///	충전 수량
	///	charge_type이
	///	max 값까지 올리는 유형이 아니라, 지정 수량만큼 올리는 타입일 경우에만 사용
	///	ex] 6분에 1만큼(지정 수치) 올린다.
	///	</summary>
	public int charge_count => _charge_count;
	int _charge_count;

	///	<summary>
	///	충전 주기 타입
	///	</summary>
	public REPEAT_TYPE repeat_type => _repeat_type;
	REPEAT_TYPE _repeat_type;

	///	<summary>
	///	충전 주기 Var
	///	repeat_type이 지정된 분 마다 올리는 타입일 경우 사용
	///	ex] 6분마다
	///	</summary>
	public int repeat_time => _repeat_time;
	int _repeat_time;

	///	<summary>
	///	데이터 사용 여부
	///	</summary>
	public bool Use_Charge_Data => _Use_Charge_Data;
	bool _Use_Charge_Data;

	private bool disposed = false;

	public Charge_Value_Data(Raw_Charge_Value_Data raw_data)
	{
		_reward_type = raw_data.reward_type;
		_charge_type = raw_data.charge_type;
		_charge_count = raw_data.charge_count;
		_repeat_type = raw_data.repeat_type;
		_repeat_time = raw_data.repeat_time;
		_Use_Charge_Data = raw_data.Use_Charge_Data;
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
		sb.AppendFormat("[charge_type] = <color=yellow>{0}</color>", charge_type).AppendLine();
		sb.AppendFormat("[charge_count] = <color=yellow>{0}</color>", charge_count).AppendLine();
		sb.AppendFormat("[repeat_type] = <color=yellow>{0}</color>", repeat_type).AppendLine();
		sb.AppendFormat("[repeat_time] = <color=yellow>{0}</color>", repeat_time).AppendLine();
		sb.AppendFormat("[Use_Charge_Data] = <color=yellow>{0}</color>", Use_Charge_Data).AppendLine();
		return sb.ToString();
	}
}
