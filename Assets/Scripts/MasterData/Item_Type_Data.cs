using FluffyDuck.Util;
using System.Linq;

public class Item_Type_Data : System.IDisposable
{
	///	<summary>
	///	아이템 타입
	///	</summary>
	public ITEM_TYPE item_type => _item_type;
	ITEM_TYPE _item_type;

	///	<summary>
	///	이름
	///	</summary>
	public string name_kr => _name_kr;
	string _name_kr;

	///	<summary>
	///	툴팁
	///	</summary>
	public string tooltip_text => _tooltip_text;
	string _tooltip_text;

	///	<summary>
	///	판매 가능 여부
	///	</summary>
	public bool sellable => _sellable;
	bool _sellable;

	///	<summary>
	///	최대 보유 한도
	///	</summary>
	public double max_bounds => _max_bounds;
	double _max_bounds;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Item_Type_Data(Raw_Item_Type_Data raw_data)
	{
		_item_type = raw_data.item_type;
		_name_kr = raw_data.name_kr;
		_tooltip_text = raw_data.tooltip_text;
		_sellable = raw_data.sellable;
		_max_bounds = raw_data.max_bounds;
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
		sb.AppendFormat("[item_type] = <color=yellow>{0}</color>", item_type).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sellable] = <color=yellow>{0}</color>", sellable).AppendLine();
		sb.AppendFormat("[max_bounds] = <color=yellow>{0}</color>", max_bounds).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

