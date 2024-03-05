#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Me_Serifu_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	대사 인덱스
	///	</summary>
	public int serifu_id => _serifu_id;
	int _serifu_id;

	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	캐릭터 대사
	///	</summary>
	public string dialogue_text_id => _dialogue_text_id;
	string _dialogue_text_id;

	///	<summary>
	///	보이스 파일 아이디_스트링 테이블 참조
	///	</summary>
	public string audio_clip_id => _audio_clip_id;
	string _audio_clip_id;

	private bool disposed = false;

	public Me_Serifu_Data(Raw_Me_Serifu_Data raw_data)
	{
		_serifu_id = raw_data.serifu_id;
		_player_character_id = raw_data.player_character_id;
		_dialogue_text_id = raw_data.dialogue_text_id;
		_audio_clip_id = raw_data.audio_clip_id;
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
		sb.AppendFormat("[serifu_id] = <color=yellow>{0}</color>", serifu_id).AppendLine();
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[dialogue_text_id] = <color=yellow>{0}</color>", dialogue_text_id).AppendLine();
		sb.AppendFormat("[audio_clip_id] = <color=yellow>{0}</color>", audio_clip_id).AppendLine();
		return sb.ToString();
	}
}

