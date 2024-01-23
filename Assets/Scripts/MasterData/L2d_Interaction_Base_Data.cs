public class L2d_Interaction_Base_Data : System.IDisposable
{
	///	<summary>
	///	interaction_group_id
	///	</summary>
	public readonly int interaction_group_id;
	///	<summary>
	///	터치 부위 타입
	///	</summary>
	public readonly TOUCH_BODY_TYPE touch_type_01;
	///	<summary>
	///	터치 형태 타입 1
	///	</summary>
	public readonly TOUCH_GESTURE_TYPE gescure_type_01;
	///	<summary>
	///	터치 반응 애니 id
	///	</summary>
	public readonly int[] reaction_ani_id;
	///	<summary>
	///	터치 반응 페이셜 id
	///	</summary>
	public readonly int[] reaction_facial_id;
	///	<summary>
	///	애니 재생 후, 변환될 상태 id
	///	</summary>
	public readonly int after_state_id;

	private bool disposed = false;

	public L2d_Interaction_Base_Data(Raw_L2d_Interaction_Base_Data raw_data)
	{
		interaction_group_id = raw_data.interaction_group_id;
		touch_type_01 = raw_data.touch_type_01;
		gescure_type_01 = raw_data.gescure_type_01;
		reaction_ani_id = raw_data.reaction_ani_id != null ? (int[])raw_data.reaction_ani_id.Clone() : new int[0];
		reaction_facial_id = raw_data.reaction_facial_id != null ? (int[])raw_data.reaction_facial_id.Clone() : new int[0];
		after_state_id = raw_data.after_state_id;
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
		int cnt = 0;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendFormat("[interaction_group_id] = <color=yellow>{0}</color>", interaction_group_id).AppendLine();
		sb.AppendFormat("[touch_type_01] = <color=yellow>{0}</color>", touch_type_01).AppendLine();
		sb.AppendFormat("[gescure_type_01] = <color=yellow>{0}</color>", gescure_type_01).AppendLine();
		sb.AppendLine("[reaction_ani_id]");
		if(reaction_ani_id != null)
		{
			cnt = reaction_ani_id.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", reaction_ani_id[i]).AppendLine();
			}
		}

		sb.AppendLine("[reaction_facial_id]");
		if(reaction_facial_id != null)
		{
			cnt = reaction_facial_id.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", reaction_facial_id[i]).AppendLine();
			}
		}

		sb.AppendFormat("[after_state_id] = <color=yellow>{0}</color>", after_state_id).AppendLine();
		return sb.ToString();
	}
}

