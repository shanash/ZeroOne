using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Item_Piece_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int itempiece_id {get; set;}
	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id {get; set;}
	///	<summary>
	///	조각 타입
	///	1:아이템
	///	2:캐릭터
	///	3:장비
	///	</summary>
	public PIECE_TYPE piece_type {get; set;}
	///	<summary>
	///	조각낼 대상 아이디
	///	</summary>
	public int target_id {get; set;}
	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num {get; set;}
	///	<summary>
	///	1개 제작을 하기 위한 수량
	///	</summary>
	public int make_count {get; set;}
	///	<summary>
	///	소비 시간(분)
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time {get; set;}
	///	<summary>
	///	소비기한
	///	> scheduleTable과 연결
	///	> 값이 0 이면, 소비 기한 없음
	///	</summary>
	public int expire_schedule_id {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Item_Piece_Data()
	{
		itempiece_id = 0;
		name_id = string.Empty;
		piece_type = PIECE_TYPE.NONE;
		target_id = 0;
		max_num = 0;
		make_count = 0;
		expire_time = 0;
		expire_schedule_id = 0;
		icon_path = string.Empty;
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
		sb.AppendFormat("[itempiece_id] = <color=yellow>{0}</color>", itempiece_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[piece_type] = <color=yellow>{0}</color>", piece_type).AppendLine();
		sb.AppendFormat("[target_id] = <color=yellow>{0}</color>", target_id).AppendLine();
		sb.AppendFormat("[max_num] = <color=yellow>{0}</color>", max_num).AppendLine();
		sb.AppendFormat("[make_count] = <color=yellow>{0}</color>", make_count).AppendLine();
		sb.AppendFormat("[expire_time] = <color=yellow>{0}</color>", expire_time).AppendLine();
		sb.AppendFormat("[expire_schedule_id] = <color=yellow>{0}</color>", expire_schedule_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

