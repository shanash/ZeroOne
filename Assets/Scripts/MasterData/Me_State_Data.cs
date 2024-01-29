#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Me_State_Data : System.IDisposable
{
	///	<summary>
	///	상태 고유 아이디
	///	</summary>
	public int state_id => _state_id;
	int _state_id;

	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	아이들 애니메이션 이름
	///	</summary>
	public string idle_animation_name => _idle_animation_name;
	string _idle_animation_name;

	///	<summary>
	///	대기시 자동재생될 반응
	///	</summary>
	public int[] bored_chatmotion_ids => _bored_chatmotion_ids;
	int[] _bored_chatmotion_ids;

	///	<summary>
	///	자동재생될 아이들 애니메이션 출력횟수
	///	</summary>
	public int bored_condition_count => _bored_condition_count;
	int _bored_condition_count;

	private bool disposed = false;

	public Me_State_Data(Raw_Me_State_Data raw_data)
	{
		_state_id = raw_data.state_id;
		_player_character_id = raw_data.player_character_id;
		_idle_animation_name = raw_data.idle_animation_name;
		if(raw_data.bored_chatmotion_ids != null)
			_bored_chatmotion_ids = raw_data.bored_chatmotion_ids.ToArray();
		_bored_condition_count = raw_data.bored_condition_count;
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

