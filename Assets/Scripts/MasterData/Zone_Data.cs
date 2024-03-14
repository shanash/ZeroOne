#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Zone_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	존 인덱스
	///	</summary>
	public int zone_id => _zone_id;
	int _zone_id;

	///	<summary>
	///	존 이름 ID
	///	</summary>
	public string zone_name_id => _zone_name_id;
	string _zone_name_id;

	///	<summary>
	///	존 명칭(기획)
	///	</summary>
	public string zone_name => _zone_name;
	string _zone_name;

	///	<summary>
	///	존 그룹 ID
	///	</summary>
	public int zone_group_id => _zone_group_id;
	int _zone_group_id;

	///	<summary>
	///	존 구분 ID
	///	</summary>
	public int zone_code_id => _zone_code_id;
	int _zone_code_id;

	///	<summary>
	///	스테이지 그룹 ID
	///	</summary>
	public int stage_group_id => _stage_group_id;
	int _stage_group_id;

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
	///	존 툴 팁 아이디
	///	</summary>
	public string zone_tooltip_id => _zone_tooltip_id;
	string _zone_tooltip_id;

	///	<summary>
	///	존 설명(기획)
	///	</summary>
	public string zone_tooltip => _zone_tooltip;
	string _zone_tooltip;

	///	<summary>
	///	오픈 던전 완료 ID
	///	</summary>
	public int open_stage_id => _open_stage_id;
	int _open_stage_id;

	///	<summary>
	///	강제 락 설정
	///	</summary>
	public bool lock_setting => _lock_setting;
	bool _lock_setting;

	private bool disposed = false;

	public Zone_Data(Raw_Zone_Data raw_data)
	{
		_zone_id = raw_data.zone_id;
		_zone_name_id = raw_data.zone_name_id;
		_zone_name = raw_data.zone_name;
		_zone_group_id = raw_data.zone_group_id;
		_zone_code_id = raw_data.zone_code_id;
		_stage_group_id = raw_data.stage_group_id;
		_zone_ordering = raw_data.zone_ordering;
		_zone_difficulty = raw_data.zone_difficulty;
		_zone_img_path = raw_data.zone_img_path;
		_zone_tooltip_id = raw_data.zone_tooltip_id;
		_zone_tooltip = raw_data.zone_tooltip;
		_open_stage_id = raw_data.open_stage_id;
		_lock_setting = raw_data.lock_setting;
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
		sb.AppendFormat("[zone_name_id] = <color=yellow>{0}</color>", zone_name_id).AppendLine();
		sb.AppendFormat("[zone_name] = <color=yellow>{0}</color>", zone_name).AppendLine();
		sb.AppendFormat("[zone_group_id] = <color=yellow>{0}</color>", zone_group_id).AppendLine();
		sb.AppendFormat("[zone_code_id] = <color=yellow>{0}</color>", zone_code_id).AppendLine();
		sb.AppendFormat("[stage_group_id] = <color=yellow>{0}</color>", stage_group_id).AppendLine();
		sb.AppendFormat("[zone_ordering] = <color=yellow>{0}</color>", zone_ordering).AppendLine();
		sb.AppendFormat("[zone_difficulty] = <color=yellow>{0}</color>", zone_difficulty).AppendLine();
		sb.AppendFormat("[zone_img_path] = <color=yellow>{0}</color>", zone_img_path).AppendLine();
		sb.AppendFormat("[zone_tooltip_id] = <color=yellow>{0}</color>", zone_tooltip_id).AppendLine();
		sb.AppendFormat("[zone_tooltip] = <color=yellow>{0}</color>", zone_tooltip).AppendLine();
		sb.AppendFormat("[open_stage_id] = <color=yellow>{0}</color>", open_stage_id).AppendLine();
		sb.AppendFormat("[lock_setting] = <color=yellow>{0}</color>", lock_setting).AppendLine();
		return sb.ToString();
	}
}

