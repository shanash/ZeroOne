using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Me_Interaction_Data : System.IDisposable
{
	///	<summary>
	///	인터렉션 아이디
	///	</summary>
	public int interaction_id {get; set;}
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	터치가능한 신체부위
	///	</summary>
	public TOUCH_BODY_TYPE touch_body_type {get; set;}
	///	<summary>
	///	좌우(L or R)
	///	</summary>
	public string touch_body_direction {get; set;}
	///	<summary>
	///	제스쳐 종류
	///	</summary>
	public TOUCH_GESTURE_TYPE touch_gesture_type {get; set;}
	///	<summary>
	///	부등호
	///	</summary>
	public INEQUALITY_TYPE touch_condition_inequality {get; set;}
	///	<summary>
	///	제스쳐 횟수
	///	</summary>
	public int touch_condition_count {get; set;}
	///	<summary>
	///	챗모션 인덱스
	///	</summary>
	public int[] chat_motion_ids {get; set;}
	///	<summary>
	///	특정 상태 조건 고유 인덱스
	///	</summary>
	public int[] condition_state_ids {get; set;}
	///	<summary>
	///	특정 상태 변환
	///	</summary>
	public int change_state_id {get; set;}
	///	<summary>
	///	기타 파라메터
	///	</summary>
	public string[] parameters {get; set;}

	private bool disposed = false;

	public Me_Interaction_Data()
	{
		interaction_id = 0;
		player_character_id = 0;
		touch_body_type = TOUCH_BODY_TYPE.NONE;
		touch_body_direction = string.Empty;
		touch_gesture_type = TOUCH_GESTURE_TYPE.NONE;
		touch_condition_inequality = INEQUALITY_TYPE.NONE;
		touch_condition_count = 0;
		change_state_id = 0;
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
		int cnt = 0;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendFormat("[interaction_id] = <color=yellow>{0}</color>", interaction_id).AppendLine();
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[touch_body_type] = <color=yellow>{0}</color>", touch_body_type).AppendLine();
		sb.AppendFormat("[touch_body_direction] = <color=yellow>{0}</color>", touch_body_direction).AppendLine();
		sb.AppendFormat("[touch_gesture_type] = <color=yellow>{0}</color>", touch_gesture_type).AppendLine();
		sb.AppendFormat("[touch_condition_inequality] = <color=yellow>{0}</color>", touch_condition_inequality).AppendLine();
		sb.AppendFormat("[touch_condition_count] = <color=yellow>{0}</color>", touch_condition_count).AppendLine();
		sb.AppendLine("[chat_motion_ids]");
		if(chat_motion_ids != null)
		{
			cnt = chat_motion_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", chat_motion_ids[i]).AppendLine();
			}
		}

		sb.AppendLine("[condition_state_ids]");
		if(condition_state_ids != null)
		{
			cnt = condition_state_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", condition_state_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[change_state_id] = <color=yellow>{0}</color>", change_state_id).AppendLine();
		sb.AppendLine("[parameters]");
		if(parameters != null)
		{
			cnt = parameters.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", parameters[i]).AppendLine();
			}
		}

		return sb.ToString();
	}
}

