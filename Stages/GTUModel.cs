using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    class GTUModel:Stage
    {
        private Dictionary<string, string> FilenameData = new Dictionary<string, string>(); //пути к файлам с данными для расчета 
        private Dictionary<string, List<Data>> Data { get; set; } = new Dictionary<string, List<Data>>();  //данные для расчета

        #region inputParam
        private static double OperatingTime { get; set; }               //Колич. часов экспл., ч
        private double StrainGTU { get; set; }                           //Нагрузка ГТУ, %
        private double OutdoorAirTemperature { get; set; }               //Темп. нар. воздуха, °C
        private double OutdoorAirPressure { get; set; }                  //Давление нар. воздуха, кПа
        private double PressureLossIn { get; set; }                      //Потери давлен на входе в компр., кПа
        private double PressureLossOut { get; set; }                     //Потери давлен на вых. ГТ, кПа
        #endregion

        #region outputParam
        public double N { get { return calcN(); } }                     //Электрическая мощность ГТУ брутто
        public double Nu { get { return calcNu(); } }                   //Электрический КПД брутто
        public double G { get { return calcG(); } }                     //Расход выхлопных газов
        public double T { get { return calcT(); } }                     //Температура выхлопных газов
        public double B                                                 //Расход природного газа
        {
            get
            {
                return N / (Q * Nu / 100.0);
            }
        }
        public double Q { get; set; } = DefQ;                                  //Низшая теплота сгорания топлива
        #endregion

        #region controlParam
        private static double DefN { get; } = 155.3;      //Электрическая мощность ГТУ брутто
        private static double DefNu { get; } = 34.12;     //Электрический КПД брутто
        private static double DefG { get; } = 509.0;      //Расход выхлопных газов
        private static double DefT { get; } = 537.0;      //Температура выхлопных газов
        private static double DefB                        //Расход природного газа
        {
            get
            {
                return DefN / (DefQ * DefNu);
            }
        }
        public static double DefQ { get; } = 29;          //Низшая теплота сгорания топлива
        #endregion


        public GTUModel()
        {
            string filenameBase = AppDomain.CurrentDomain.BaseDirectory + @"DataFiles\ГТУ\";

            FilenameData.Add("G(dPin)", filenameBase + "G(dPin)");
            FilenameData.Add("G(Ngtu)", filenameBase + "G(Ngtu)");
            FilenameData.Add("N(dPin)", filenameBase + "N(dPin)");
            FilenameData.Add("N(Tnv)", filenameBase + "N(Tnv)");
            FilenameData.Add("N(Pnv)", filenameBase + "N(Pnv)");
            FilenameData.Add("N(dPout)", filenameBase + "N(dPout)");
            FilenameData.Add("Nu(dPin)", filenameBase + "Nu(dPin)");
            FilenameData.Add("Nu(Ngtu)", filenameBase + "Nu(Ngtu)");
            FilenameData.Add("Nu(Tnv)", filenameBase + "Nu(Tnv)");
            FilenameData.Add("T(dPin)", filenameBase + "T(dPin)");
            FilenameData.Add("T(dPout)", filenameBase + "T(dPout)");
            FilenameData.Add("T(Ngtu)", filenameBase + "T(Ngtu)");
            FilenameData.Add("T(Tnv)", filenameBase + "T(Tnv)");
            FilenameData.Add("N(t)", filenameBase + "N(t)");

            foreach (var x in FilenameData)
                Data.Add(x.Key, FileManager.ReadFromFile<Data>(filenameBase, x.Key));

            OperatingTime = Data["N(t)"][0].GetData().Item1;
            StrainGTU = Data["Nu(Ngtu)"][0].GetData().Item1;
            OutdoorAirTemperature = Data["N(Tnv)"][0].GetData().Item1;
            OutdoorAirPressure = Data["N(Pnv)"][0].GetData().Item1;
            PressureLossIn = Data["N(dPin)"][0].GetData().Item1;
            PressureLossOut = Data["N(dPout)"][0].GetData().Item1;
        }

        public void updateParam(string name, int value)
        {
            string paramName = string.Empty;
            switch (name)
            {
                case "tTrackBar":
                    paramName = "N(t)";
                    break;
                case "TnvTrackBar":
                    paramName = "N(Tnv)";
                    break;
                case "PnvTrackBar":
                    paramName = "N(Pnv)";
                    break;
                case "NgtuTrackBar":
                    paramName = "Nu(Ngtu)";
                    break;
                case "dPinTrackBar":
                    paramName = "N(dPin)";
                    break;
                case "dPoutTrackBar":
                    paramName = "N(dPout)";
                    break;
                default:
                    return;
            }

            double newValue = Data[paramName][0].GetData().Item1 + value * (Data[paramName][Data[paramName].Count - 1].GetData().Item1 - Data[paramName][0].GetData().Item1) / 100;
            switch (name)
            {
                case "tTrackBar":
                    OperatingTime = newValue;
                    break;
                case "TnvTrackBar":
                    OutdoorAirTemperature = newValue;
                    break;
                case "PnvTrackBar":
                    OutdoorAirPressure = newValue;
                    break;
                case "NgtuTrackBar":
                    StrainGTU = newValue;
                    break;
                case "dPinTrackBar":
                    PressureLossIn = newValue;
                    break;
                case "dPoutTrackBar":
                    PressureLossOut = newValue;
                    break;
            }
            
        }

        private double calcN()
        {
            return DefN * Interpolation(OperatingTime, Data["N(t)"]) * Interpolation(OutdoorAirTemperature, Data["N(Tnv)"]) * Interpolation(OutdoorAirPressure, Data["N(Pnv)"])
                * Interpolation(PressureLossIn, Data["N(dPin)"]) * Interpolation(PressureLossOut, Data["N(dPout)"]);
        }

        private double calcNu()
        {
            return DefNu * Interpolation(OperatingTime, Data["N(t)"]) * Interpolation(StrainGTU, Data["Nu(Ngtu)"])
                * Interpolation(OutdoorAirTemperature, Data["Nu(Tnv)"]) * Interpolation(PressureLossIn, Data["Nu(dPin)"])
                * Interpolation(PressureLossOut, Data["N(dPout)"]);
        }

        private double calcG()
        {
            return DefG * Interpolation(OutdoorAirPressure, Data["N(Pnv)"]) * Interpolation(PressureLossIn, Data["G(dPin)"])
                * Interpolation(StrainGTU, Data["G(Ngtu)"]);
        }

        private double calcT()
        {
            return DefT * Interpolation(StrainGTU, Data["T(Ngtu)"]) * Interpolation(OutdoorAirTemperature, Data["T(Tnv)"])
                 * Interpolation(PressureLossIn, Data["T(dPin)"]) * Interpolation(PressureLossOut, Data["T(dPout)"]);
        }
    }
}
