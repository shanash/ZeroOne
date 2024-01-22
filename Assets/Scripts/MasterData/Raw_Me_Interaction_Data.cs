

[System.Serializable]
public class Raw_Me_Interaction_Data : System.IDisposable
{
	public int interaction_id {get; set;}
	public int player_character_id {get; set;}
	public TOUCH_BODY_TYPE touch_body_type {get; set;}
	public string touch_body_direction {get; set;}
	public TOUCH_GESTURE_TYPE touch_gesture_type {get; set;}
	public int condition_min_gesture_count {get; set;}
	public int condition_max_gesture_count {get; set;}
	public string drag_animation_name {get; set;}
	public int[] chat_motion_ids {get; set;}
	public int[] condition_state_ids {get; set;}
	public int change_state_id {get; set;}

	private bool disposed = false;

	public Raw_Me_Interaction_Data()
	{
		interaction_id = 0;
		player_character_id = 0;
		touch_body_type = TOUCH_BODY_TYPE.NONE;
		touch_body_direction = string.Empty;
		touch_gesture_type = TOUCH_GESTURE_TYPE.NONE;
		condition_min_gesture_count = 0;
		condition_max_gesture_count = 0;
		drag_animation_name = string.Empty;
		change_state_id = 0;
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

