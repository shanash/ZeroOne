using FluffyDuck.Util;
using System.Linq;

public class Exp_Potion_Data : System.IDisposable
{
	///	<summary>
	///	경험치 물약 id
	///	</summary>
	public int exp_potion_id => _exp_potion_id;
	int _exp_potion_id;

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
	///	판매가격
	///	</summary>
	public int sell_price => _sell_price;
	int _sell_price;

	///	<summary>
	///	사용 효과
	///	</summary>
	public int use_effect => _use_effect;
	int _use_effect;

	///	<summary>
	///	아이콘 경로
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	private bool disposed = false;

	public Exp_Potion_Data(Raw_Exp_Potion_Data raw_data)
	{
		_exp_potion_id = raw_data.exp_potion_id;
		_name_kr = raw_data.name_kr;
		_tooltip_text = raw_data.tooltip_text;
		_sell_price = raw_data.sell_price;
		_use_effect = raw_data.use_effect;
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
		sb.AppendFormat("[exp_potion_id] = <color=yellow>{0}</color>", exp_potion_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[use_effect] = <color=yellow>{0}</color>", use_effect).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

