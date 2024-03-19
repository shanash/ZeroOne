﻿#nullable disable


[System.Serializable]
public class Raw_Attribute_Icon_Data : System.IDisposable
{
	public ATTRIBUTE_TYPE attribute_type {get; set;}
	public string name_id {get; set;}
	public string icon {get; set;}
	public string color {get; set;}

	private bool disposed = false;

	public Raw_Attribute_Icon_Data()
	{
		attribute_type = ATTRIBUTE_TYPE.NONE;
		name_id = string.Empty;
		icon = string.Empty;
		color = string.Empty;
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
}

