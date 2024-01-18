public class Me_State_Data : System.IDisposable
{
	///	<summary>
	///	상태 고유 아이디
	///	</summary>
	public readonly int state_id;
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public readonly int player_character_id;
	///	<summary>
	///	아이들 애니메이션 이름
	///	</summary>
	public readonly string idle_animation_name;
	///	<summary>
	///	대기시 자동재생될 반응
	///	</summary>
	public readonly int[] bored_chatmotion_ids;
	///	<summary>
	///	자동재생될 아이들 애니메이션 출력횟수
	///	</summary>
	public readonly int bored_condition_count;

	private bool disposed = false;

	public Me_State_Data(Raw_Me_State_Data raw_data)
	{
		state_id = raw_data.state_id;
		player_character_id = raw_data.player_character_id;
		idle_animation_name = raw_data.idle_animation_name;
		bored_chatmotion_ids = raw_data.bored_chatmotion_ids != null ? (int[])raw_data.bored_chatmotion_ids.Clone() : new int[0];
		bored_condition_count = raw_data.bored_condition_count;
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
		sb.AppendFormat("[state_id] = <color=yellow>{0}</color>", state_id).AppendLine();
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[idle_animation_name] = <color=yellow>{0}</color>", idle_animation_name).AppendLine();
		sb.AppendLine("[bored_chatmotion_ids]");
		if(bored_chatmotion_ids != null)
		{
			cnt = bored_chatmotion_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", bored_chatmotion_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[bored_condition_count] = <color=yellow>{0}</color>", bored_condition_count).AppendLine();
		return sb.ToString();
	}
}

