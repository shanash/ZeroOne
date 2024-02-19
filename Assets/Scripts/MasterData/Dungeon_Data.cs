#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Dungeon_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	던전 인덱스
	///	</summary>
	public int dungeon_id => _dungeon_id;
	int _dungeon_id;

	///	<summary>
	///	던전 타입
	///	</summary>
	public GAME_TYPE game_type => _game_type;
	GAME_TYPE _game_type;

	///	<summary>
	///	던전 그룹 ID
	///	</summary>
	public int dungeon_group_id => _dungeon_group_id;
	int _dungeon_group_id;

	///	<summary>
	///	스케쥴 ID
	///	</summary>
	public int schedule_id => _schedule_id;
	int _schedule_id;

	///	<summary>
	///	입장 제한 횟수
	///	</summary>
	public int entrance_limit_count => _entrance_limit_count;
	int _entrance_limit_count;

	///	<summary>
	///	던전 오픈 조건 타입
	///	</summary>
	public GAME_TYPE open_game_type => _open_game_type;
	GAME_TYPE _open_game_type;

	///	<summary>
	///	오픈 던전 완료 ID
	///	</summary>
	public int open_dungeon_id => _open_dungeon_id;
	int _open_dungeon_id;

	private bool disposed = false;

	public Dungeon_Data(Raw_Dungeon_Data raw_data)
	{
		_dungeon_id = raw_data.dungeon_id;
		_game_type = raw_data.game_type;
		_dungeon_group_id = raw_data.dungeon_group_id;
		_schedule_id = raw_data.schedule_id;
		_entrance_limit_count = raw_data.entrance_limit_count;
		_open_game_type = raw_data.open_game_type;
		_open_dungeon_id = raw_data.open_dungeon_id;
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
		sb.AppendFormat("[dungeon_id] = <color=yellow>{0}</color>", dungeon_id).AppendLine();
		sb.AppendFormat("[game_type] = <color=yellow>{0}</color>", game_type).AppendLine();
		sb.AppendFormat("[dungeon_group_id] = <color=yellow>{0}</color>", dungeon_group_id).AppendLine();
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[entrance_limit_count] = <color=yellow>{0}</color>", entrance_limit_count).AppendLine();
		sb.AppendFormat("[open_game_type] = <color=yellow>{0}</color>", open_game_type).AppendLine();
		sb.AppendFormat("[open_dungeon_id] = <color=yellow>{0}</color>", open_dungeon_id).AppendLine();
		return sb.ToString();
	}
}

