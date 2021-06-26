using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    static class FileManager
    {
        //загрузка модели данных
        public static List<Data> ReadFromFile<T>(string foldername, string filename)
        {
            try
            {
                if (File.Exists(foldername + filename + ".txt"))
                    return ExtractDataFromFile(foldername, filename);
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось загрузить файл: " + e.Message);
                return null;
            }
        }

        //Выгрузка данных из файла
        public static List<Data> ExtractDataFromFile(string foldername, string filename)
        {
            //File.ReadLines(file.FullName).Count()
            try
            {
                List<Data> dataFromFile = new List<Data>();
                foreach (FileInfo file in new DirectoryInfo(foldername).GetFiles(filename + ".txt"))
                {
                    string line = string.Empty;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        List<double> paramas = new List<double>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("params"))
                                paramas = GetParamsFromFile(line.Replace("params",""));
                            else
                                dataFromFile.Add(GetValuesFromFile<Data>(line, paramas));
                        }
                    }
                }
                return dataFromFile;
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось загрузить файл: " + e.Message);
                return null;
            }
        }

        private static T GetValuesFromFile<T>(string line, List<double> paramas) where T : Data
        {
            var values = GetXY(line);

            switch (paramas.Count)
            {
                case 0:
                    return new ParameterlessData(values.Item1, values.Item2) as T;
                case 1:
                    return new OneParametrData(values.Item1, values.Item2, paramas[0]) as T;
                case 2:
                    return new TwoParametrsData(values.Item1, values.Item2, paramas[0], paramas[1]) as T;
                default:
                    return new ParameterlessData(values.Item1, values.Item2) as T;
            }
        }

        private static List<double> GetParamsFromFile(string line)
        {
            string[] dataParams = line.Replace('.', ',').Trim().Split(' ');
            return dataParams.Select(x => Convert.ToDouble(x.Remove(0, 2))).ToList();
        }

        private static (double, double) GetXY(string line)
        {
            string[] param = line.Replace('.', ',').Trim().Split(' ');
            return (Convert.ToDouble(param[0]), Convert.ToDouble(param[1]));
        }
    }
}
