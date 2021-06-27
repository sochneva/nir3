using OKAWSP6A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace dip3
{
    class RecoveryBoiler : Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 
        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();  //данные для расчета

        #region Params
        private double Gokb = 70.7;
        private double Grec = 0;
        private double Gbype = 8;
        private double nugpk = 0.99;
        private double dvd;
        private double nuind = 0.99;
        private double deltappend = 0.008;
        private double nuekvd = 0.99;
        private double nuivd = 0.99;
        private double deltapevd = 0.04;
        private double nupevd = 0.99;
        private double nupendgas = 0.99;
        #endregion

        #region Output
        public double Dvd { get; set; } = 0;         //Расход пара высокого давления
        public double Tpevd { get; set; } = 0;       //Температура пара высокого давления
        public double Pbvd { get; set; } = 0;         //Давление пара высокого давления
        public double Dnd { get; set; } = 0;         //Расход пара низкого давления
        public double Tpend { get; set; } = 0;       //Расход пара низкого давления
        public double Pbnd { get; set; } = 0;      //Расход пара низкого давления
        #endregion

        public RecoveryBoiler()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\КУ\";

            FilenameData.Add("Dnd(Dvd)", filenameBase + "Dnd(Dvd)");
            FilenameData.Add("Pnd(Ngtu,tnv)", filenameBase + "Pnd(Ngtu,tnv)");
            FilenameData.Add("Pvd(Ngtu,tnv)", filenameBase + "Pvd(Ngtu,tnv)");
            FilenameData.Add("Tnd(Ngtu,tnv)", filenameBase + "Tnd(Ngtu,tnv)");
            FilenameData.Add("Tokb(Ngtu,tnv)", filenameBase + "Tokb(Ngtu,tnv)");
            FilenameData.Add("Tuh(Ngtu,tnv)", filenameBase + "Tuh(Ngtu,tnv)");
            FilenameData.Add("Tvd(Ngtu,tnv)", filenameBase + "Tvd(Ngtu,tnv)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

            dvd = Data["Dnd(Dvd)"][0].GetData().Item1;
        }

        public void updateParam(string name, int value, double tnv, double ngtu, double gkt, double t)
        {
            switch (name)
            {
                case "DvdTrackBar":
                    dvd = Data["Dnd(Dvd)"][0].GetData().Item1 + value * (Data["Dnd(Dvd)"][Data["Dnd(Dvd)"].Count - 1].GetData().Item1 - Data["Dnd(Dvd)"][0].GetData().Item1) / 100;
                    break;
            }

            Dvd = dvd;

            if ((ngtu < 25 || ngtu > 100) || (tnv<=-3.1 || tnv>=37))
                return;

            WSPCalculator wspCalculator = new WSPCalculator();
            double tokbout = BilinearInterpolation(ngtu, tnv, Data["Tokb(Ngtu,tnv)"]); //Interpolation(ngtu, Data["Tokb(Ngtu,tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            double hokbout = 4.2 * tokbout;

            double pbnd = BilinearInterpolation(ngtu, tnv, Data["Pnd(Ngtu,tnv)"]);//Interpolation(ngtu, Data["Pnd(Ngtu,tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            Pbnd = pbnd;

            double tgpkout = wspCalculator.wspTSP(pbnd * 1000000) - 273.15;
            double hgpkout = wspCalculator.wspHSWT(tgpkout + 273.15);

            double Ggpk = Gokb + Grec - Gbype;
            double hgpkin = hokbout;
            double Qgpkvod = (hgpkout - hgpkin) * Ggpk;

            double tbype = tokbout;
            double tbnd = (Ggpk * tgpkout + Gbype * tbype) / (Ggpk + Gbype);

            double Gokoutbn = Ggpk + Gbype - Grec;
            double QgpkGas = Qgpkvod / nugpk;

            double tux = BilinearInterpolation(ngtu, tnv, Data["Tuh(Ngtu,tnv)"]); //Interpolation(ngtu, Data["Tuh(Ngtu, tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            double hyx = 1.1 * tux;
            double hgasgpkin = hyx + QgpkGas / gkt;

            double toutind = wspCalculator.wspTSP(pbnd * 1000000) - 273.15;
            double houtind = wspCalculator.wspHSST(toutind + 273.15);

            double Dind = Interpolation(dvd, Data["Dnd(Dvd)"]);
            Dnd = Dind;

            double Qindvod = (houtind - hgpkout) * Dind;
            double Qindgas = Qindvod / nuind;

            double hgasindin = hgasgpkin + Qindgas / gkt;
            double ppendout = pbnd * (1 - deltappend);
            double tpend = BilinearInterpolation(ngtu, tnv, Data["Tnd(Ngtu,tnv)"]); //Interpolation(ngtu, Data["Tnd(Ngtu,tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            Tpend = tpend;
            double houtpend = wspCalculator.wspHPT(ppendout * 1000000, tpend + 273.15);

            double Qpendvod = (houtpend - houtind) * Dind;
            double Qpendgas = Qpendvod / nupendgas;
            double hgaspendin = hgasindin + Qpendgas / gkt;
            double Gevd = Gokoutbn - Dind;

            double pbvd = BilinearInterpolation(ngtu, tnv, Data["Pvd(Ngtu,tnv)"]); //Interpolation(ngtu, Data["Pvd(Ngtu,tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            Pbvd = pbvd;

            double pvodinevd = pbvd;
            double hinevd = wspCalculator.wspHPT(pvodinevd * 1000000, toutind + 273.15);
            double toutevd = wspCalculator.wspTSP(pvodinevd * 1000000) - 273.15;
            double houtevd = wspCalculator.wspHSWT(toutevd + 273.15);
            double Qevdvod = (houtevd - hinevd) * Gevd;
            double Qevdgas = Qevdvod / nuekvd;

            double hgasevdin = hgaspendin + Qevdgas / gkt;
            double toutivd = wspCalculator.wspTSP(pbvd * 1000000) - 273.15;
            double houtivd = wspCalculator.wspHSST(toutivd + 273.15);

            double Qivdvod = (houtivd - houtevd) * Gevd;
            double Qivdgas = Qivdvod / nuivd;
            double hgasivdin = hgasevdin + Qivdvod / gkt;
            double ppevdout = pbvd * (1 - deltapevd);

            double tpevd = BilinearInterpolation(ngtu, tnv, Data["Tvd(Ngtu,tnv)"]); //Interpolation(ngtu, Data["Tvd(Ngtu,tnv)"].Select(x => x).Where(y => y.GetParams()[0] == tnv).ToList());
            Tpevd = tpevd;

            double houtpevd = wspCalculator.wspHPT(ppevdout * 1000000, tpevd + 273.15);
            double Qpevdvod = (houtpevd - houtivd) * Gevd;
            double Qpevdgas = Qpendvod / nupevd;
            double hgaspevdin = hgasivdin + Qpevdgas / gkt;

            double hkt = 1.1 * t;

            bool checkH = (hkt - hgaspevdin) / (hkt * 100) < 0.01;
        }
    }
}
