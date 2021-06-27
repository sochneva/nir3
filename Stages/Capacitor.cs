using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3.Stages
{
    class Capacitor : Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 
        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();   //данные для расчета

        #region Params
        public double W { get; set; }         //Расход охлаждающей воды
        public double Tlv { get; set; }       //Температура охлаждающей воды
        #endregion

        #region Output
        public double Pk { get; set; }         //Давление в конденсаторе
        public double Dtk { get; set; }        //Температурный напор в конденсаторе
        #endregion

        public Capacitor()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\Конденсатор\";

            FilenameData.Add("dTk(Dk,Tlv,W)", filenameBase + "dTk(Dk,Tlv,W)");
            FilenameData.Add("Pk(Dk,Tlv,W)", filenameBase + "Pk(Dk,Tlv,W)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

            W = Data["dTk(Dk,Tlv,W)"][0].GetParams()[0];
            Tlv = Data["dTk(Dk,Tlv,W)"][0].GetParams()[1];
        }

        public void updateParam(string name, int value, double dvd, double dnd)
        {
            switch (name)
            {
                case "TlvTrackBar":
                    Tlv = Data["dTk(Dk,Tlv,W)"].GroupBy(y => y.GetParams()[1]).Select(x => x.Key).ToArray()[value];
                    break;
                case "WTrackBar":
                    W = Data["dTk(Dk,Tlv,W)"].GroupBy(y => y.GetParams()[0]).Select(x => x.Key).ToArray()[value];
                    break;

            }
            Dtk = Interpolation(dvd + dnd, Data["dTk(Dk,Tlv,W)"].Select(x => x).Where(y => y.GetParams()[0] == W && y.GetParams()[1] == Tlv).ToList());
            Pk = Interpolation(dvd + dnd, Data["Pk(Dk,Tlv,W)"].Select(x => x).Where(y => y.GetParams()[0] == W && y.GetParams()[1] == Tlv).ToList());
        }
    }
}
