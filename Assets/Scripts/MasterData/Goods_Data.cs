using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Goods_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int item_id {get; set;}
	///	<summary>
	///	이름 string ID
	///	</summary>
	public string name_id {get; set;}
	///	<summary>
	///	아이템 타입
	///	</summary>
	public GOODS_TYPE goods_type {get; set;}
	///	<summary>
	///	base 최대값
	///	money 유형의 재화는 이 칼럼 미사용
	///	</summary>
	public int base_max {get; set;}
	///	<summary>
	///	limit 최대값
	///	player가 보유할 수 있는 최대 수치
	///	</summary>
	public int limit_max {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path {get; set;}

	private bool disposed = false;

	public Goods_Data()
	{
		item_id = 0;
		name_id = string.Empty;
		goods_type = GOODS_TYPE.NONE;
		base_max = 0;
		limit_max = 0;
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
		sb.AppendFormat("[item_id] = <color=yellow>{0}</color>", item_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[goods_type] = <color=yellow>{0}</color>", goods_type).AppendLine();
		sb.AppendFormat("[base_max] = <color=yellow>{0}</color>", base_max).AppendLine();
		sb.AppendFormat("[limit_max] = <color=yellow>{0}</color>", limit_max).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

