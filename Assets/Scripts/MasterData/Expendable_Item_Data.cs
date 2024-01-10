using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Expendable_Item_Data : System.IDisposable
{
	///	<summary>
	///	소모용 아이템 id
	///	</summary>
	public int expendable_item_id {get; set;}
	///	<summary>
	///	이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	툴팁
	///	</summary>
	public string tooltip_text {get; set;}
	///	<summary>
	///	판매가격
	///	</summary>
	public int sell_price {get; set;}
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Expendable_Item_Data()
	{
		expendable_item_id = 0;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sell_price = 0;
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
		sb.AppendFormat("[expendable_item_id] = <color=yellow>{0}</color>", expendable_item_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tooltip_text] = <color=yellow>{0}</color>", tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

