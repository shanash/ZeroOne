using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Star_Mission_Data : System.IDisposable
{
	///	<summary>
	///	Star Mission ID
	///	</summary>
	public int star_mission_id {get; set;}

	private bool disposed = false;

	public Star_Mission_Data()
	{
		star_mission_id = 0;
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
		sb.AppendFormat("[star_mission_id] = <color=yellow>{0}</color>", star_mission_id).AppendLine();
		return sb.ToString();
	}
}

