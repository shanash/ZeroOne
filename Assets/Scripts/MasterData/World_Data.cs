using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class World_Data : System.IDisposable
{
	///	<summary>
	///	월드 인덱스
	///	</summary>
	public int world_id {get; set;}
	///	<summary>
	///	월드 명칭
	///	</summary>
	public string world_name {get; set;}

	private bool disposed = false;

	public World_Data()
	{
		world_id = 0;
		world_name = string.Empty;
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
		sb.AppendFormat("[world_id] = <color=yellow>{0}</color>", world_id).AppendLine();
		sb.AppendFormat("[world_name] = <color=yellow>{0}</color>", world_name).AppendLine();
		return sb.ToString();
	}
}

