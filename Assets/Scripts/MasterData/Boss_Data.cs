#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Boss_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	보스 인덱스
	///	</summary>
	public int boss_id => _boss_id;
	int _boss_id;

	///	<summary>
	///	보스 그룹 ID
	///	</summary>
	public int boss_group_id => _boss_group_id;
	int _boss_group_id;

	///	<summary>
	///	보스 명
	///	</summary>
	public string boss_name => _boss_name;
	string _boss_name;

	///	<summary>
	///	보스 스테이지 그룹
	///	</summary>
	public int boss_stage_group_id => _boss_stage_group_id;
	int _boss_stage_group_id;

	///	<summary>
	///	보스 설정 설명
	///	</summary>
	public string boss_story_info => _boss_story_info;
	string _boss_story_info;

	///	<summary>
	///	보스 스킬 설명
	///	</summary>
	public string boss_skill_info => _boss_skill_info;
	string _boss_skill_info;

	///	<summary>
	///	프리팹
	///	</summary>
	public string prefab_path => _prefab_path;
	string _prefab_path;

	///	<summary>
	///	던전 오픈 게임 타입
	///	</summary>
	public GAME_TYPE open_game_type => _open_game_type;
	GAME_TYPE _open_game_type;

	///	<summary>
	///	오픈 조건 던전 ID
	///	</summary>
	public int open_dungeon_id => _open_dungeon_id;
	int _open_dungeon_id;

	private bool disposed = false;

	public Boss_Data(Raw_Boss_Data raw_data)
	{
		_boss_id = raw_data.boss_id;
		_boss_group_id = raw_data.boss_group_id;
		_boss_name = raw_data.boss_name;
		_boss_stage_group_id = raw_data.boss_stage_group_id;
		_boss_story_info = raw_data.boss_story_info;
		_boss_skill_info = raw_data.boss_skill_info;
		_prefab_path = raw_data.prefab_path;
		_open_game_type = raw_data.open_game_type;
		_open_dungeon_id = raw_data.open_dungeon_id;
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
		sb.AppendFormat("[boss_id] = <color=yellow>{0}</color>", boss_id).AppendLine();
		sb.AppendFormat("[boss_group_id] = <color=yellow>{0}</color>", boss_group_id).AppendLine();
		sb.AppendFormat("[boss_name] = <color=yellow>{0}</color>", boss_name).AppendLine();
		sb.AppendFormat("[boss_stage_group_id] = <color=yellow>{0}</color>", boss_stage_group_id).AppendLine();
		sb.AppendFormat("[boss_story_info] = <color=yellow>{0}</color>", boss_story_info).AppendLine();
		sb.AppendFormat("[boss_skill_info] = <color=yellow>{0}</color>", boss_skill_info).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[open_game_type] = <color=yellow>{0}</color>", open_game_type).AppendLine();
		sb.AppendFormat("[open_dungeon_id] = <color=yellow>{0}</color>", open_dungeon_id).AppendLine();
		return sb.ToString();
	}
}

