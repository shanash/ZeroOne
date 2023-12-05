
using System;
using System.Collections.Generic;
using System.IO;

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
    class Program
    {
        static string VERSION = "Excel2Json Ver.1.5.0";


        static void Main()
        {
            string[] args = { "-d", "Android\\ExcelData", "-o", "Assets\\AssetResources\\Master", "-cs", "Assets\\Scripts\\MasterData" };

            if (args.Length == 0)
            {
                Console.WriteLine("== Please enter parameters. ==");
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
                            Console.WriteLine("===== Error =====");
                            Console.WriteLine("The directory to change does not exist.");
                            Console.WriteLine(args[idx]);
                            Console.WriteLine(input_path);
                            Console.WriteLine("=================");
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
                //            Console.WriteLine("");
                //            Console.WriteLine("The Excel file to change does not exist.");
                //            Console.WriteLine("");
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
                            Console.WriteLine("===== Error =====");
                            Console.WriteLine("The directory where the Json file will be saved does not exist.");
                            Console.WriteLine(output_path);
                            Console.WriteLine("=================");
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
                            Console.WriteLine("===== Error =====");
                            Console.WriteLine("The directory where the C#(Csharp) file will be saved does not exist.");
                            Console.WriteLine(csharp_output_dir);
                            Console.WriteLine("=================");
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
                Console.WriteLine("===== Error =====");
                Console.WriteLine("The directory path where the Json file will be saved is not specified.");
                Console.WriteLine("=================");
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
                    Console.WriteLine("\n");

                    foreach (string fname in files)
                    {
                        string ext = Path.GetExtension(fname);
                        bool is_backup_file = fname.IndexOf("$") == -1 ? false : true;
                        if (ext == ".xlsx" && !is_backup_file)
                        {
                            Console.WriteLine("Convert => {0}", fname);
                            ExcelApp.ReadExcelData(fname, output_path, is_csharp_make, csharp_output_dir, is_encrypt, encrypt_password, ref master_table_columns, ref master_enum_data_list);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("Directory Is Not Exist!! [{0}]", args[1]));
                    Console.WriteLine("");
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
            //        Console.WriteLine("");
            //        Console.WriteLine(string.Format("File Is Not Exist!! [{0}]", input_path));
            //        Console.WriteLine("");
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
            Console.WriteLine("==================================================");
            Console.WriteLine("");
            Console.WriteLine(VERSION);
            Console.WriteLine("");
            Console.WriteLine("Programming by ForestJ");
            Console.WriteLine("");
            //Console.WriteLine("How to Use : File to Json");
            //Console.WriteLine("Excel2Json -f <input file> -o <output folder>");
            //Console.WriteLine("");
            Console.WriteLine("How to Use : Directory to Json");
            Console.WriteLine("Excel2Json -d <input folder> -o <output folder>");
            Console.WriteLine("");
            Console.WriteLine("==================================================");
        }

        private static void UsageHelp()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("");
            Console.WriteLine(VERSION);
            Console.WriteLine("");
            Console.WriteLine("Programming by ForestJ");
            Console.WriteLine("");
            Console.WriteLine("-d : The directory that contains the Excel file.(*.xlsx only)");
            //Console.WriteLine("-f : The path of the Excel file.(*.xlsx only)");
            Console.WriteLine("-o : The path to the directory where the Json file will be created. The name of the Json file is created with the name of the sheet.");
            Console.WriteLine("-cs : Generate C#(Csharp) file with key, data type of Json file.");
            Console.WriteLine("-pw : Encrypt the json file. Please enter your password. (Version C#6)");
            Console.WriteLine("-h/-help : Help Menual");
            Console.WriteLine("==================================================");
        }
    }
}
