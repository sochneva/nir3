using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    class BoosterCompressor : Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 
        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();   //данные для расчета

        #region Params
        private double Gskb = 70.7;

        #endregion
        public double BoosterCompressorValue { get; set; }

        public BoosterCompressor()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\ДК\";

            FilenameData.Add("Ndk(N)", filenameBase + "Ndk(N)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

        }

        public void updateParam(double value)
        {
            BoosterCompressorValue = Interpolation(value, Data["Ndk(N)"]);
        }

    }
}
