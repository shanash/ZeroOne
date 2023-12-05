using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Json
{
    class EnumInfo
    {
        public string type_name;
        public int value;
        public string comment;
    }

    class EnumData
    {
        public string enum_name;
        public string enum_comment;
        public List<EnumInfo> enums;
        public EnumData()
        {
            enums = new List<EnumInfo>();
        }
    }

    enum ENUM_ROW_TYPE_INDEX
    {
        COMMENT = 2,
        NAME = 3,
        DATA_START = 4
    }

    enum ENUM_COL_TYPE_INDEX
    {
        TYPE_DEFINE = 1,
        VALUE = 2,
        COMMENT = 3
    }

    internal class ConvertEnumSheet
    {
        /// <summary>
        /// 시트의 enum 정보를 컨버팅
        /// 이곳에서는 파일 저장은 하지 않는다.
        /// 각 enum의 선언 변수 및 값, 주석 정보만 담아서 반환한다.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="master_enum_infos"></param>
        public static void ConvertSheet(IXLWorksheet ws, ref List<EnumData> result_enum_list)
        {
            try
            {
                // enum 이름과 설명 가져오기
                string enum_name = GetEnumName(ws);
                string enum_comment = GetEnumComment(ws);

                Console.WriteLine($"Enum Sheet Name => [{ws.Name}], Enum Name => [{enum_name}], Comment => [{enum_comment}]");

                // type, value, comment 가져오기
                var enum_data = GetSheetEnumData(ws);
                enum_data.enum_name = enum_name;
                enum_data.enum_comment = enum_comment;

                result_enum_list.Add(enum_data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        static EnumData GetSheetEnumData(IXLWorksheet ws)
        {
            EnumData enum_data = new EnumData();

            int rows = ws.RangeUsed().RowCount();
            int cols = GetColumnCount(ws);

            for (int r = (int)ENUM_ROW_TYPE_INDEX.DATA_START; r <= rows; r++)
            {
                EnumInfo info = new EnumInfo();

                // [1] enum var define
                var typeCell = ws.Cell(r, (int)ENUM_COL_TYPE_INDEX.TYPE_DEFINE);
                if (!typeCell.IsEmpty())
                {
                    info.type_name = typeCell.GetValue<string>();
                }

                // [2] value
                var valueCell = ws.Cell(r, (int)ENUM_COL_TYPE_INDEX.VALUE);
                if (!valueCell.IsEmpty())
                {
                    if (valueCell.TryGetValue(out int intValue))
                    {
                        info.value = intValue;
                    }
                    else if (valueCell.TryGetValue(out string strVal))
                    {
                        info.value = Int32.Parse(strVal);
                    }
                }

                // [3] comment
                var commentCell = ws.Cell(r, (int)ENUM_COL_TYPE_INDEX.COMMENT);
                if (!commentCell.IsEmpty())
                {
                    info.comment = commentCell.GetValue<string>();
                }

                enum_data.enums.Add(info);
            }

            return enum_data;
        }

        /// <summary>
        /// 컬럼의 수를 조회
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        static int GetColumnCount(IXLWorksheet ws)
        {
            var firstRow = ws.FirstRowUsed();
            return firstRow.CellsUsed().Count();
        }


        static string GetEnumComment(IXLWorksheet ws)
        {
            var commentCell = ws.Cell((int)ENUM_ROW_TYPE_INDEX.COMMENT, 1);
            if (commentCell.IsEmpty())
            {
                Console.WriteLine("Comment is required in column (2,1).");
                return string.Empty;
            }
            return commentCell.GetValue<string>();
        }

        /// <summary>
        /// enum의 이름 가져오기
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        static string GetEnumName(IXLWorksheet ws)
        {
            var nameCell = ws.Cell(1, 1);
            if (nameCell.IsEmpty())
            {
                Console.WriteLine("Enum name is required in column (1,1).");
                return string.Empty;
            }
            return nameCell.GetValue<string>();
        }
    }
}
