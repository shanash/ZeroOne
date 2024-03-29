#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Boss_Stage_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	스테이지 인덱스
	///	</summary>
	public int boss_stage_id => _boss_stage_id;
	int _boss_stage_id;

	///	<summary>
	///	스테이지 그룹 ID
	///	</summary>
	public int boss_stage_group_id => _boss_stage_group_id;
	int _boss_stage_group_id;

	///	<summary>
	///	웨이브 그룹 ID
	///	</summary>
	public int wave_group_id => _wave_group_id;
	int _wave_group_id;

	///	<summary>
	///	스테이지 넘버링
	///	</summary>
	public int stage_ordering => _stage_ordering;
	int _stage_ordering;

	///	<summary>
	///	스테이지 명칭
	///	</summary>
	public string stage_name => _stage_name;
	string _stage_name;

	///	<summary>
	///	클리어 시 캐릭터 경험치
	///	</summary>
	public int character_exp => _character_exp;
	int _character_exp;

	///	<summary>
	///	호감도 경험치
	///	</summary>
	public int destiny_exp => _destiny_exp;
	int _destiny_exp;

	///	<summary>
	///	지급 골드
	///	</summary>
	public int gold => _gold;
	int _gold;

	///	<summary>
	///	통상 보상
	///	</summary>
	public int repeat_reward_group_id => _repeat_reward_group_id;
	int _repeat_reward_group_id;

	///	<summary>
	///	초회 보상
	///	</summary>
	public int first_reward_group_id => _first_reward_group_id;
	int _first_reward_group_id;

	///	<summary>
	///	권장 레벨
	///	</summary>
	public int recomment_level => _recomment_level;
	int _recomment_level;

	///	<summary>
	///	스테이지 BGM
	///	</summary>
	public string bgm_path => _bgm_path;
	string _bgm_path;

	private bool disposed = false;

	public Boss_Stage_Data(Raw_Boss_Stage_Data raw_data)
	{
		_boss_stage_id = raw_data.boss_stage_id;
		_boss_stage_group_id = raw_data.boss_stage_group_id;
		_wave_group_id = raw_data.wave_group_id;
		_stage_ordering = raw_data.stage_ordering;
		_stage_name = raw_data.stage_name;
		_character_exp = raw_data.character_exp;
		_destiny_exp = raw_data.destiny_exp;
		_gold = raw_data.gold;
		_repeat_reward_group_id = raw_data.repeat_reward_group_id;
		_first_reward_group_id = raw_data.first_reward_group_id;
		_recomment_level = raw_data.recomment_level;
		_bgm_path = raw_data.bgm_path;
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
		sb.AppendFormat("[boss_stage_id] = <color=yellow>{0}</color>", boss_stage_id).AppendLine();
		sb.AppendFormat("[boss_stage_group_id] = <color=yellow>{0}</color>", boss_stage_group_id).AppendLine();
		sb.AppendFormat("[wave_group_id] = <color=yellow>{0}</color>", wave_group_id).AppendLine();
		sb.AppendFormat("[stage_ordering] = <color=yellow>{0}</color>", stage_ordering).AppendLine();
		sb.AppendFormat("[stage_name] = <color=yellow>{0}</color>", stage_name).AppendLine();
		sb.AppendFormat("[character_exp] = <color=yellow>{0}</color>", character_exp).AppendLine();
		sb.AppendFormat("[destiny_exp] = <color=yellow>{0}</color>", destiny_exp).AppendLine();
		sb.AppendFormat("[gold] = <color=yellow>{0}</color>", gold).AppendLine();
		sb.AppendFormat("[repeat_reward_group_id] = <color=yellow>{0}</color>", repeat_reward_group_id).AppendLine();
		sb.AppendFormat("[first_reward_group_id] = <color=yellow>{0}</color>", first_reward_group_id).AppendLine();
		sb.AppendFormat("[recomment_level] = <color=yellow>{0}</color>", recomment_level).AppendLine();
		sb.AppendFormat("[bgm_path] = <color=yellow>{0}</color>", bgm_path).AppendLine();
		return sb.ToString();
	}
}

