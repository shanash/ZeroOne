using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Editor_Stage_Data : System.IDisposable
{
	///	<summary>
	///	스테이지 인덱스
	///	</summary>
	public int stage_id {get; set;}
	///	<summary>
	///	스테이지 별 개수
	///	</summary>
	public int stage_star_count {get; set;}
	///	<summary>
	///	스테이지 웨이브 개수
	///	</summary>
	public int stage_wave_count {get; set;}
	///	<summary>
	///	웨이브 연결 ID
	///	</summary>
	public int wave_group_id {get; set;}

	private bool disposed = false;

	public Editor_Stage_Data()
	{
		stage_id = 0;
		stage_star_count = 0;
		stage_wave_count = 0;
		wave_group_id = 0;
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

