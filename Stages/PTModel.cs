using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    class PTModel : Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 

        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();   //данные для расчета

        #region inputParam
        private double FlowHighSteam { get; set; }                        //Расход пара высокого давления
        private double PressureHighSteam { get; set; }                    //Давление пара высокого давления
        private double TemperatureHighSteam { get; set; }                 //Температура пара высокого давления
        private double FlowLowSteam { get; set; }                         //Расход пара низкого давления
        private double PressureLowSteam { get; set; }                      //Давление пара низкого давления
        private double TemperatureLowSteam { get; set; }                   //Температура пара низкого давления
        private double PressureCondenser { get; set; }                     //Давление пара в конденсаторе
        #endregion

        #region outputParam
        public double GrossPower { get { return calculateGrossPower(); } }                     //Электрическая мощность ПТ брутто
        public double ConsumptionSteam { get { return calculateConsumptionSteam(); } }         //Расход пара в конденсаторе
        #endregion

        #region controlParam
        private double DefFlowHighSteam { get; } = 63.4;                       //Расход пара высокого давления
        private double DefPressureHighSteam { get; } = 7.51;                   //Давление пара высокого давления
        private double DefTemperatureHighSteam { get; } = 501;                 //Температура пара высокого давления
        private double DefFlowLowSteam { get; } = 15.9;                        //Расход пара низкого давления
        private double DefPressureLowSteam { get; } = 206;                     //Давление пара низкого давления
        private double DefTemperatureLowSteam { get; } = 0.542;                //Температура пара низкого давления
        private double DefPressureCondenser { get; } = 0.1275;                 //Давление пара в конденсаторе

        #endregion

        public PTModel()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\ПТ\";

            FilenameData.Add("N(Dnd)", filenameBase + "N(Dnd)");
            FilenameData.Add("N(Dvd)", filenameBase + "N(Dvd)");
            FilenameData.Add("N(Pk)", filenameBase + "N(Pk)");
            FilenameData.Add("N(Pvd)", filenameBase + "N(Pvd)");
            FilenameData.Add("N(Tnd)", filenameBase + "N(Tnd)");
            FilenameData.Add("N(Tvd)", filenameBase + "N(Tvd)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

        }

        public void updateParam(double FlowHighSteam, double TemperatureHighSteam, double PressureHighSteam, double FlowLowSteam, double TemperatureLowSteam, double PressureCondenser)
        {
            this.FlowHighSteam = FlowHighSteam;
            this.TemperatureHighSteam = TemperatureHighSteam;
            this.PressureHighSteam = PressureHighSteam;
            this.FlowLowSteam = FlowLowSteam;
            this.TemperatureLowSteam = TemperatureLowSteam;
            this.PressureCondenser = PressureCondenser;
        }

        private double calculateGrossPower()
        {
            double PressureHighSteamParam = Data["N(Pvd)"].GroupBy(y => y.GetParams()[0]).Select(x => x.Key).ToArray().OrderBy(x => Math.Abs(x - PressureHighSteam)).First();
            double TemperatureHighSteamParam = Data["N(Tvd)"].GroupBy(y => y.GetParams()[0]).Select(x => x.Key).ToArray().OrderBy(x => Math.Abs(x - TemperatureHighSteam)).First();
            double TemperatureLowSteamParam = Data["N(Tnd)"].GroupBy(y => y.GetParams()[0]).Select(x => x.Key).ToArray().OrderBy(x => Math.Abs(x - TemperatureLowSteam)).First();
            double PressureCondenserParam = Data["N(Pk)"].GroupBy(y => y.GetParams()[0]).Select(x => x.Key).ToArray().OrderBy(x => Math.Abs(x - PressureCondenser)).First();

            double defN = Interpolation(FlowHighSteam, Data["N(Dvd)"]);
            double deltaVP = Interpolation(FlowHighSteam, Data["N(Pvd)"].Select(x => x).Where(y => y.GetParams()[0] == PressureHighSteamParam).ToList());
            double deltaVT = Interpolation(FlowHighSteam, Data["N(Tvd)"].Select(x => x).Where(y => y.GetParams()[0] == TemperatureHighSteamParam).ToList());
            double deltaNT = Interpolation(FlowLowSteam, Data["N(Tnd)"].Select(x => x).Where(y => y.GetParams()[0] == TemperatureLowSteamParam).ToList());
            double deltaNRT = Interpolation(FlowLowSteam, Data["N(Dnd)"]);
            double deltaNRP = Interpolation(FlowHighSteam + FlowLowSteam, Data["N(Pk)"].Select(x => x).Where(y => y.GetParams()[0] == PressureCondenserParam).ToList());
            return defN + (deltaVP + deltaVT + deltaNT + deltaNRT + deltaNRP) / 1000;

        }

        private double calculateConsumptionSteam()
        {
            return FlowHighSteam + FlowLowSteam;
        }
    }
}
