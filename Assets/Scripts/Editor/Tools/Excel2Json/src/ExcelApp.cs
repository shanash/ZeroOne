using System.Text;
using ClosedXML.Excel;
#if UNITY_5_3_OR_NEWER
using System;
using System.IO;
using System.Collections.Generic;
#endif

namespace Excel2Json
{
    class ExcelApp
    {
        static bool USE_ADDRESSABLE_ASSETS = true;

        public static void ReadExcelData(string path, string output_dir, bool is_csharp_make, string csharp_output_dir, bool is_encrypt, string enc_password, bool use_raw, ref Dictionary<string, List<ColumnInfo>> master_table_columns, ref List<EnumData> result_enum_list)
        {
            string tempPath = Path.GetTempFileName(); // 임시 파일 경로 생성

            try
            {
                File.Copy(path, tempPath, true); // 원본 파일을 임시 파일로 복사

                using (FileStream fileStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
                {
                    // 나머지 작업을 fileStream을 사용하여 수행
                    using (var workbook = new XLWorkbook(fileStream))
                    {
                        foreach (IXLWorksheet ws in workbook.Worksheets)
                        {
                            // 참조 시트는 건너뛴다
                            if (ws.Name.Contains("!"))
                            {
                                continue;
                            }

                            if (ws.Name.Equals("reward_set"))
                            {
                                bool a = false;
                            }

                            // ENUM 시트와 데이터 시트로 구분 필요
                            if (ws.Name.Contains("@"))
                            {
                                ConvertEnumSheet.ConvertSheet(ws, ref result_enum_list);
                            }
                            else
                            {
                                ConvertDataSheet.ConvertSheet(ws, output_dir, is_csharp_make, csharp_output_dir, is_encrypt, enc_password, ref master_table_columns, use_raw);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Ex : {e}");
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath); // 임시 파일 삭제
                }
            }
        }

        /// <summary>
        /// 워크북, 앱 사용 종료
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception e)
            {
                obj = null;
                throw e;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void MakeMasterDefine(List<EnumData> master_enum_infos, string output_dir)
        {
            StringBuilder sb = new StringBuilder();
            const string master_define_name = "MasterDefine";

            int cnt = master_enum_infos.Count;
            for (int i = 0; i < cnt; i++)
            {
                var enum_data = master_enum_infos[i];

                //  enum 주석
                if (!string.IsNullOrEmpty(enum_data.enum_comment))
                {
                    sb.AppendLine("///\t<summary>");

                    string[] comments = enum_data.enum_comment.Split('\n');
                    for (int c = 0; c < comments.Length; c++)
                    {
                        sb.AppendLine($"///\t{comments[c]}");
                    }

                    sb.AppendLine("///\t</summary>");
                }

                //  enum 선언
                sb.AppendLine($"public enum {enum_data.enum_name}");
                sb.AppendLine("{");

                int ecnt = enum_data.enums.Count;
                for (int e = 0; e < ecnt; e++)
                {
                    //  각 변수 및 값, 주석 
                    var info = enum_data.enums[e];
                    if (string.IsNullOrEmpty(info.comment))
                    {
                        sb.AppendLine($"\t{info.type_name} = {info.value},");
                    }
                    else
                    {
                        string[] var_comments = info.comment.Split('\n');
                        if (var_comments.Length > 1)
                        {
                            sb.AppendLine("\t///\t<summary>");
                            for (int c = 0; c < var_comments.Length; c++)
                            {
                                sb.AppendLine($"\t///\t{var_comments[c]}");
                            }
                            sb.AppendLine("\t///\t</summary>");
                        }
                        else
                        {
                            sb.AppendLine($"\t/// <summary>{info.comment}</summary>");
                        }
                        sb.AppendLine($"\t{info.type_name} = {info.value},");
                    }
                }

                sb.AppendLine("}").AppendLine();
            }


            string filename = string.Format("{0}.cs", master_define_name);
            string path = Path.Combine(output_dir, filename);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 마스터 데이터를 읽어들이는 BaseMasterDataManager 를 생성
        /// </summary>
        /// <param name="master_table_columns"></param>
        /// <param name="output_dir"></param>
        /// <param name="is_encrypt"></param>
        public static void MakeLoadBaseMasterData(Dictionary<string, List<ColumnInfo>> master_table_columns, string output_dir, bool is_encrypt, bool use_raw)
        {
            StringBuilder sb = new StringBuilder();
            const string mng_name = "BaseMasterDataManager";

            //  include package
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("#if UNITY_5_3_OR_NEWER");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("#endif");
            if (USE_ADDRESSABLE_ASSETS)
            {
                if (is_encrypt)
                {
                    sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                    sb.AppendLine("using FluffyDuck.Util;");
                    sb.AppendLine("#endif");
                }
                sb.AppendLine("using System.Threading.Tasks;");
                sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                sb.AppendLine("using UnityEngine.AddressableAssets;");
                sb.AppendLine("#endif");
                sb.AppendLine("#nullable disable");
            }
            else
            {
                if (is_encrypt)
                {
                    sb.AppendLine("using ForestJ.Util;");
                }
            }
            sb.AppendLine("using Newtonsoft.Json;");
            if (!use_raw)
            {
                sb.AppendLine("using System.Linq;");
            }
            sb.AppendLine();

            //  class declare
            sb.AppendLine($"public class {mng_name}");
            sb.AppendLine("{");
            sb.AppendLine();

            //  declare variables
            foreach (var table in master_table_columns)
            {
                //  define var
                sb.AppendLine($"\tprotected List<{table.Key}> _{table.Key}");
                sb.AppendLine("\t{");
                sb.AppendLine("\t\tget;");
                sb.AppendLine("\t\tprivate set;");
                sb.AppendLine("\t}");  //  var close
            }
            sb.AppendLine();
            sb.AppendLine();


            //  init load vars
            sb.AppendLine("\tprotected bool is_init_load = false;").AppendLine();


            //  construct
            sb.AppendLine($"\tprotected {mng_name}()");
            sb.AppendLine("\t{");                                  //  construct begin
            sb.AppendLine("\t\tif(!is_init_load)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tInitLoadMasterData();");
            //sb.AppendLine("\t\t\tis_init_load = true;");
            sb.AppendLine("\t\t}");                                //  construct end
    

            sb.AppendLine("\t}");
            sb.AppendLine().AppendLine();

            //  InitLoadMasterData();
            sb.AppendLine("\tprivate async void InitLoadMasterData()");
            sb.AppendLine("\t{");          //  InitLoadMasterData Begin

            foreach (var table in master_table_columns)
            {
                //  call init func
                sb.AppendLine($"\t\tawait LoadMaster_{table.Key}();");
            }
            sb.AppendLine("\t\tis_init_load = true;");
            sb.AppendLine("\t}");          //  InitLoadMasterData end
            sb.AppendLine().AppendLine();

            //  LoadJsonData
            if (!USE_ADDRESSABLE_ASSETS)
            {
                sb.AppendLine("\tstring LoadJsonData(string path)");
                sb.AppendLine("\t{");
                sb.AppendLine("\t\tvar json_text = Resources.Load(path) as TextAsset;");
                sb.AppendLine("\t\treturn json_text.text;");
                sb.AppendLine("\t}").AppendLine().AppendLine();

                //  load master func
                foreach(var table in master_table_columns)
                {
                    sb.AppendLine($"\tprotected void LoadMaster_{table.Key}()");
                    sb.AppendLine("\t{");
                    if (is_encrypt)
                    {
                        sb.AppendLine($"\t\tstring data = LoadJsonData(\"Master/{table.Key}\");");
                        sb.AppendLine("\t\tstring decrypt = ForestJ.Util.Security.AESDecrypt256(data);");
                        if (use_raw)
                        {
                            sb.AppendLine($"\t\t_{table.Key} = JsonConvert.DeserializeObject<List<{table.Key}>>(decrypt);");
                        }
                        else
                        {
                            sb.AppendLine($"\t\tvar raw_data_list = JsonConvert.DeserializeObject<List<Raw_{table.Key}>>(decrypt);");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"\t\tstring data = LoadJsonData(\"Master/{table.Key}\");");
                        if (use_raw)
                        {
                            sb.AppendLine($"\t\t_{table.Key} = JsonConvert.DeserializeObject<List<{table.Key}>>(data);");
                        }
                        else
                        {
                            sb.AppendLine($"\t\tvar raw_data_list = JsonConvert.DeserializeObject<List<Raw_{table.Key}>>(data);");
                        }
                    }

                    if (!use_raw)
                    {
                        sb.AppendLine($"\t\t_{table.Key} = raw_data_list.Select(raw_data => new {table.Key}(raw_data)).ToList();");
                    }

                    sb.AppendLine("\t}").AppendLine();
                }
                sb.AppendLine("\t}").AppendLine();
            }
            else
            {
                sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                //  LoadJsonDataAsync
                sb.AppendLine("\tasync Task<string> LoadJsonDataAsync(string path)");
                sb.AppendLine("\t{");              //  LoadJsonData begin
                sb.AppendLine("\t\tvar handle = Addressables.LoadAssetAsync<TextAsset>(path);");
                sb.AppendLine("\t\tTextAsset txt_asset = await handle.Task;");
                sb.Append("\t\treturn txt_asset.text;");
                sb.AppendLine();
                sb.AppendLine("\t}");              //  LoadJsonData end
                sb.AppendLine().AppendLine();
                sb.AppendLine("#else");
                sb.AppendLine("\tasync Task<string> LoadJsonDataAsync(string path)");
                sb.AppendLine("\t{");              //  LoadJsonData begin
                sb.AppendLine("\t\tusing (var reader = new StreamReader(path))");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\treturn await reader.ReadToEndAsync();");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");              //  LoadJsonData end
                sb.AppendLine().AppendLine();
                sb.AppendLine("#endif");

                //  load master funcs
                foreach (var table in master_table_columns)
                {
                    sb.AppendLine($"\tprotected async Task LoadMaster_{table.Key}()");
                    sb.AppendLine("\t{");
                    if (is_encrypt)
                    {
                        sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                        sb.AppendLine($"\t\tstring json = await LoadJsonDataAsync(\"Assets/AssetResources/Master/{table.Key}\");");
                        sb.AppendLine("#else");
                        sb.AppendLine($"\t\tstring json = await LoadJsonDataAsync(\"../Master/{table.Key}.json\");");
                        sb.AppendLine("#endif");

                        sb.AppendLine("\t\tstring decrypt = FluffyDuck.Util.Security.AESDecrypt256(json);");
                        if (use_raw)
                        {
                            sb.AppendLine($"\t\t_{table.Key} = JsonConvert.DeserializeObject<List<{table.Key}>>(decrypt);");
                        }
                        else
                        {
                            sb.AppendLine($"\t\tvar raw_data_list = JsonConvert.DeserializeObject<List<Raw_{table.Key}>>(decrypt);");
                        }
                    }
                    else
                    {
                        sb.AppendLine("#if UNITY_5_3_OR_NEWER");
                        sb.AppendLine($"\t\tstring json = await LoadJsonDataAsync(\"Assets/AssetResources/Master/{table.Key}\");");
                        sb.AppendLine("#else");
                        sb.AppendLine($"\t\tstring json = await LoadJsonDataAsync(\"../Master/{table.Key}.json\");");
                        sb.AppendLine("#endif");
                        if (use_raw)
                        {
                            sb.AppendLine($"\t\t_{table.Key} = JsonConvert.DeserializeObject<List<{table.Key}>>(json);");
                        }
                        else
                        {
                            sb.AppendLine($"\t\tvar raw_data_list = JsonConvert.DeserializeObject<List<Raw_{table.Key}>>(json);");
                        }
                    }

                    if (!use_raw)
                    {
                        sb.AppendLine($"\t\t_{table.Key} = raw_data_list.Select(raw_data => new {table.Key}(raw_data)).ToList();");
                    }
                    sb.AppendLine("\t}");
                    sb.AppendLine();
                }

                //  check master load funcs
                foreach (var table in master_table_columns)
                {
                    sb.AppendLine($"\tprotected async void Check_{table.Key}()");
                    sb.AppendLine("\t{");

                    sb.AppendLine($"\t\tif(_{table.Key} == null)");
                    sb.AppendLine("\t\t{");
                    sb.AppendLine($"\t\t\tawait LoadMaster_{table.Key}();");
                    sb.AppendLine("\t\t}");

                    sb.AppendLine("\t}");
                    sb.AppendLine();
                }

                //  class declare end
                sb.AppendLine("}");

            }

            string filename = string.Format("{0}.cs", mng_name);
            string path = Path.Combine(output_dir, filename);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
