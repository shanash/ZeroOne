#if UNITY_EDITOR
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FluffyDuck.EditorUtil
{
    public class DeviceInfo
    {
        public string ID { get; set; }
        public string Model
        {
            get { return _Model; }
            set
            {
                _Model = value.Replace('-', ' ');
            }
        }
        string _Model = string.Empty;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set
            {
                _Manufacturer = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            }
        }
        string _Manufacturer = string.Empty;

        public override string ToString()
        {
            return $"{Manufacturer} {Model} ({ID})";
        }
    }

    public static class DeviceInfoExtensions
    {
        public static string[] ConvertToPopupArray(this List<DeviceInfo> deviceInfos, string special_string)
        {
            // LINQ를 사용하여 ID 필드 추출
            string[] ids = deviceInfos.Select(device => device.ToString()).ToArray();

            // 새 배열 생성: 특정 문자열을 첫 번째 요소로, 그 뒤에 기존 ID 값들을 추가
            string[] updatedIds = new string[ids.Length + 1];
            updatedIds[0] = special_string;
            ids.CopyTo(updatedIds, 1);

            return updatedIds;
        }
    }
}
#endif
