using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Favorite_Item_Data : System.IDisposable
{
	///	<summary>
	///	인덱스
	///	</summary>
	public int item_index {get; set;}
	///	<summary>
	///	한글명
	///	</summary>
	public string item_name_kr {get; set;}
	///	<summary>
	///	사용 효과
	///	</summary>
	public int use_effect {get; set;}
	///	<summary>
	///	툴팁 TEXT
	///	</summary>
	public string item_tooltip_text {get; set;}
	///	<summary>
	///	판매 가격
	///	</summary>
	public int sell_price {get; set;}
	///	<summary>
	///	아이콘 경로
	///	</summary>
	public int icon_path {get; set;}

	private bool disposed = false;

	public Favorite_Item_Data()
	{
		item_index = 0;
		item_name_kr = string.Empty;
		use_effect = 0;
		item_tooltip_text = string.Empty;
		sell_price = 0;
		icon_path = 0;
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
		sb.AppendFormat("[item_index] = <color=yellow>{0}</color>", item_index).AppendLine();
		sb.AppendFormat("[item_name_kr] = <color=yellow>{0}</color>", item_name_kr).AppendLine();
		sb.AppendFormat("[use_effect] = <color=yellow>{0}</color>", use_effect).AppendLine();
		sb.AppendFormat("[item_tooltip_text] = <color=yellow>{0}</color>", item_tooltip_text).AppendLine();
		sb.AppendFormat("[sell_price] = <color=yellow>{0}</color>", sell_price).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

