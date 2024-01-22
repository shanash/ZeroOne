public class Editor_Wave_Data : System.IDisposable
{
	///	<summary>
	///	웨이브 그룹 ID
	///	</summary>
	public readonly int wave_group_id;
	///	<summary>
	///	웨이브 순서
	///	</summary>
	public readonly int wave_sequence;
	///	<summary>
	///	웨이브에 출현하는
	///	적 최대 마리 수
	///	</summary>
	public readonly int enemy_appearance_count;
	///	<summary>
	///	출현 적 정보
	///	</summary>
	public readonly int[] enemy_appearance_info;
	///	<summary>
	///	웨이브 제한 시간
	///	</summary>
	public readonly int wave_time;

	private bool disposed = false;

	public Editor_Wave_Data(Raw_Editor_Wave_Data raw_data)
	{
		wave_group_id = raw_data.wave_group_id;
		wave_sequence = raw_data.wave_sequence;
		enemy_appearance_count = raw_data.enemy_appearance_count;
		enemy_appearance_info = raw_data.enemy_appearance_info != null ? (int[])raw_data.enemy_appearance_info.Clone() : new int[0];
		wave_time = raw_data.wave_time;
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

		sb.AppendFormat("[wave_time] = <color=yellow>{0}</color>", wave_time).AppendLine();
		return sb.ToString();
	}
}

