[System.Serializable]
public class Role_Icon_Data : System.IDisposable
{
	///	<summary>
	///	역할 타입
	///	</summary>
	public ROLE_TYPE role_type {get; set;}
	///	<summary>
	///	이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	아이콘
	///	</summary>
	public string icon {get; set;}
	///	<summary>
	///	카드 아이콘
	///	</summary>
	public string card_icon {get; set;}
	///	<summary>
	///	태그 bg
	///	</summary>
	public string tag_bg_path {get; set;}

	private bool disposed = false;

	public Role_Icon_Data()
	{
		role_type = ROLE_TYPE.NONE;
		name_kr = string.Empty;
		icon = string.Empty;
		card_icon = string.Empty;
		tag_bg_path = string.Empty;
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
		sb.AppendFormat("[role_type] = <color=yellow>{0}</color>", role_type).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		sb.AppendFormat("[card_icon] = <color=yellow>{0}</color>", card_icon).AppendLine();
		sb.AppendFormat("[tag_bg_path] = <color=yellow>{0}</color>", tag_bg_path).AppendLine();
		return sb.ToString();
	}
}

