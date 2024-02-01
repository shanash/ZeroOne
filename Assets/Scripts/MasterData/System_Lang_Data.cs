#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class System_Lang_Data : System.IDisposable
{
	///	<summary>
	///	인덱스 ID
	///	</summary>
	public int index_id => _index_id;
	int _index_id;

	///	<summary>
	///	<b>key_1</b><br/>
	///	string ID
	///	</summary>
	public string string_id => _string_id;
	string _string_id;

	///	<summary>
	///	한국어
	///	</summary>
	public string kor => _kor;
	string _kor;

	///	<summary>
	///	영어
	///	</summary>
	public string eng => _eng;
	string _eng;

	///	<summary>
	///	일본어
	///	</summary>
	public string jpn => _jpn;
	string _jpn;

	private bool disposed = false;

	public System_Lang_Data(Raw_System_Lang_Data raw_data)
	{
		_index_id = raw_data.index_id;
		_string_id = raw_data.string_id;
		_kor = raw_data.kor;
		_eng = raw_data.eng;
		_jpn = raw_data.jpn;
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
		sb.AppendFormat("[index_id] = <color=yellow>{0}</color>", index_id).AppendLine();
		sb.AppendFormat("[string_id] = <color=yellow>{0}</color>", string_id).AppendLine();
		sb.AppendFormat("[kor] = <color=yellow>{0}</color>", kor).AppendLine();
		sb.AppendFormat("[eng] = <color=yellow>{0}</color>", eng).AppendLine();
		sb.AppendFormat("[jpn] = <color=yellow>{0}</color>", jpn).AppendLine();
		return sb.ToString();
	}
}

