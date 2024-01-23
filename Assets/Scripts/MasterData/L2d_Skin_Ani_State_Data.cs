using FluffyDuck.Util;
using System.Linq;

public class L2d_Skin_Ani_State_Data : System.IDisposable
{
	///	<summary>
	///	상태 id
	///	</summary>
	public int state_id => _state_id;
	int _state_id;

	///	<summary>
	///	기본 idle 애니 ID
	///	</summary>
	public int base_ani_id => _base_ani_id;
	int _base_ani_id;

	///	<summary>
	///	interaction_group_id
	///	</summary>
	public int interaction_group_id => _interaction_group_id;
	int _interaction_group_id;

	private bool disposed = false;

	public L2d_Skin_Ani_State_Data(Raw_L2d_Skin_Ani_State_Data raw_data)
	{
		_state_id = raw_data.state_id;
		_base_ani_id = raw_data.base_ani_id;
		_interaction_group_id = raw_data.interaction_group_id;
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
		sb.AppendFormat("[state_id] = <color=yellow>{0}</color>", state_id).AppendLine();
		sb.AppendFormat("[base_ani_id] = <color=yellow>{0}</color>", base_ani_id).AppendLine();
		sb.AppendFormat("[interaction_group_id] = <color=yellow>{0}</color>", interaction_group_id).AppendLine();
		return sb.ToString();
	}
}

