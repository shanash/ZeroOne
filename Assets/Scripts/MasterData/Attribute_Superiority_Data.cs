#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Attribute_Superiority_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	공격 에너지
	///	</summary>
	public ATTRIBUTE_TYPE attacker_attribute_type => _attacker_attribute_type;
	ATTRIBUTE_TYPE _attacker_attribute_type;

	///	<summary>
	///	공격 받는 속성
	///	</summary>
	public ATTRIBUTE_TYPE defender_attribute_type => _defender_attribute_type;
	ATTRIBUTE_TYPE _defender_attribute_type;

	///	<summary>
	///	최종 데미지 추가 가중 비율
	///	</summary>
	public double final_damage_per => _final_damage_per;
	double _final_damage_per;

	private bool disposed = false;

	public Attribute_Superiority_Data(Raw_Attribute_Superiority_Data raw_data)
	{
		_attacker_attribute_type = raw_data.attacker_attribute_type;
		_defender_attribute_type = raw_data.defender_attribute_type;
		_final_damage_per = raw_data.final_damage_per;
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
		sb.AppendFormat("[attacker_attribute_type] = <color=yellow>{0}</color>", attacker_attribute_type).AppendLine();
		sb.AppendFormat("[defender_attribute_type] = <color=yellow>{0}</color>", defender_attribute_type).AppendLine();
		sb.AppendFormat("[final_damage_per] = <color=yellow>{0}</color>", final_damage_per).AppendLine();
		return sb.ToString();
	}
}

