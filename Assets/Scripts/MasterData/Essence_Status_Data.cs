#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Essence_Status_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	도달 전달률
	///	</summary>
	public int essence_charge_per => _essence_charge_per;
	int _essence_charge_per;

	///	<summary>
	///	증가 물리 공격력
	///	</summary>
	public int add_atk => _add_atk;
	int _add_atk;

	///	<summary>
	///	증가 마법 공격력
	///	</summary>
	public int add_matk => _add_matk;
	int _add_matk;

	///	<summary>
	///	증가 물리 방어력
	///	</summary>
	public int add_def => _add_def;
	int _add_def;

	///	<summary>
	///	증가 마법 방어력
	///	</summary>
	public int add_mdef => _add_mdef;
	int _add_mdef;

	///	<summary>
	///	증가 최대 체력
	///	</summary>
	public int add_hp => _add_hp;
	int _add_hp;

	private bool disposed = false;

	public Essence_Status_Data(Raw_Essence_Status_Data raw_data)
	{
		_essence_charge_per = raw_data.essence_charge_per;
		_add_atk = raw_data.add_atk;
		_add_matk = raw_data.add_matk;
		_add_def = raw_data.add_def;
		_add_mdef = raw_data.add_mdef;
		_add_hp = raw_data.add_hp;
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
		sb.AppendFormat("[essence_charge_per] = <color=yellow>{0}</color>", essence_charge_per).AppendLine();
		sb.AppendFormat("[add_atk] = <color=yellow>{0}</color>", add_atk).AppendLine();
		sb.AppendFormat("[add_matk] = <color=yellow>{0}</color>", add_matk).AppendLine();
		sb.AppendFormat("[add_def] = <color=yellow>{0}</color>", add_def).AppendLine();
		sb.AppendFormat("[add_mdef] = <color=yellow>{0}</color>", add_mdef).AppendLine();
		sb.AppendFormat("[add_hp] = <color=yellow>{0}</color>", add_hp).AppendLine();
		return sb.ToString();
	}
}

