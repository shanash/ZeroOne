[System.Serializable]
public class Reward_Set_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int reward_id {get; set;}
	///	<summary>
	///	보상 타입
	///	</summary>
	public GOODS_TYPE goods_type {get; set;}
	///	<summary>
	///	변수1
	///	</summary>
	public int var1 {get; set;}
	///	<summary>
	///	변수2
	///	</summary>
	public int var2 {get; set;}
	///	<summary>
	///	출현 타입
	///	</summary>
	public int drop_type {get; set;}
	///	<summary>
	///	출현확률
	///	</summary>
	public int drop_per {get; set;}
	///	<summary>
	///	노출 여부
	///	</summary>
	public bool is_use {get; set;}
	///	<summary>
	///	보상 노출 순서
	///	</summary>
	public int sort_order {get; set;}

	private bool disposed = false;

	public Reward_Set_Data()
	{
		reward_id = 0;
		goods_type = GOODS_TYPE.NONE;
		var1 = 0;
		var2 = 0;
		drop_type = 0;
		drop_per = 0;
		is_use = false;
		sort_order = 0;
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
		sb.AppendFormat("[reward_id] = <color=yellow>{0}</color>", reward_id).AppendLine();
		sb.AppendFormat("[goods_type] = <color=yellow>{0}</color>", goods_type).AppendLine();
		sb.AppendFormat("[var1] = <color=yellow>{0}</color>", var1).AppendLine();
		sb.AppendFormat("[var2] = <color=yellow>{0}</color>", var2).AppendLine();
		sb.AppendFormat("[drop_type] = <color=yellow>{0}</color>", drop_type).AppendLine();
		sb.AppendFormat("[drop_per] = <color=yellow>{0}</color>", drop_per).AppendLine();
		sb.AppendFormat("[is_use] = <color=yellow>{0}</color>", is_use).AppendLine();
		sb.AppendFormat("[sort_order] = <color=yellow>{0}</color>", sort_order).AppendLine();
		return sb.ToString();
	}
}

