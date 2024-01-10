using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Me_Chat_Motion_Data : System.IDisposable
{
	///	<summary>
	///	챗모션 id
	///	</summary>
	public int chat_motion_id {get; set;}
	///	<summary>
	///	캐릭터 고유 아이디
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	애니메이션 이름
	///	</summary>
	public string[] animation_name {get; set;}
	///	<summary>
	///	대사 id
	///	</summary>
	public int[] serifu_ids {get; set;}

	private bool disposed = false;

	public Me_Chat_Motion_Data()
	{
		chat_motion_id = 0;
		player_character_id = 0;
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
		sb.AppendLine("[animation_name]");
		if(animation_name != null)
		{
			cnt = animation_name.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", animation_name[i]).AppendLine();
			}
		}

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

