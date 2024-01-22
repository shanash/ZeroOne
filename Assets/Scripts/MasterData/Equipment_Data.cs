using FluffyDuck.Util;
using System.Linq;

public class Equipment_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id => _item_id;
	int _item_id;

	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	장비 타입
	///	</summary>
	public EQUIPMENT_TYPE equipment_type => _equipment_type;
	EQUIPMENT_TYPE _equipment_type;

	///	<summary>
	///	최대 중첩 갯수
	///	</summary>
	public int max_num => _max_num;
	int _max_num;

	///	<summary>
	///	방어력
	///	</summary>
	public int def => _def;
	int _def;

	///	<summary>
	///	최대HP
	///	</summary>
	public int max_hp => _max_hp;
	int _max_hp;

	///	<summary>
	///	회피
	///	</summary>
	public int evasion => _evasion;
	int _evasion;

	///	<summary>
	///	HP 자동회복
	///	</summary>
	public int recover_hp => _recover_hp;
	int _recover_hp;

	///	<summary>
	///	HP 흡수
	///	</summary>
	public int drain_hp => _drain_hp;
	int _drain_hp;

	///	<summary>
	///	명중
	///	</summary>
	public int hit => _hit;
	int _hit;

	///	<summary>
	///	마법 공격력
	///	</summary>
	public int matk => _matk;
	int _matk;

	///	<summary>
	///	물리 공격력
	///	</summary>
	public int atk => _atk;
	int _atk;

	///	<summary>
	///	마법 크리티컬
	///	</summary>
	public int hit_mcri => _hit_mcri;
	int _hit_mcri;

	///	<summary>
	///	물리 크리티컬
	///	</summary>
	public int hit_cri => _hit_cri;
	int _hit_cri;

	///	<summary>
	///	회복량 상승
	///	</summary>
	public int heal_up => _heal_up;
	int _heal_up;

	///	<summary>
	///	소비 시간(분)
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 시간 없음
	///	</summary>
	public int expire_time => _expire_time;
	int _expire_time;

	///	<summary>
	///	소비기한
	///	소비 시간(기한)이 만료되면, 아이템이 사라짐
	///	값이 0 이면, 소비 기한 없음
	///	</summary>
	public int schedule_id => _schedule_id;
	int _schedule_id;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Equipment_Data(Raw_Equipment_Data raw_data)
	{
		_item_id = raw_data.item_id;
		_name_id = raw_data.name_id;
		_equipment_type = raw_data.equipment_type;
		_max_num = raw_data.max_num;
		_def = raw_data.def;
		_max_hp = raw_data.max_hp;
		_evasion = raw_data.evasion;
		_recover_hp = raw_data.recover_hp;
		_drain_hp = raw_data.drain_hp;
		_hit = raw_data.hit;
		_matk = raw_data.matk;
		_atk = raw_data.atk;
		_hit_mcri = raw_data.hit_mcri;
		_hit_cri = raw_data.hit_cri;
		_heal_up = raw_data.heal_up;
		_expire_time = raw_data.expire_time;
		_schedule_id = raw_data.schedule_id;
		_icon_path = raw_data.icon_path;
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

