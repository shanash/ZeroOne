public class Zone_Data : System.IDisposable
{
	///	<summary>
	///	존 인덱스
	///	</summary>
	public readonly int zone_id;
	///	<summary>
	///	존 명칭
	///	</summary>
	public readonly string zone_name;
	///	<summary>
	///	속한 월드
	///	</summary>
	public readonly int in_world_id;
	///	<summary>
	///	오더링
	///	</summary>
	public readonly int zone_ordering;
	///	<summary>
	///	존 난이도
	///	</summary>
	public readonly STAGE_DIFFICULTY_TYPE zone_difficulty;
	///	<summary>
	///	존 이미지
	///	</summary>
	public readonly string zone_img_path;
	///	<summary>
	///	존 설명
	///	</summary>
	public readonly string zone_tooltip;
	///	<summary>
	///	해금 조건
	///	</summary>
	public readonly LIMIT_TYPE limit_type;

	private bool disposed = false;

	public Zone_Data(Raw_Zone_Data raw_data)
	{
		zone_id = raw_data.zone_id;
		zone_name = raw_data.zone_name;
		in_world_id = raw_data.in_world_id;
		zone_ordering = raw_data.zone_ordering;
		zone_difficulty = raw_data.zone_difficulty;
		zone_img_path = raw_data.zone_img_path;
		zone_tooltip = raw_data.zone_tooltip;
		limit_type = raw_data.limit_type;
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

