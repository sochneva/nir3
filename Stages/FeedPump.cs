using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3.Stages
{
    class FeedPump : Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 
        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();   //данные для расчета

        #region Params
        private double nupn;                       // кпд насоса
        private double nued = 95.3;                // кпд электродвигателя
        private double vsr = 1 / 903.3;
        private double Qnd;
        private double Qvd;
        private double n;
        #endregion

        #region OutputParam  
        public double N { get; set; }       // мощность насоса
        #endregion

        public FeedPump()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\Насос\";

            FilenameData.Add("H", filenameBase + "H");
            FilenameData.Add("N", filenameBase + "N");
            FilenameData.Add("Nupn(Q)", filenameBase + "Nupn(Q)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

            Qnd = Data["Nupn(Q)"][0].GetData().Item1;
            Qvd = Data["H"][0].GetData().Item1;
        }

        public void updateParam(string name, int value, double pvd, double pnd)
        {
            switch (name)
            {
                case "QndTrackBar":
                    Qnd = Data["Nupn(Q)"][0].GetData().Item1 + value * (Data["Nupn(Q)"][Data["Nupn(Q)"].Count - 1].GetData().Item1 - Data["Nupn(Q)"][0].GetData().Item1) / 100;
                    break;
                case "QvdTrackBar":
                    Qvd = Data["H"][0].GetData().Item1 + value * (Data["H"][Data["H"].Count - 1].GetData().Item1 - Data["H"][0].GetData().Item1) / 100;
                    break;
            }
            nupn = Interpolation(Qnd, Data["Nupn(Q)"]);
            double Nlow = vsr * (pvd - pnd) * Qnd / (nued * nupn);
            double H = pvd - pnd;

            List<(double,double)> sampleData = new List<(double,double)>();
            foreach (var p in Data["H"].GroupBy(x => x.GetParams()[0]).Select(y => y.Key).ToArray())
                sampleData.Add((p,Interpolation(Qvd, Data["H"].Select(u => u).Where(o => o.GetParams()[0] == p).ToList())));
            n  = sampleData.OrderBy(x => Math.Abs(x.Item2 - H)).First().Item1;

            double Nhigh = Interpolation(Qvd, Data["N"].Select(x => x).Where(y => y.GetParams()[0] == n).ToList());

            N = (Nlow + Nhigh) * 2;
        }
    }
}
