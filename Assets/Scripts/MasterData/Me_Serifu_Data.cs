using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Me_Serifu_Data : System.IDisposable
{
	///	<summary>
	///	대사 인덱스
	///	</summary>
	public int serifu_id {get; set;}
	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	말풍선 텍스트
	///	</summary>
	public string text_kr {get; set;}
	///	<summary>
	///	오디오 클립 키
	///	</summary>
	public string audio_clip_key {get; set;}

	private bool disposed = false;

	public Me_Serifu_Data()
	{
		serifu_id = 0;
		player_character_id = 0;
		text_kr = string.Empty;
		audio_clip_key = string.Empty;
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

