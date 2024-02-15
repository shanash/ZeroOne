#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Wave_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	웨이브 ID
	///	</summary>
	public int wave_id => _wave_id;
	int _wave_id;

	///	<summary>
	///	웨이브 그룹ID
	///	</summary>
	public int wave_group_id => _wave_group_id;
	int _wave_group_id;

	///	<summary>
	///	웨이브 순서
	///	</summary>
	public int wave_sequence => _wave_sequence;
	int _wave_sequence;

	///	<summary>
	///	웨이브에 출현하는
	///	적 최대 마리 수
	///	</summary>
	public int enemy_appearance_count => _enemy_appearance_count;
	int _enemy_appearance_count;

	///	<summary>
	///	출현 적 정보
	///	</summary>
	public int[] enemy_appearance_info => _enemy_appearance_info;
	int[] _enemy_appearance_info;

	///	<summary>
	///	NPC 레벨 정보
	///	</summary>
	public int[] npc_levels => _npc_levels;
	int[] _npc_levels;

	///	<summary>
	///	NPC 스탯 증가 정보
	///	</summary>
	public int[] npc_stat_ids => _npc_stat_ids;
	int[] _npc_stat_ids;

	///	<summary>
	///	웨이브 제한 시간
	///	</summary>
	public int wave_time => _wave_time;
	int _wave_time;

	private bool disposed = false;

	public Wave_Data(Raw_Wave_Data raw_data)
	{
		_wave_id = raw_data.wave_id;
		_wave_group_id = raw_data.wave_group_id;
		_wave_sequence = raw_data.wave_sequence;
		_enemy_appearance_count = raw_data.enemy_appearance_count;
		if(raw_data.enemy_appearance_info != null)
			_enemy_appearance_info = raw_data.enemy_appearance_info.ToArray();
		if(raw_data.npc_levels != null)
			_npc_levels = raw_data.npc_levels.ToArray();
		if(raw_data.npc_stat_ids != null)
			_npc_stat_ids = raw_data.npc_stat_ids.ToArray();
		_wave_time = raw_data.wave_time;
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
		sb.AppendFormat("[wave_id] = <color=yellow>{0}</color>", wave_id).AppendLine();
		sb.AppendFormat("[wave_group_id] = <color=yellow>{0}</color>", wave_group_id).AppendLine();
		sb.AppendFormat("[wave_sequence] = <color=yellow>{0}</color>", wave_sequence).AppendLine();
		sb.AppendFormat("[enemy_appearance_count] = <color=yellow>{0}</color>", enemy_appearance_count).AppendLine();
		sb.AppendLine("[enemy_appearance_info]");
		if(enemy_appearance_info != null)
		{
			cnt = enemy_appearance_info.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", enemy_appearance_info[i]).AppendLine();
			}
		}

		sb.AppendLine("[npc_levels]");
		if(npc_levels != null)
		{
			cnt = npc_levels.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", npc_levels[i]).AppendLine();
			}
		}

		sb.AppendLine("[npc_stat_ids]");
		if(npc_stat_ids != null)
		{
			cnt = npc_stat_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", npc_stat_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[wave_time] = <color=yellow>{0}</color>", wave_time).AppendLine();
		return sb.ToString();
	}
}

