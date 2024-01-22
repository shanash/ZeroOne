public class Stage_Data : System.IDisposable
{
	///	<summary>
	///	스테이지 인덱스
	///	</summary>
	public readonly int stage_id;
	///	<summary>
	///	지역 id
	///	</summary>
	public readonly int zone_id;
	///	<summary>
	///	스테이지 넘버링
	///	</summary>
	public readonly int stage_ordering;
	///	<summary>
	///	스테이지 명칭
	///	</summary>
	public readonly string stage_name;
	///	<summary>
	///	사용 스태미나
	///	</summary>
	public readonly int use_stamina;
	///	<summary>
	///	클리어 시 캐릭터 경험치
	///	</summary>
	public readonly int character_exp;
	///	<summary>
	///	호감도 경험치
	///	</summary>
	public readonly int destiny_exp;
	///	<summary>
	///	지급 골드
	///	</summary>
	public readonly int gold;
	///	<summary>
	///	통상 보상
	///	</summary>
	public readonly int repeat_reward_group_id;
	///	<summary>
	///	초회 보상
	///	</summary>
	public readonly int first_reward_group_id;
	///	<summary>
	///	별 보상
	///	</summary>
	public readonly int star_reward_group_id;

	private bool disposed = false;

	public Stage_Data(Raw_Stage_Data raw_data)
	{
		stage_id = raw_data.stage_id;
		zone_id = raw_data.zone_id;
		stage_ordering = raw_data.stage_ordering;
		stage_name = raw_data.stage_name;
		use_stamina = raw_data.use_stamina;
		character_exp = raw_data.character_exp;
		destiny_exp = raw_data.destiny_exp;
		gold = raw_data.gold;
		repeat_reward_group_id = raw_data.repeat_reward_group_id;
		first_reward_group_id = raw_data.first_reward_group_id;
		star_reward_group_id = raw_data.star_reward_group_id;
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

