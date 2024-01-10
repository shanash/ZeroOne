using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Zone_Data : System.IDisposable
{
	///	<summary>
	///	존 인덱스
	///	</summary>
	public int zone_id {get; set;}
	///	<summary>
	///	존 명칭
	///	</summary>
	public string zone_name {get; set;}
	///	<summary>
	///	속한 월드
	///	</summary>
	public int in_world_id {get; set;}
	///	<summary>
	///	오더링
	///	</summary>
	public int zone_ordering {get; set;}
	///	<summary>
	///	존 난이도
	///	</summary>
	public STAGE_DIFFICULTY_TYPE zone_difficulty {get; set;}
	///	<summary>
	///	존 이미지
	///	</summary>
	public string zone_img_path {get; set;}
	///	<summary>
	///	존 설명
	///	</summary>
	public string zone_tooltip {get; set;}
	///	<summary>
	///	해금 조건
	///	</summary>
	public LIMIT_TYPE limit_type {get; set;}

	private bool disposed = false;

	public Zone_Data()
	{
		zone_id = 0;
		zone_name = string.Empty;
		in_world_id = 0;
		zone_ordering = 0;
		zone_difficulty = STAGE_DIFFICULTY_TYPE.NONE;
		zone_img_path = string.Empty;
		zone_tooltip = string.Empty;
		limit_type = LIMIT_TYPE.NONE;
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
		sb.AppendFormat("[zone_id] = <color=yellow>{0}</color>", zone_id).AppendLine();
		sb.AppendFormat("[zone_name] = <color=yellow>{0}</color>", zone_name).AppendLine();
		sb.AppendFormat("[in_world_id] = <color=yellow>{0}</color>", in_world_id).AppendLine();
		sb.AppendFormat("[zone_ordering] = <color=yellow>{0}</color>", zone_ordering).AppendLine();
		sb.AppendFormat("[zone_difficulty] = <color=yellow>{0}</color>", zone_difficulty).AppendLine();
		sb.AppendFormat("[zone_img_path] = <color=yellow>{0}</color>", zone_img_path).AppendLine();
		sb.AppendFormat("[zone_tooltip] = <color=yellow>{0}</color>", zone_tooltip).AppendLine();
		sb.AppendFormat("[limit_type] = <color=yellow>{0}</color>", limit_type).AppendLine();
		return sb.ToString();
	}
}

