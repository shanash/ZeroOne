#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Attribute_Synergy_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	팀 배치 중인 속성
	///	</summary>
	public ATTRIBUTE_TYPE attribute_type => _attribute_type;
	ATTRIBUTE_TYPE _attribute_type;

	///	<summary>
	///	<b>key_2</b><br/>
	///	같은 속성 캐릭터 수
	///	</summary>
	public int same_attribute_count => _same_attribute_count;
	int _same_attribute_count;

	///	<summary>
	///	스탯 멀티플 타입
	///	</summary>
	public STAT_MULTIPLE_TYPE multiple_type => _multiple_type;
	STAT_MULTIPLE_TYPE _multiple_type;

	///	<summary>
	///	시너지 추가 가중 비율
	///	</summary>
	public double add_damage_per => _add_damage_per;
	double _add_damage_per;

	private bool disposed = false;

	public Attribute_Synergy_Data(Raw_Attribute_Synergy_Data raw_data)
	{
		_attribute_type = raw_data.attribute_type;
		_same_attribute_count = raw_data.same_attribute_count;
		_multiple_type = raw_data.multiple_type;
		_add_damage_per = raw_data.add_damage_per;
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
		sb.AppendFormat("[attribute_type] = <color=yellow>{0}</color>", attribute_type).AppendLine();
		sb.AppendFormat("[same_attribute_count] = <color=yellow>{0}</color>", same_attribute_count).AppendLine();
		sb.AppendFormat("[multiple_type] = <color=yellow>{0}</color>", multiple_type).AppendLine();
		sb.AppendFormat("[add_damage_per] = <color=yellow>{0}</color>", add_damage_per).AppendLine();
		return sb.ToString();
	}
}

