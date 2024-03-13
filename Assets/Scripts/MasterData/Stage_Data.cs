#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Stage_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	스테이지 인덱스
	///	</summary>
	public int stage_id => _stage_id;
	int _stage_id;

	///	<summary>
	///	스테이지 그룹 ID
	///	</summary>
	public int stage_group_id => _stage_group_id;
	int _stage_group_id;

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
	///	스테이지 이름 ID
	///	</summary>
	public string stage_name_id => _stage_name_id;
	string _stage_name_id;

	///	<summary>
	///	기획팀 체크용
	///	</summary>
	public string stage_name => _stage_name;
	string _stage_name;

	///	<summary>
	///	사용 스태미나
	///	</summary>
	public int use_stamina => _use_stamina;
	int _use_stamina;

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
	///	별 보상
	///	</summary>
	public int star_reward_group_id => _star_reward_group_id;
	int _star_reward_group_id;

	///	<summary>
	///	스케쥴 ID
	///	</summary>
	public int schedule_id => _schedule_id;
	int _schedule_id;

	///	<summary>
	///	입장 제한 횟수
	///	</summary>
	public int entrance_limit_count => _entrance_limit_count;
	int _entrance_limit_count;

	///	<summary>
	///	대표 보상 타입
	///	</summary>
	public REWARD_TYPE reward_type => _reward_type;
	REWARD_TYPE _reward_type;

	///	<summary>
	///	대표 보상 ID
	///	</summary>
	public int reward_id => _reward_id;
	int _reward_id;

	///	<summary>
	///	스테이지 진입 전 재생할 다이얼로그
	///	</summary>
	public string entrance_dialogue => _entrance_dialogue;
	string _entrance_dialogue;

	///	<summary>
	///	스테이지 클리어 후 재생할 다이얼로그
	///	</summary>
	public string outrance_dialogue => _outrance_dialogue;
	string _outrance_dialogue;

	private bool disposed = false;

	public Stage_Data(Raw_Stage_Data raw_data)
	{
		_stage_id = raw_data.stage_id;
		_stage_group_id = raw_data.stage_group_id;
		_wave_group_id = raw_data.wave_group_id;
		_stage_ordering = raw_data.stage_ordering;
		_stage_name_id = raw_data.stage_name_id;
		_stage_name = raw_data.stage_name;
		_use_stamina = raw_data.use_stamina;
		_character_exp = raw_data.character_exp;
		_destiny_exp = raw_data.destiny_exp;
		_gold = raw_data.gold;
		_repeat_reward_group_id = raw_data.repeat_reward_group_id;
		_first_reward_group_id = raw_data.first_reward_group_id;
		_star_reward_group_id = raw_data.star_reward_group_id;
		_schedule_id = raw_data.schedule_id;
		_entrance_limit_count = raw_data.entrance_limit_count;
		_reward_type = raw_data.reward_type;
		_reward_id = raw_data.reward_id;
		_entrance_dialogue = raw_data.entrance_dialogue;
		_outrance_dialogue = raw_data.outrance_dialogue;
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
		sb.AppendFormat("[stage_group_id] = <color=yellow>{0}</color>", stage_group_id).AppendLine();
		sb.AppendFormat("[wave_group_id] = <color=yellow>{0}</color>", wave_group_id).AppendLine();
		sb.AppendFormat("[stage_ordering] = <color=yellow>{0}</color>", stage_ordering).AppendLine();
		sb.AppendFormat("[stage_name_id] = <color=yellow>{0}</color>", stage_name_id).AppendLine();
		sb.AppendFormat("[stage_name] = <color=yellow>{0}</color>", stage_name).AppendLine();
		sb.AppendFormat("[use_stamina] = <color=yellow>{0}</color>", use_stamina).AppendLine();
		sb.AppendFormat("[character_exp] = <color=yellow>{0}</color>", character_exp).AppendLine();
		sb.AppendFormat("[destiny_exp] = <color=yellow>{0}</color>", destiny_exp).AppendLine();
		sb.AppendFormat("[gold] = <color=yellow>{0}</color>", gold).AppendLine();
		sb.AppendFormat("[repeat_reward_group_id] = <color=yellow>{0}</color>", repeat_reward_group_id).AppendLine();
		sb.AppendFormat("[first_reward_group_id] = <color=yellow>{0}</color>", first_reward_group_id).AppendLine();
		sb.AppendFormat("[star_reward_group_id] = <color=yellow>{0}</color>", star_reward_group_id).AppendLine();
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[entrance_limit_count] = <color=yellow>{0}</color>", entrance_limit_count).AppendLine();
		sb.AppendFormat("[reward_type] = <color=yellow>{0}</color>", reward_type).AppendLine();
		sb.AppendFormat("[reward_id] = <color=yellow>{0}</color>", reward_id).AppendLine();
		sb.AppendFormat("[entrance_dialogue] = <color=yellow>{0}</color>", entrance_dialogue).AppendLine();
		sb.AppendFormat("[outrance_dialogue] = <color=yellow>{0}</color>", outrance_dialogue).AppendLine();
		return sb.ToString();
	}
}

