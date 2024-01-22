using FluffyDuck.Util;
using System.Linq;

public class Goods_Data : System.IDisposable
{
	///	<summary>
	///	재화 타입
	///	</summary>
	public GOODS_TYPE goods_type => _goods_type;
	GOODS_TYPE _goods_type;

	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	최대값
	///	</summary>
	public double max_bound => _max_bound;
	double _max_bound;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Goods_Data(Raw_Goods_Data raw_data)
	{
		_goods_type = raw_data.goods_type;
		_name_id = raw_data.name_id;
		_max_bound = raw_data.max_bound;
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
		sb.AppendFormat("[goods_type] = <color=yellow>{0}</color>", goods_type).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[max_bound] = <color=yellow>{0}</color>", max_bound).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

