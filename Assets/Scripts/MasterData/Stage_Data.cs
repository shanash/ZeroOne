using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Stage_Data : System.IDisposable
{
	///	<summary>
	///	스테이지 인덱스
	///	</summary>
	public int stage_id {get; set;}
	///	<summary>
	///	지역 id
	///	</summary>
	public int zone_id {get; set;}
	///	<summary>
	///	스테이지 넘버링
	///	</summary>
	public int stage_ordering {get; set;}
	///	<summary>
	///	스테이지 명칭
	///	</summary>
	public string stage_name {get; set;}
	///	<summary>
	///	사용 스태미나
	///	</summary>
	public int use_stamina {get; set;}
	///	<summary>
	///	클리어 시 캐릭터 경험치
	///	</summary>
	public int character_exp {get; set;}
	///	<summary>
	///	호감도 경험치
	///	</summary>
	public int destiny_exp {get; set;}
	///	<summary>
	///	지급 골드
	///	</summary>
	public int gold {get; set;}
	///	<summary>
	///	초회 보상
	///	</summary>
	public int first_reward_id {get; set;}
	///	<summary>
	///	통상 보상
	///	</summary>
	public int repeat_reward_id {get; set;}

	private bool disposed = false;

	public Stage_Data()
	{
		stage_id = 0;
		zone_id = 0;
		stage_ordering = 0;
		stage_name = string.Empty;
		use_stamina = 0;
		character_exp = 0;
		destiny_exp = 0;
		gold = 0;
		first_reward_id = 0;
		repeat_reward_id = 0;
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
		sb.AppendFormat("[first_reward_id] = <color=yellow>{0}</color>", first_reward_id).AppendLine();
		sb.AppendFormat("[repeat_reward_id] = <color=yellow>{0}</color>", repeat_reward_id).AppendLine();
		return sb.ToString();
	}
}

