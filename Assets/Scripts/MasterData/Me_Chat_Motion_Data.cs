#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Me_Chat_Motion_Data : System.IDisposable
{
	///	<summary>
	///	챗모션 id
	///	</summary>
	public int chat_motion_id => _chat_motion_id;
	int _chat_motion_id;

	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	애니메이션 이름
	///	</summary>
	public string animation_name => _animation_name;
	string _animation_name;

	///	<summary>
	///	대사 id
	///	</summary>
	public int[] serifu_ids => _serifu_ids;
	int[] _serifu_ids;

	private bool disposed = false;

	public Me_Chat_Motion_Data(Raw_Me_Chat_Motion_Data raw_data)
	{
		_chat_motion_id = raw_data.chat_motion_id;
		_player_character_id = raw_data.player_character_id;
		_animation_name = raw_data.animation_name;
		if(raw_data.serifu_ids != null)
			_serifu_ids = raw_data.serifu_ids.ToArray();
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
		sb.AppendFormat("[chat_motion_id] = <color=yellow>{0}</color>", chat_motion_id).AppendLine();
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[animation_name] = <color=yellow>{0}</color>", animation_name).AppendLine();
		sb.AppendLine("[serifu_ids]");
		if(serifu_ids != null)
		{
			cnt = serifu_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", serifu_ids[i]).AppendLine();
			}
		}

		return sb.ToString();
	}
}

