[System.Serializable]
public class Me_State_Data : System.IDisposable
{
	///	<summary>
	///	상태 고유 아이디
	///	</summary>
	public int state_id {get; set;}
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	아이들 애니메이션 이름
	///	</summary>
	public string idle_animation_name {get; set;}
	///	<summary>
	///	대기시 자동재생될 반응
	///	</summary>
	public int[] bored_chatmotion_ids {get; set;}
	///	<summary>
	///	자동재생될 아이들 애니메이션 출력횟수
	///	</summary>
	public int bored_condition_count {get; set;}

	private bool disposed = false;

	public Me_State_Data()
	{
		state_id = 0;
		player_character_id = 0;
		idle_animation_name = string.Empty;
		bored_condition_count = 0;
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

