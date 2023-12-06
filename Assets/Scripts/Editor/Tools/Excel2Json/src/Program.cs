#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.IO;
#endif

namespace Excel2Json
{
    public enum PARAM_TYPE
    {
        NONE = 0,
        DIRECTORY,
        FILE,               //  사용 안함
        OUTPUT_DIR,
        CSHARP_MAKE,
        CSHARP_OUTOUT_DIR,
        ENCRYPT_ENABLE,
    }
    public static class Program
    {
        const string VERSION = "Excel2Json Ver.1.5.0";

        public static void Main(params string[] args)
        {
            if (args.Length == 0)
            {
                Logger.Log("== Please enter parameters. ==");
                Usage();
                Console.ReadKey();
                return;
            }

            PARAM_TYPE input_type = PARAM_TYPE.NONE;
            string input_path = string.Empty;

            PARAM_TYPE output_type = PARAM_TYPE.NONE;
            string output_path = string.Empty;

            PARAM_TYPE csharp_type = PARAM_TYPE.NONE;
            string csharp_output_dir = string.Empty;

            PARAM_TYPE encrypt_type = PARAM_TYPE.NONE;
            string encrypt_password = string.Empty;


            //  check help type
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg == "-h" || arg == "-help")
                {
                    UsageHelp();
                    return;
                }
                else if (arg == "-d")
                {
                    int idx = i + 1;
                    if (idx < args.Length)
                    {
                        input_path = Path.GetFullPath(args[idx]);
                        input_type = PARAM_TYPE.DIRECTORY;
                        if (!Directory.Exists(input_path))
                        {
                            Logger.Log("===== Error =====");
                            Logger.Log("The directory to change does not exist.");
                            Logger.Log(args[idx]);
                            Logger.Log(input_path);
                            Logger.Log("=================");
                            Usage();
                            Console.ReadKey();
                            return;
                        }
                    }
                }
                //else if (arg == "-f")
                //{
                //    int idx = i + 1;
                //    if (idx < args.Length)
                //    {
                //        input_path = Path.GetFullPath(args[idx]);
                //        input_type = PARAM_TYPE.FILE;
                //        if (!File.Exists(input_path))
                //        {
                //            Logger.Log("");
                //            Logger.Log("The Excel file to change does not exist.");
                //            Logger.Log("");
                //            Usage();
                //            return;
                //        }
                //    }
                //}
                else if (arg == "-o")
                {
                    int idx = i + 1;
                    if (idx < args.Length)
                    {
                        output_path = Path.GetFullPath(args[idx]);
                        output_type = PARAM_TYPE.OUTPUT_DIR;
                        if (!Directory.Exists(output_path))
                        {
                            Logger.Log("===== Error =====");
                            Logger.Log("The directory where the Json file will be saved does not exist.");
                            Logger.Log(output_path);
                            Logger.Log("=================");
                            Usage();
                            Console.ReadKey();
                            return;
                        }

                    }
                }
                else if (arg == "-cs")
                {
                    int idx = i + 1;
                    if (idx < args.Length)
                    {
                        csharp_output_dir = Path.GetFullPath(args[idx]);
                        if (Directory.Exists(csharp_output_dir))
                        {
                            csharp_type = PARAM_TYPE.CSHARP_MAKE;
                        }
                        else
                        {
                            Logger.Log("===== Error =====");
                            Logger.Log("The directory where the C#(Csharp) file will be saved does not exist.");
                            Logger.Log(csharp_output_dir);
                            Logger.Log("=================");
                            Console.ReadKey();
                            return;
                        }

                    }
                }
                else if (arg == "-pw")
                {
                    int idx = i + 1;
                    if (idx < args.Length)
                    {
                        encrypt_password = args[idx];
                        encrypt_type = PARAM_TYPE.ENCRYPT_ENABLE;
                    }
                }
            }

