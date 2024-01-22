using FluffyDuck.Util;
using System.Linq;

public class Reward_Set_Data : System.IDisposable
{
	///	<summary>
	///	아이템 ID
	///	</summary>
	public int reward_id => _reward_id;
	int _reward_id;

	///	<summary>
	///	보상 타입
	///	</summary>
	public REWARD_TYPE reward_type => _reward_type;
	REWARD_TYPE _reward_type;

	///	<summary>
	///	변수1
	///	</summary>
	public int var1 => _var1;
	int _var1;

	///	<summary>
	///	변수2
	///	</summary>
	public int var2 => _var2;
	int _var2;

	///	<summary>
	///	출현 타입
	///	0: drop_per 칼럼을 해당 행의 보상 출현 성공률로 사용
	///	1: drop_per 칼럼을 같은 reward_id 내에서 출현 비중으로 사용
	///	</summary>
	public int drop_type => _drop_type;
	int _drop_type;

	///	<summary>
	///	출현확률
	///	</summary>
	public int drop_per => _drop_per;
	int _drop_per;

	///	<summary>
	///	노출 여부
	///	</summary>
	public bool is_use => _is_use;
	bool _is_use;

	///	<summary>
	///	보상 노출 순서
	///	</summary>
	public int sort_order => _sort_order;
	int _sort_order;

	private bool disposed = false;

	public Reward_Set_Data(Raw_Reward_Set_Data raw_data)
	{
		_reward_id = raw_data.reward_id;
		_reward_type = raw_data.reward_type;
		_var1 = raw_data.var1;
		_var2 = raw_data.var2;
		_drop_type = raw_data.drop_type;
		_drop_per = raw_data.drop_per;
		_is_use = raw_data.is_use;
		_sort_order = raw_data.sort_order;
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
		sb.AppendFormat("[reward_type] = <color=yellow>{0}</color>", reward_type).AppendLine();
		sb.AppendFormat("[var1] = <color=yellow>{0}</color>", var1).AppendLine();
		sb.AppendFormat("[var2] = <color=yellow>{0}</color>", var2).AppendLine();
		sb.AppendFormat("[drop_type] = <color=yellow>{0}</color>", drop_type).AppendLine();
		sb.AppendFormat("[drop_per] = <color=yellow>{0}</color>", drop_per).AppendLine();
		sb.AppendFormat("[is_use] = <color=yellow>{0}</color>", is_use).AppendLine();
		sb.AppendFormat("[sort_order] = <color=yellow>{0}</color>", sort_order).AppendLine();
		return sb.ToString();
	}
}

