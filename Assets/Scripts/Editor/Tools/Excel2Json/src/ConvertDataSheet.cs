using System.Text;
using Newtonsoft.Json.Linq;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Diagnostics;


#if UNITY_5_3_OR_NEWER
using System.Collections.Generic;
using System;
using System.IO;
#endif

namespace Excel2Json
{
    class ColumnInfo
    {
        public string hash_key = string.Empty;
        public string key;
        public string key_name;
        public string type;
        public string comment;
        public string init_value;
        public bool is_enum;        //  enum 타입 여부
        public bool is_array;       //  배열 여부
        public bool is_ref;         //  참조용 컬럼 여부. 참조용 컬럼은 c# 파일 및 json 데이터로 변환하지 않는다.
        public ColumnInfo()
        {
            is_enum = false;
            is_array = false;
            is_ref = false;
        }
    }
    enum DATA_ROW_TYPE_INDEX
    {
        NAME = 2,
        DATA_TYPE = 3,
        VAR_NAME = 4,
        DATA_START = 5
    }

    internal class ConvertDataSheet
    {
        /// <summary>
        /// 워크 시트를 json 데이터 형식으로 변경하기 위한 함수
        /// 변경 후 json 파일로 저장
        /// 조건에 따라 c# 파일도 생성
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="output_dir"></param>
        /// <param name="is_csharp_make"></param>
        /// <param name="csharp_output_dir"></param>
        /// <param name="is_encrypt"></param>
        /// <param name="enc_password"></param>
        /// <param name="master_table_columns"></param>
        public static void ConvertSheet(IXLWorksheet ws, string output_dir, bool is_csharp_make, string csharp_output_dir, bool is_encrypt, string enc_password, ref Dictionary<string, List<ColumnInfo>> master_table_columns, bool use_raw_cs_file)
        {
            try
            {
                List<ColumnInfo> col_info = new List<ColumnInfo>();
                string table_name = GetTableName(ws);

                Logger.Log($"Data Sheet Name => [{ws.Name}], Json Name => [{table_name}]");

                GetColumnInfo(ws, ref col_info);
                JArray newton_sheet_arr = GetSheetData(ws, col_info);

                string filename = Path.Combine(output_dir, table_name + ".json");
                string newton_str_json = newton_sheet_arr.ToString();

                // TODO: Json은 이후에 바이너리 파일로 저장해서 사용하는것을 고려해봅시다...
                if (is_encrypt)
                {
                    string encJson = Security.AESEncrypt256(newton_str_json, enc_password);
                    File.WriteAllText(filename, encJson, Encoding.UTF8);
                }
                else
                {
                    File.WriteAllText(filename, newton_str_json, Encoding.UTF8);
                }

                if (is_csharp_make)
                {
                    MakeCSharpFile(col_info, table_name, csharp_output_dir, use_raw_cs_file);
                    if (!use_raw_cs_file)
                    {
                        MakeCSharpRawFile(col_info, table_name, csharp_output_dir);
                    }
                    master_table_columns.Add(table_name, col_info);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 테이블 이름 가져오기
        /// </summary>
        /// <param name="ws"></param>
        static string GetTableName(IXLWorksheet ws)
        {
            var nameCell = ws.Cell(1, 1);
            if (nameCell.IsEmpty())
            {
                Logger.Log("Table name is required in column (1,1).");
                return string.Empty;
            }
            return nameCell.GetValue<string>();
        }


        static void GetColumnInfo(IXLWorksheet ws, ref List<ColumnInfo> col_info)
        {
            int cols = ws.Row((int)DATA_ROW_TYPE_INDEX.NAME).LastCellUsed().Address.ColumnNumber;

            for (int r = (int)DATA_ROW_TYPE_INDEX.NAME; r <= (int)DATA_ROW_TYPE_INDEX.VAR_NAME; r++)
            {
                for (int c = 1; c <= cols; c++)
                {
                    var cell = ws.Cell(r, c);
                    if (cell.IsEmpty())
                    {
                        break;
                    }
                    //cellValue
                    string s = cell.GetValue<string>();
                    ColumnInfo info = null;

                    if (r == (int)DATA_ROW_TYPE_INDEX.NAME)
                    {
                        info = new ColumnInfo();
                        info.key_name = s;
                        col_info.Add(info);
                    }
                    else if (r == (int)DATA_ROW_TYPE_INDEX.DATA_TYPE)
                    {
                        info = col_info[c - 1];
                        if (s.IndexOf("[]") != -1)
                        {
                            var idx = s.IndexOf("[]");
                            string var_name = s.Substring(0, idx);

                            if (var_name.ToLower().IndexOf("enum:") != -1)
                            {
                                string[] split = var_name.Split(':');
                                if (split.Length < 2)
                                {
                                    throw new Exception("Enum Type Define Error : ex) enum:ENUM_NAME:INIT_VALUE");
                                }
                                info.is_enum = true;
                                info.type = string.Format("{0}[]", split[1]);
                            }
                            else
                            {
                                info.type = s;
                            }
                            info.is_array = true;
                        }
                        else
                        {
                            int keyStartIndex = s.IndexOf("key");
                            if (keyStartIndex != -1)
                            {
                                int colonIndex = s.IndexOf(":", keyStartIndex);
                                if (colonIndex != -1)
                                {
                                    string result = s.Substring(keyStartIndex, colonIndex - keyStartIndex + 1);
                                    s = s.Replace(result, "");
                                    info.hash_key = result;
                                }
                            }

                            if (s.ToLower().IndexOf("enum:") != -1)
                            {
                                string[] split = s.Split(':');
                                if (split.Length != 3)
                                {
                                    throw new Exception("Enum Type Define Error : ex) enum:ENUM_NAME:INIT_VALUE");
                                }
                                info.is_enum = true;
                                info.type = split[1];
                                info.init_value = split[2];
                            }
                            else
                            {
                                info.type = s;
                            }
                        }
                    }
                    else if (r == (int)DATA_ROW_TYPE_INDEX.VAR_NAME)
                    {
                        info = col_info[c - 1];
                        info.key = s;
                        if (cell?.GetComment() != null)
                        {
                            info.comment = cell?.GetComment().Text ?? string.Empty;
                        }
                        //  참조 컬럼 여부
                        info.is_ref = info.key.IndexOf('#') != -1;
                    }
                }
            }
        }

        /// <summary>
        /// 시트의 데이터를 조회하여 json 형식에 대입하기
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="col_info"></param>
        /// <returns></returns>
        static JArray GetSheetData(IXLWorksheet ws, List<ColumnInfo> col_info)
        {
            JArray sheetData = new JArray();

            int rows = ws.LastRowUsed().RowNumber();

            for (int r = (int)DATA_ROW_TYPE_INDEX.DATA_START; r <= rows; r++)
            {
                JObject rowObject = new JObject();

                for (int c = 1; c <= col_info.Count; c++)
                {
                    var cell = ws.Cell(r, c);
                    if (cell.IsEmpty())
                    {
                        continue;
                    }

                    ColumnInfo info = col_info[c - 1];

                    // 참조 컬럼은 건너뛴다
                    if (info.is_ref)
                    {
                        continue;
                    }

                    // 데이터 형식에 따라 처리
                    if (info.is_array)
                    {
                        // 배열 처리
                        var arrayData = ProcessArrayCell(cell, info);
                        if (arrayData != null)
                        {
                            rowObject[info.key] = arrayData;
                        }
                    }
                    else
                    {
                        // 단일 값 처리
                        var cellValue = ProcessSingleCell(cell, info);
                        if (cellValue != null)
                        {
                            rowObject[info.key] = cellValue;
                        }
                    }
                }
                sheetData.Add(rowObject);
            }

            return sheetData;
        }

        static T GetValue<T>(IXLCell cell)
        {
            object result = default;
            if (cell.CachedValue.IsNumber)
            {
                result = Convert.ChangeType(cell.CachedValue.GetNumber(), typeof(T));
            }
            else if (cell.CachedValue.IsBoolean)
            {
                result = Convert.ChangeType(cell.CachedValue.GetBoolean(), typeof(T));
            }
            else if (cell.CachedValue.IsText)
            {
                result = Convert.ChangeType(cell.CachedValue.GetText(), typeof(T));
            }

            return (T)result;
        }

        static JToken ProcessSingleCell(IXLCell cell, ColumnInfo info)
        {
            try
            {
                // 타입에 따라 셀 값을 처리
                switch (info.type.Replace("*", "").ToLower())
                {
                    case "int":
                        return JToken.FromObject(GetValue<int>(cell));
                    case "bool":
                        return JToken.FromObject(GetValue<bool>(cell));
                    case "string":
                        return JToken.FromObject(GetValue<string>(cell));
                    case "double":
                        return JToken.FromObject(GetValue<double>(cell));
                    // 기타 데이터 타입에 대한 처리 필요
                    default:
                        if (info.is_enum)
                        {
                            return JToken.FromObject(GetValue<int>(cell));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            Logger.Assert(false, "Unknown type");
            return null;
        }

        static JArray ProcessArrayCell(IXLCell cell, ColumnInfo info)
        {
            var cellValue = GetValue<string>(cell);
            cellValue = Regex.Replace(cellValue, @"[\[\]]", "");
            string[] elements = cellValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            JArray array = new JArray();

            foreach (string element in elements)
            {
                if (string.IsNullOrEmpty(element))
                {
                    continue;
                }
                switch (info.type.Replace("*", "").ToLower())
                {
                    case "int[]":
                        array.Add(JToken.FromObject(int.Parse(element.Trim())));
                        break;
                    case "bool[]":
                        array.Add(JToken.FromObject(bool.Parse(element.Trim())));
                        break;
                    case "string[]":
                        array.Add(JToken.FromObject(element.Trim()));
                        break;
                    case "double[]":
                        array.Add(JToken.FromObject(double.Parse(element.Trim())));
                        break;
                    // 기타 배열 데이터 타입에 대한 처리 필요
                    default:
                        if (info.is_enum)
                        {
                            array.Add(JToken.FromObject(int.Parse(element.Trim())));
                            break;
                        }
                        Logger.Assert(false, "Unknown array type");
                        break;
                }
            }

            return array;
        }



        /// <summary>
        /// C# 파일 생성
        /// </summary>
        /// <param name="col_info_list"></param>
        /// <param name="data_table_name"></param>
        /// <param name="output_dir"></param>
        static void MakeCSharpFile(List<ColumnInfo> col_info_list, string data_table_name, string output_dir, bool use_raw)
        {
            StringBuilder sb = new StringBuilder();

            // using namespace
            if (!use_raw)
            {
                // 서버는 FluffyDuck.Util 사용하지 않음
                sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                sb.AppendLine("using FluffyDuck.Util;");
                sb.AppendLine("#endif");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("#nullable disable");
                sb.AppendLine();
            }

            //  class declare
            if (use_raw)
            {
                sb.AppendLine("[System.Serializable]");
            }
            sb.AppendLine($"public class {data_table_name} : System.IDisposable");
            sb.AppendLine("{");

            string get_set_str = "{get; set;}";
            //  declare variables
            for (int i = 0; i < col_info_list.Count; i++)
            {
                ColumnInfo info = col_info_list[i];
                bool is_secure_var = info.type.Contains('*');
                string origin_type = info.type.Replace("*", "");
                string base_type = origin_type.Replace("[]", "");

                if (info.is_ref)
                {
                    continue;
                }
                sb.AppendLine("\t///\t<summary>");
                if (!string.IsNullOrEmpty(info.hash_key))
                {
                    sb.AppendLine($"\t///\t<b>{info.hash_key.Replace(":", "")}</b><br/>");
                }
                if (!string.IsNullOrEmpty(info.key_name))
                {
                    string[] knames = info.key_name.Trim().Split('\n');
                    for (int k = 0; k < knames.Length; k++)
                    {
                        sb.AppendLine($"\t///\t{knames[k]}");
                    }
                }

                //sb.AppendLine($"\t///\t[{info.key_name}]");
                if (info.comment != null)
                {
                    string[] comments = info.comment.Trim().Split('\n');
                    for (int n = 0; n < comments.Length; n++)
                    {
                        if (string.IsNullOrEmpty(comments[n]))
                        {
                            continue;
                        }
                        sb.AppendLine($"\t///\t{comments[n]}");
                    }
                }

                sb.AppendLine("\t///\t</summary>");

                if (use_raw)
                {
                    sb.AppendLine($"\tpublic {origin_type} {info.key} {get_set_str}");
                }
                else
                {
                    if (!info.is_array)
                    {
                        sb.AppendLine($"\tpublic {base_type} {info.key} => _{info.key}{(is_secure_var ? ".Get()" : "")};");
                    }
                    else
                    {
                        sb.AppendLine($"\tpublic {base_type}[] {info.key} => _{info.key}{(is_secure_var ? ".Select(item => item.Get()).ToArray()" : "")};");
                    }

                    if (is_secure_var)
                    {
                        sb.AppendLine($"\tSecureVar<{base_type}>{(info.is_array ? "[]" : "")} _{info.key};");
                    }
                    else
                    {
                        sb.AppendLine($"\t{origin_type} _{info.key};");
                    }
                }
                sb.AppendLine();
            }
            sb.AppendLine("\tprivate bool disposed = false;");
            sb.AppendLine();

            //  constructer
            if (use_raw)
            {
                sb.AppendLine($"\tpublic {data_table_name}()");
            }
            else
            {
                sb.AppendLine($"\tpublic {data_table_name}(Raw_{data_table_name} raw_data)");
            }

            sb.AppendLine("\t{");


            for (int i = 0; i < col_info_list.Count; i++)
            {
                ColumnInfo info = col_info_list[i];
                bool is_secure_var = info.type.Contains('*');
                string origin_type = info.type.Replace("*", "");
                string base_type = info.type.Replace("[]", "");

                if (info.is_ref)
                {
                    continue;
                }

                if (use_raw)
                {
                    sb.AppendLine($"\t\t{info.key} = default;");
                }
                else if (origin_type == "int" || origin_type == "double")
                {
                    sb.AppendLine(is_secure_var
                        ? $"\t\t_{info.key} = new SecureVar<{origin_type}>(raw_data.{info.key});"
                        : $"\t\t_{info.key} = raw_data.{info.key};");
                }
                else if (origin_type == "bool" || origin_type == "string")
                {
                    sb.AppendLine($"\t\t_{info.key} = raw_data.{info.key};");
                }
                else if (info.is_enum)
                {
                    if (!info.is_array)
                    {
                        sb.AppendLine($"\t\t_{info.key} = raw_data.{info.key};");
                    }
                }

                if (!use_raw && info.is_array)
                {
                    if (IsRecreatableSecureVar(info.type))
                    {
                        sb.AppendLine($"\t\t_{info.key} = SecureVar<{base_type}>.CreateSecureArray(raw_data.{info.key});");
                    }
                    else
                    {
                        sb.AppendLine($"\t\tif(raw_data.{info.key} != null)");
                        sb.AppendLine($"\t\t\t_{info.key} = raw_data.{info.key}.ToArray();");
                    }
                }
            }

            sb.AppendLine("\t}").AppendLine();

            //  Dispose()
            sb.AppendLine("\tpublic void Dispose()");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tDispose(true);");
            sb.AppendLine("\t\tSystem.GC.SuppressFinalize(this);");
            sb.AppendLine("\t}");

            //  Dispose(bool disposing) - begin
            sb.AppendLine("\tprotected virtual void Dispose(bool disposing)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tif (!disposed)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif (disposing)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\t// todo dispose resouces");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tdisposed = true;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");       //  Dispose(bool disposing) - finish


            //  ToString()
            sb.AppendLine("\tpublic override string ToString()");
            sb.AppendLine("\t{");
            if (col_info_list.Exists(x => x.is_array))
            {
                sb.AppendLine("\t\tint cnt = 0;");
            }

            sb.AppendLine("\t\tSystem.Text.StringBuilder sb = new System.Text.StringBuilder();");
            for (int i = 0; i < col_info_list.Count; i++)
            {
                ColumnInfo info = col_info_list[i];
                if (info.is_ref)
                {
                    continue;
                }
                if (!info.is_array)
                {
                    sb.Append("\t\t").Append("sb.AppendFormat(").AppendFormat("\"[{0}] = ", info.key).Append("<color=yellow>{0}</color>\", ").Append(info.key).Append(").AppendLine();").AppendLine();
                }
                else
                {
                    sb.AppendLine($"\t\tsb.AppendLine(\"[{info.key}]\");");
                    sb.AppendLine($"\t\tif({info.key} != null)");
                    sb.AppendLine("\t\t{");
                    sb.AppendLine($"\t\t\tcnt = {info.key}.Length;");
                    sb.AppendLine("\t\t\tfor(int i = 0; i< cnt; i++)");
                    sb.AppendLine("\t\t\t{");
                    sb.AppendLine($"\t\t\t\tsb.Append(\"\\t\").AppendFormat(\"<color=yellow>{{0}}</color>\", {info.key}[i]).AppendLine();");
                    sb.AppendLine("\t\t\t}");
                    sb.AppendLine("\t\t}").AppendLine();
                }
            }
            
            sb.AppendLine("\t\treturn sb.ToString();");
            sb.AppendLine("\t}");

            sb.AppendLine("}").AppendLine();    //  class end

            string filename = string.Format("{0}.cs", data_table_name);
            string path = Path.Combine(output_dir, filename);
            //File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// C# 파일 생성
        /// </summary>
        /// <param name="col_info_list"></param>
        /// <param name="data_table_name"></param>
        /// <param name="output_dir"></param>
        static void MakeCSharpRawFile(List<ColumnInfo> col_info_list, string data_table_name, string output_dir)
        {
            StringBuilder sb = new StringBuilder();

            //  include package
            sb.AppendLine("#nullable disable");
            sb.AppendLine().AppendLine();

            sb.AppendLine("[System.Serializable]");

            //  class declare
            sb.AppendLine($"public class Raw_{data_table_name} : System.IDisposable");
            sb.AppendLine("{");

            string get_set_str = "{get; set;}";
            //  declare variables
            for (int i = 0; i < col_info_list.Count; i++)
            {
                ColumnInfo info = col_info_list[i];
                if (info.is_ref)
                {
                    continue;
                }

                string origin_type = info.type.Replace("*", "");

                sb.AppendLine($"\tpublic {origin_type} {info.key} {get_set_str}");
            }
            sb.AppendLine();
            sb.AppendLine("\tprivate bool disposed = false;").AppendLine();

            //  constructer
            sb.AppendLine($"\tpublic Raw_{data_table_name}()");
            sb.AppendLine("\t{");

            for (int i = 0; i < col_info_list.Count; i++)
            {
                ColumnInfo info = col_info_list[i];
                string origin_type = info.type.Replace("*", "");
                if (info.is_ref)
                {
                    continue;
                }
                if (origin_type == "int")
                {
                    sb.AppendLine($"\t\t{info.key} = 0;");
                }
                else if (origin_type == "string")
                {
                    sb.AppendLine($"\t\t{info.key} = string.Empty;");
                }
                else if (origin_type == "bool")
                {
                    sb.AppendLine($"\t\t{info.key} = false;");
                }
                else if (info.is_enum)
                {
                    if (!info.is_array)
                    {
                        sb.AppendLine($"\t\t{info.key} = {info.type}.{info.init_value};");
                    }
                }
            }

            sb.AppendLine("\t}").AppendLine();

            //  Dispose()
            sb.AppendLine("\tpublic void Dispose()");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tDispose(true);");
            sb.AppendLine("\t\tSystem.GC.SuppressFinalize(this);");
            sb.AppendLine("\t}");

            //  Dispose(bool disposing) - begin
            sb.AppendLine("\tprotected virtual void Dispose(bool disposing)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tif (!disposed)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif (disposing)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\t// todo dispose resouces");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tdisposed = true;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");       //  Dispose(bool disposing) - finish

            sb.AppendLine("}").AppendLine();    //  class end

            string filename = string.Format("{0}.cs", $"Raw_{data_table_name}");
            string path = Path.Combine(output_dir, filename);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        static bool IsRecreatableSecureVar(string type)
        {
            return type.Contains("*int") || type.Contains("*float") || type.Contains("*double");
        }
    }
}
