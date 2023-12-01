using System.Collections.Generic;


namespace FluffyDuck.Util
{
    public class KoreanUtils
    {
        static List<char> firstElemList = new List<char>() { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        static List<char> middleElemList = new List<char>() { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
        static List<char> lastElemList = new List<char>() { ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        public const int DIV = 588;

        const int FIRST_ELEM_NUM = (int)'가';

        /// <summary>
        /// 초성 정보 반환
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char GetFirstElem(int c)
        {
            int r = c - FIRST_ELEM_NUM;
            r = r / DIV;
            if (r < firstElemList.Count)
            {
                return firstElemList[r];
            }
            return ' ';
        }
        /// <summary>
        /// 중성 정보 반환
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char GetMiddleElem(int c)
        {
            int r = c - FIRST_ELEM_NUM;
            r = (r % DIV) / 28;
            if (r < middleElemList.Count)
            {
                return middleElemList[r];
            }
            return ' ';
        }
        /// <summary>
        /// 종성 정보 반환
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char GetLastElem(int c)
        {
            int r = c - FIRST_ELEM_NUM;
            r = (r % DIV) % 28;
            if (r < lastElemList.Count)
            {
                return lastElemList[r];
            }
            return ' ';
        }
        /// <summary>
        /// 종성이 있는지 여부 체크
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsLastElemExist(string txt)
        {
            int txt_int = (int)txt[txt.Length - 1];
            char result_char = GetLastElem(txt_int);
            return result_char != ' ';
        }
    }

}
