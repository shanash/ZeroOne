[System.Serializable]
public class Equipment_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id {get; set;}
	///	<summary>
	///	장비 타입
	///	</summary>
	public EQUIPMENT_TYPE equipment_type {get; set;}
	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num {get; set;}
	///	<summary>
	///	방어력
	///	</summary>
	public int def {get; set;}
	///	<summary>
	///	최대HP
	///	</summary>
	public int max_hp {get; set;}
	///	<summary>
	///	회피
	///	</summary>
	public int evasion {get; set;}
	///	<summary>
	///	HP 자동회복
	///	</summary>
	public int recover_hp {get; set;}
	///	<summary>
	///	HP 흡수
	///	</summary>
	public int drain_hp {get; set;}
	///	<summary>
	///	명중
	///	</summary>
	public int hit {get; set;}
	///	<summary>
	///	마법 공격력
	///	</summary>
	public int matk {get; set;}
	///	<summary>
	///	물리 공격력
	///	</summary>
	public int atk {get; set;}
	///	<summary>
	///	마법 크리티컬
	///	</summary>
	public int hit_mcri {get; set;}
	///	<summary>
	///	물리 크리티컬
	///	</summary>
	public int hit_cri {get; set;}
	///	<summary>
	///	회복량 상승
	///	</summary>
	public int heal_up {get; set;}
	///	<summary>
	///	소비 시간(분)
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time {get; set;}
	///	<summary>
	///	소비기한
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public int schedule_id {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Equipment_Data()
	{
		item_id = 0;
		name_id = string.Empty;
		equipment_type = EQUIPMENT_TYPE.NONE;
		max_num = 0;
		def = 0;
		max_hp = 0;
		evasion = 0;
		recover_hp = 0;
		drain_hp = 0;
		hit = 0;
		matk = 0;
		atk = 0;
		hit_mcri = 0;
		hit_cri = 0;
		heal_up = 0;
		expire_time = 0;
		schedule_id = 0;
		icon_path = string.Empty;
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
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[equipment_type] = <color=yellow>{0}</color>", equipment_type).AppendLine();
		sb.AppendFormat("[max_num] = <color=yellow>{0}</color>", max_num).AppendLine();
		sb.AppendFormat("[def] = <color=yellow>{0}</color>", def).AppendLine();
		sb.AppendFormat("[max_hp] = <color=yellow>{0}</color>", max_hp).AppendLine();
		sb.AppendFormat("[evasion] = <color=yellow>{0}</color>", evasion).AppendLine();
		sb.AppendFormat("[recover_hp] = <color=yellow>{0}</color>", recover_hp).AppendLine();
		sb.AppendFormat("[drain_hp] = <color=yellow>{0}</color>", drain_hp).AppendLine();
		sb.AppendFormat("[hit] = <color=yellow>{0}</color>", hit).AppendLine();
		sb.AppendFormat("[matk] = <color=yellow>{0}</color>", matk).AppendLine();
		sb.AppendFormat("[atk] = <color=yellow>{0}</color>", atk).AppendLine();
		sb.AppendFormat("[hit_mcri] = <color=yellow>{0}</color>", hit_mcri).AppendLine();
		sb.AppendFormat("[hit_cri] = <color=yellow>{0}</color>", hit_cri).AppendLine();
		sb.AppendFormat("[heal_up] = <color=yellow>{0}</color>", heal_up).AppendLine();
		sb.AppendFormat("[expire_time] = <color=yellow>{0}</color>", expire_time).AppendLine();
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

