using FluffyDuck.Util;
using System.Linq;

public class Zone_Data : System.IDisposable
{
	///	<summary>
	///	존 인덱스
	///	</summary>
	public int zone_id => _zone_id;
	int _zone_id;

	///	<summary>
	///	존 명칭
	///	</summary>
	public string zone_name => _zone_name;
	string _zone_name;

	///	<summary>
	///	속한 월드
	///	</summary>
	public int in_world_id => _in_world_id;
	int _in_world_id;

	///	<summary>
	///	오더링
	///	</summary>
	public int zone_ordering => _zone_ordering;
	int _zone_ordering;

	///	<summary>
	///	존 난이도
	///	</summary>
	public STAGE_DIFFICULTY_TYPE zone_difficulty => _zone_difficulty;
	STAGE_DIFFICULTY_TYPE _zone_difficulty;

	///	<summary>
	///	존 이미지
	///	</summary>
	public string zone_img_path => _zone_img_path;
	string _zone_img_path;

	///	<summary>
	///	존 설명
	///	</summary>
	public string zone_tooltip => _zone_tooltip;
	string _zone_tooltip;

	///	<summary>
	///	해금 조건
	///	</summary>
	public LIMIT_TYPE limit_type => _limit_type;
	LIMIT_TYPE _limit_type;

	private bool disposed = false;

	public Zone_Data(Raw_Zone_Data raw_data)
	{
		_zone_id = raw_data.zone_id;
		_zone_name = raw_data.zone_name;
		_in_world_id = raw_data.in_world_id;
		_zone_ordering = raw_data.zone_ordering;
		_zone_difficulty = raw_data.zone_difficulty;
		_zone_img_path = raw_data.zone_img_path;
		_zone_tooltip = raw_data.zone_tooltip;
		_limit_type = raw_data.limit_type;
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

