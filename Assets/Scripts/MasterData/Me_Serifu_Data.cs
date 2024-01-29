#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Me_Serifu_Data : System.IDisposable
{
	///	<summary>
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
	///	말풍선 텍스트
	///	</summary>
	public string text_kr => _text_kr;
	string _text_kr;

	///	<summary>
	///	오디오 클립 키
	///	</summary>
	public string audio_clip_key => _audio_clip_key;
	string _audio_clip_key;

	private bool disposed = false;

	public Me_Serifu_Data(Raw_Me_Serifu_Data raw_data)
	{
		_serifu_id = raw_data.serifu_id;
		_player_character_id = raw_data.player_character_id;
		_text_kr = raw_data.text_kr;
		_audio_clip_key = raw_data.audio_clip_key;
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
		sb.AppendFormat("[text_kr] = <color=yellow>{0}</color>", text_kr).AppendLine();
		sb.AppendFormat("[audio_clip_key] = <color=yellow>{0}</color>", audio_clip_key).AppendLine();
		return sb.ToString();
	}
}

