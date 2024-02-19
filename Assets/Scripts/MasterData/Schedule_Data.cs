#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Schedule_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	스케쥴 아이디
	///	</summary>
	public int schedule_id => _schedule_id;
	int _schedule_id;

	///	<summary>
	///	스케쥴 시작
	///	</summary>
	public string date_start => _date_start;
	string _date_start;

	///	<summary>
	///	스케쥴 종료
	///	</summary>
	public string date_end => _date_end;
	string _date_end;

	///	<summary>
	///	스케쥴 일일 오픈 시작 시간
	///	</summary>
	public string time_open => _time_open;
	string _time_open;

	///	<summary>
	///	스케쥴 일일 오픈 종료 시간
	///	</summary>
	public string time_close => _time_close;
	string _time_close;

	///	<summary>
	///	요일 오픈 체크
	///	c#의 DayOfWeek 변수를 사용하도록 하겠습니다.
	///	복수의 날짜 사용이 가능하니, 배열로 지정 요일을 입력하는 방식으로 하겠습니다.
	///	직관적으로 표시하기 위한 방법으로 보여집니다.
	///	0 : Sunday
	///	1 : Monday
	///	2 : Tuesday
	///	3 : Wednesday
	///	4 : Thursday
	///	5 : Friday
	///	6 : Saturday
	///	</summary>
	public int[] day_of_weeks => _day_of_weeks;
	int[] _day_of_weeks;

	private bool disposed = false;

	public Schedule_Data(Raw_Schedule_Data raw_data)
	{
		_schedule_id = raw_data.schedule_id;
		_date_start = raw_data.date_start;
		_date_end = raw_data.date_end;
		_time_open = raw_data.time_open;
		_time_close = raw_data.time_close;
		if(raw_data.day_of_weeks != null)
			_day_of_weeks = raw_data.day_of_weeks.ToArray();
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
		sb.AppendFormat("[schedule_id] = <color=yellow>{0}</color>", schedule_id).AppendLine();
		sb.AppendFormat("[date_start] = <color=yellow>{0}</color>", date_start).AppendLine();
		sb.AppendFormat("[date_end] = <color=yellow>{0}</color>", date_end).AppendLine();
		sb.AppendFormat("[time_open] = <color=yellow>{0}</color>", time_open).AppendLine();
		sb.AppendFormat("[time_close] = <color=yellow>{0}</color>", time_close).AppendLine();
		sb.AppendLine("[day_of_weeks]");
		if(day_of_weeks != null)
		{
			cnt = day_of_weeks.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", day_of_weeks[i]).AppendLine();
			}
		}

		return sb.ToString();
	}
}

