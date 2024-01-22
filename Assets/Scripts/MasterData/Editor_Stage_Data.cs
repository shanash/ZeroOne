using FluffyDuck.Util;
using System.Linq;

public class Editor_Stage_Data : System.IDisposable
{
	///	<summary>
	///	스테이지 인덱스
	///	</summary>
	public int stage_id => _stage_id;
	int _stage_id;

	///	<summary>
	///	스테이지 별 개수
	///	</summary>
	public int stage_star_count => _stage_star_count;
	int _stage_star_count;

	///	<summary>
	///	스테이지 웨이브 개수
	///	</summary>
	public int stage_wave_count => _stage_wave_count;
	int _stage_wave_count;

	///	<summary>
	///	웨이브 연결 ID
	///	</summary>
	public int wave_group_id => _wave_group_id;
	int _wave_group_id;

	private bool disposed = false;

	public Editor_Stage_Data(Raw_Editor_Stage_Data raw_data)
	{
		_stage_id = raw_data.stage_id;
		_stage_star_count = raw_data.stage_star_count;
		_stage_wave_count = raw_data.stage_wave_count;
		_wave_group_id = raw_data.wave_group_id;
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
		sb.AppendFormat("[stage_id] = <color=yellow>{0}</color>", stage_id).AppendLine();
		sb.AppendFormat("[stage_star_count] = <color=yellow>{0}</color>", stage_star_count).AppendLine();
		sb.AppendFormat("[stage_wave_count] = <color=yellow>{0}</color>", stage_wave_count).AppendLine();
		sb.AppendFormat("[wave_group_id] = <color=yellow>{0}</color>", wave_group_id).AppendLine();
		return sb.ToString();
	}
}

