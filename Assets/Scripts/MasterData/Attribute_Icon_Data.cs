#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Attribute_Icon_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	역할 타입
	///	</summary>
	public ATTRIBUTE_TYPE attribute_type => _attribute_type;
	ATTRIBUTE_TYPE _attribute_type;

	///	<summary>
	///	이름
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon => _icon;
	string _icon;

	///	<summary>
	///	속성 컬러
	///	</summary>
	public string color => _color;
	string _color;

	private bool disposed = false;

	public Attribute_Icon_Data(Raw_Attribute_Icon_Data raw_data)
	{
		_attribute_type = raw_data.attribute_type;
		_name_id = raw_data.name_id;
		_icon = raw_data.icon;
		_color = raw_data.color;
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
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		sb.AppendFormat("[color] = <color=yellow>{0}</color>", color).AppendLine();
		return sb.ToString();
	}
}

