﻿#nullable disable


[System.Serializable]
public class Raw_L2d_Interaction_Base_Data : System.IDisposable
{
	public int interaction_group_id {get; set;}
	public bool check_using_essense {get; set;}
	public TOUCH_BODY_TYPE touch_type_01 {get; set;}
	public TOUCH_GESTURE_TYPE gescure_type_01 {get; set;}
	public int reaction_timeline_id {get; set;}
	public int reaction_ani_id {get; set;}
	public int reaction_facial_id {get; set;}
	public int after_state_id {get; set;}

	private bool disposed = false;

	public Raw_L2d_Interaction_Base_Data()
	{
		interaction_group_id = 0;
		check_using_essense = false;
		touch_type_01 = TOUCH_BODY_TYPE.NONE;
		gescure_type_01 = TOUCH_GESTURE_TYPE.NONE;
		reaction_timeline_id = 0;
		reaction_ani_id = 0;
		reaction_facial_id = 0;
		after_state_id = 0;
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

