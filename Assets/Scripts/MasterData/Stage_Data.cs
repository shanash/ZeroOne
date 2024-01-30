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
	///	지역 id
	///	</summary>
	public int zone_id => _zone_id;
	int _zone_id;

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

	private bool disposed = false;

	public Stage_Data(Raw_Stage_Data raw_data)
	{
		_stage_id = raw_data.stage_id;
		_zone_id = raw_data.zone_id;
		_stage_ordering = raw_data.stage_ordering;
		_stage_name = raw_data.stage_name;
		_use_stamina = raw_data.use_stamina;
		_character_exp = raw_data.character_exp;
		_destiny_exp = raw_data.destiny_exp;
		_gold = raw_data.gold;
		_repeat_reward_group_id = raw_data.repeat_reward_group_id;
		_first_reward_group_id = raw_data.first_reward_group_id;
		_star_reward_group_id = raw_data.star_reward_group_id;
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
		sb.AppendFormat("[zone_id] = <color=yellow>{0}</color>", zone_id).AppendLine();
		sb.AppendFormat("[stage_ordering] = <color=yellow>{0}</color>", stage_ordering).AppendLine();
		sb.AppendFormat("[stage_name] = <color=yellow>{0}</color>", stage_name).AppendLine();
		sb.AppendFormat("[use_stamina] = <color=yellow>{0}</color>", use_stamina).AppendLine();
		sb.AppendFormat("[character_exp] = <color=yellow>{0}</color>", character_exp).AppendLine();
		sb.AppendFormat("[destiny_exp] = <color=yellow>{0}</color>", destiny_exp).AppendLine();
		sb.AppendFormat("[gold] = <color=yellow>{0}</color>", gold).AppendLine();
		sb.AppendFormat("[repeat_reward_group_id] = <color=yellow>{0}</color>", repeat_reward_group_id).AppendLine();
		sb.AppendFormat("[first_reward_group_id] = <color=yellow>{0}</color>", first_reward_group_id).AppendLine();
		sb.AppendFormat("[star_reward_group_id] = <color=yellow>{0}</color>", star_reward_group_id).AppendLine();
		return sb.ToString();
	}
}