            if (output_type != PARAM_TYPE.OUTPUT_DIR)
            {
                Logger.Log("===== Error =====");
                Logger.Log("The directory path where the Json file will be saved is not specified.");
                Logger.Log("=================");
                Usage();
                Console.ReadKey();
                return;
            }
            //  데이터 시트들의 테이블별 리스트 정보. c# 파일을 만들기 위한 컨테이너
            Dictionary<string, List<ColumnInfo>> master_table_columns = new Dictionary<string, List<ColumnInfo>>();
            //  enum 테이블에 정의한 정보. 이 값들은 모두  MasterDefine.cs 파일에 저장한다.
            Dictionary<string, List<EnumInfo>> master_enum_rows = new Dictionary<string, List<EnumInfo>>();

            List<EnumData> master_enum_data_list = new List<EnumData>();

            bool is_csharp_make = csharp_type == PARAM_TYPE.CSHARP_MAKE;
            bool is_encrypt = encrypt_type == PARAM_TYPE.ENCRYPT_ENABLE;

            if (input_type == PARAM_TYPE.DIRECTORY)
            {
                if (Directory.Exists(input_path))
                {
                    string[] files = Directory.GetFiles(input_path);
                    Logger.Log("\n");

                    foreach (string fname in files)
                    {
                        string ext = Path.GetExtension(fname);
                        bool is_backup_file = fname.IndexOf("$") == -1 ? false : true;
                        if (ext == ".xlsx" && !is_backup_file)
                        {
                            Logger.Log($"Convert => {fname}");
                            ExcelApp.ReadExcelData(fname, output_path, is_csharp_make, csharp_output_dir, is_encrypt, encrypt_password, ref master_table_columns, ref master_enum_data_list);
                        }
                    }

                }
                else
                {
                    Logger.Log("");
                    Logger.Log(string.Format("Directory Is Not Exist!! [{0}]", args[1]));
                    Logger.Log("");
                    Console.ReadKey();
                    return;
                }
            }
            //else if (input_type == PARAM_TYPE.FILE)
            //{
            //    if (File.Exists(input_path))
            //    {
            //        ExcelApp.ReadExcelData(input_path, output_path, csharp_type == PARAM_TYPE.CSHARP_MAKE, csharp_output_dir, encrypt_type == PARAM_TYPE.ENCRYPT_ENABLE, encrypt_password, ref master_table_columns);
            //    }
            //    else
            //    {
            //        Logger.Log("");
            //        Logger.Log(string.Format("File Is Not Exist!! [{0}]", input_path));
            //        Logger.Log("");
            //        return;
            //    }
            //}
            else
            {
                Usage();
                Console.ReadKey();
                return;
            }
            
            if (is_csharp_make && master_table_columns.Count > 0)
            {
                ExcelApp.MakeLoadBaseMasterData(master_table_columns, csharp_output_dir, is_encrypt);
            }

            if (is_csharp_make && master_enum_data_list.Count > 0)
            {
                ExcelApp.MakeMasterDefine(master_enum_data_list, csharp_output_dir);
            }
            

        }

        private static void Usage()
        {
            Logger.Log("==================================================");
            Logger.Log("");
            Logger.Log(VERSION);
            Logger.Log("");
            Logger.Log("Programming by ForestJ");
            Logger.Log("");
            //Logger.Log("How to Use : File to Json");
            //Logger.Log("Excel2Json -f <input file> -o <output folder>");
            //Logger.Log("");
            Logger.Log("How to Use : Directory to Json");
            Logger.Log("Excel2Json -d <input folder> -o <output folder>");
            Logger.Log("");
            Logger.Log("==================================================");
        }

        private static void UsageHelp()
        {
            Logger.Log("==================================================");
            Logger.Log("");
            Logger.Log(VERSION);
            Logger.Log("");
            Logger.Log("Programming by ForestJ");
            Logger.Log("");
            Logger.Log("-d : The directory that contains the Excel file.(*.xlsx only)");
            //Logger.Log("-f : The path of the Excel file.(*.xlsx only)");
            Logger.Log("-o : The path to the directory where the Json file will be created. The name of the Json file is created with the name of the sheet.");
            Logger.Log("-cs : Generate C#(Csharp) file with key, data type of Json file.");
            Logger.Log("-pw : Encrypt the json file. Please enter your password. (Version C#6)");
            Logger.Log("-h/-help : Help Menual");
            Logger.Log("==================================================");
        }
    }
}
