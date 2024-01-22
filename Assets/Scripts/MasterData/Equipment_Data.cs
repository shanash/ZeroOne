public class Equipment_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public readonly int item_id;
	///	<summary>
	///	이름 string ID
	///	</summary>
	public readonly string name_id;
	///	<summary>
	///	장비 타입
	///	</summary>
	public readonly EQUIPMENT_TYPE equipment_type;
	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public readonly int max_num;
	///	<summary>
	///	방어력
	///	</summary>
	public readonly int def;
	///	<summary>
	///	최대HP
	///	</summary>
	public readonly int max_hp;
	///	<summary>
	///	회피
	///	</summary>
	public readonly int evasion;
	///	<summary>
	///	HP 자동회복
	///	</summary>
	public readonly int recover_hp;
	///	<summary>
	///	HP 흡수
	///	</summary>
	public readonly int drain_hp;
	///	<summary>
	///	명중
	///	</summary>
	public readonly int hit;
	///	<summary>
	///	마법 공격력
	///	</summary>
	public readonly int matk;
	///	<summary>
	///	물리 공격력
	///	</summary>
	public readonly int atk;
	///	<summary>
	///	마법 크리티컬
	///	</summary>
	public readonly int hit_mcri;
	///	<summary>
	///	물리 크리티컬
	///	</summary>
	public readonly int hit_cri;
	///	<summary>
	///	회복량 상승
	///	</summary>
	public readonly int heal_up;
	///	<summary>
	///	소비 시간(분)
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public readonly int expire_time;
	///	<summary>
	///	소비기한
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public readonly int schedule_id;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Equipment_Data(Raw_Equipment_Data raw_data)
	{
		item_id = raw_data.item_id;
		name_id = raw_data.name_id;
		equipment_type = raw_data.equipment_type;
		max_num = raw_data.max_num;
		def = raw_data.def;
		max_hp = raw_data.max_hp;
		evasion = raw_data.evasion;
		recover_hp = raw_data.recover_hp;
		drain_hp = raw_data.drain_hp;
		hit = raw_data.hit;
		matk = raw_data.matk;
		atk = raw_data.atk;
		hit_mcri = raw_data.hit_mcri;
		hit_cri = raw_data.hit_cri;
		heal_up = raw_data.heal_up;
		expire_time = raw_data.expire_time;
		schedule_id = raw_data.schedule_id;
		icon_path = raw_data.icon_path;
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

