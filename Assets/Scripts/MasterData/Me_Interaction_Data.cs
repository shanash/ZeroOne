public class Me_Interaction_Data : System.IDisposable
{
	///	<summary>
	///	인터렉션 아이디
	///	</summary>
	public readonly int interaction_id;
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public readonly int player_character_id;
	///	<summary>
	///	터치가능한 신체부위
	///	</summary>
	public readonly TOUCH_BODY_TYPE touch_body_type;
	///	<summary>
	///	좌우(L or R)
	///	</summary>
	public readonly string touch_body_direction;
	///	<summary>
	///	제스쳐 종류
	///	</summary>
	public readonly TOUCH_GESTURE_TYPE touch_gesture_type;
	///	<summary>
	///	이상
	///	</summary>
	public readonly int condition_min_gesture_count;
	///	<summary>
	///	이하
	///	</summary>
	public readonly int condition_max_gesture_count;
	///	<summary>
	///	드래그 애니메이션 이름
	///	</summary>
	public readonly string drag_animation_name;
	///	<summary>
	///	챗모션 인덱스
	///	</summary>
	public readonly int[] chat_motion_ids;
	///	<summary>
	///	특정 상태 조건 고유 인덱스
	///	</summary>
	public readonly int[] condition_state_ids;
	///	<summary>
	///	특정 상태 변환
	///	</summary>
	public readonly int change_state_id;
	///	<summary>
	///	기타 파라메터
	///	</summary>
	public readonly string[] parameters;

	private bool disposed = false;

	public Me_Interaction_Data(Raw_Me_Interaction_Data raw_data)
	{
		interaction_id = raw_data.interaction_id;
		player_character_id = raw_data.player_character_id;
		touch_body_type = raw_data.touch_body_type;
		touch_body_direction = raw_data.touch_body_direction;
		touch_gesture_type = raw_data.touch_gesture_type;
		condition_min_gesture_count = raw_data.condition_min_gesture_count;
		condition_max_gesture_count = raw_data.condition_max_gesture_count;
		drag_animation_name = raw_data.drag_animation_name;
		chat_motion_ids = raw_data.chat_motion_ids != null ? (int[])raw_data.chat_motion_ids.Clone() : new int[0];
		condition_state_ids = raw_data.condition_state_ids != null ? (int[])raw_data.condition_state_ids.Clone() : new int[0];
		change_state_id = raw_data.change_state_id;
		parameters = raw_data.parameters != null ? (string[])raw_data.parameters.Clone() : new string[0];
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
		sb.AppendFormat("[condition_min_gesture_count] = <color=yellow>{0}</color>", condition_min_gesture_count).AppendLine();
		sb.AppendFormat("[condition_max_gesture_count] = <color=yellow>{0}</color>", condition_max_gesture_count).AppendLine();
		sb.AppendFormat("[drag_animation_name] = <color=yellow>{0}</color>", drag_animation_name).AppendLine();
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

