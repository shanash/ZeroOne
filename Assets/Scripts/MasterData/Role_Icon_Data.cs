public class Role_Icon_Data : System.IDisposable
{
	///	<summary>
	///	역할 타입
	///	</summary>
	public readonly ROLE_TYPE role_type;
	///	<summary>
	///	이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon;
	///	<summary>
	///	카드 아이콘
	///	</summary>
	public readonly string card_icon;
	///	<summary>
	///	태그 bg
	///	</summary>
	public readonly string tag_bg_path;

	private bool disposed = false;

	public Role_Icon_Data(Raw_Role_Icon_Data raw_data)
	{
		role_type = raw_data.role_type;
		name_kr = raw_data.name_kr;
		icon = raw_data.icon;
		card_icon = raw_data.card_icon;
		tag_bg_path = raw_data.tag_bg_path;
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

