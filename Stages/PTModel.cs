using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    class PTModel:Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 

        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();   //данные для расчета

        #region inputParam
        private double FlowHighSteam { get; set; }                                  //Расход пара высокого давления
        private double PressureHighSteam { get; set; } = PVD[0];                    //Давление пара высокого давления
        private double TemperatureHighSteam { get; set; } = TVD[0];                 //Температура пара высокого давления
        private double FlowLowSteam { get; set; }                                   //Расход пара низкого давления
        private double PressureLowSteam { get; set; }                               //Давление пара низкого давления
        private double TemperatureLowSteam { get; set; } = TND[0];                  //Температура пара низкого давления
        private double PressureCondenser { get; set; } = PK[0];                     //Давление пара в конденсаторе
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

        #region inputRangeParam

        public static double[] TVD { get; set; } = new double[1] { 1 };
        public static double[] PVD { get; set; } = new double[1] { 1 };
        public static double[] TND { get; set; } = new double[1] { 1 };
        public static double[] PK { get; set; } = new double[1] { 1 };

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

        public void updateParam(string name, int value)
        {
            double newValue = 1;

            /*switch (name)
            {
                case "GvdTrackBar":
                    FlowHighSteam = Data["N(Dvd)"].GetData()[0, 0] + value * (Data["N(Dvd)"].GetData()[0, Data["N(Dvd)"].GetData().GetLength(1) - 1] - Data["N(Dvd)"].GetData()[0, 0]) / 100;
                    break;
                case "TvdTrackBar":
                    TemperatureHighSteam = TVD[value];
                    break;
                case "PvdTrackBar":
                    PressureHighSteam =  PVD[value];
                    break;
                case "GndTrackBar":
                    FlowLowSteam = Data["N(Dnd)"].GetData()[0, 0] + value * (Data["N(Dnd)"].GetData()[0, Data["N(Dnd)"].GetData().GetLength(1) - 1] - Data["N(Dnd)"].GetData()[0, 0]) / 100;
                    break;
                case "TndTrackBar":
                    TemperatureLowSteam = TND[value]; 
                    break;
                case "PndTrackBar":
                    PressureLowSteam = newValue;// не используется
                    break;
                case "PkTrackBar":
                    PressureCondenser = PK[value];
                    break;
            }*/

        }

        private double calculateGrossPower()
        {
            return FlowHighSteam + FlowLowSteam;
        }

        private double calculateConsumptionSteam()
        {
            //double defN = Interpolation(FlowHighSteam, Data["N(Dvd)"].GetData());
            //double deltaVP = Interpolation(FlowHighSteam, Data["N(Pvd)"].GetData(PressureHighSteam));
            //double deltaVT = Interpolation(FlowHighSteam, Data["N(Tvd)"].GetData(TemperatureHighSteam));
            //double deltaNT = Interpolation(FlowLowSteam, Data["N(Tnd)"].GetData(TemperatureLowSteam));
            //double deltaNRT = Interpolation(FlowLowSteam, Data["N(Dnd)"].GetData());
            //double deltaNRP = Interpolation(FlowHighSteam + FlowLowSteam, Data["N(Pk)"].GetData(PressureCondenser));
            // return defN + defN + deltaVP+ deltaVT+ deltaNT+ deltaNRT + deltaNRP;
            return 1;
        }
    }
}
