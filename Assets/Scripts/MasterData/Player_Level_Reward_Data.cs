#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Player_Level_Reward_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	레벨
	///	</summary>
	public int player_level => _player_level;
	int _player_level;

	///	<summary>
	///	스태미나 증가량
	///	</summary>
	public int increase_stamina => _increase_stamina;
	int _increase_stamina;

	private bool disposed = false;

	public Player_Level_Reward_Data(Raw_Player_Level_Reward_Data raw_data)
	{
		_player_level = raw_data.player_level;
		_increase_stamina = raw_data.increase_stamina;
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
		sb.AppendFormat("[player_level] = <color=yellow>{0}</color>", player_level).AppendLine();
		sb.AppendFormat("[increase_stamina] = <color=yellow>{0}</color>", increase_stamina).AppendLine();
		return sb.ToString();
	}
}

