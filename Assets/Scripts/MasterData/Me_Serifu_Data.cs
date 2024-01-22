public class Me_Serifu_Data : System.IDisposable
{
	///	<summary>
	///	대사 인덱스
	///	</summary>
	public readonly int serifu_id;
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public readonly int player_character_id;
	///	<summary>
	///	말풍선 텍스트
	///	</summary>
	public readonly string text_kr;
	///	<summary>
	///	오디오 클립 키
	///	</summary>
	public readonly string audio_clip_key;

	private bool disposed = false;

	public Me_Serifu_Data(Raw_Me_Serifu_Data raw_data)
	{
		serifu_id = raw_data.serifu_id;
		player_character_id = raw_data.player_character_id;
		text_kr = raw_data.text_kr;
		audio_clip_key = raw_data.audio_clip_key;
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

