using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3.Stages
{
    class FinalPower : Stage
    {
        #region Params
        private double nupn;                       // кпд насоса
        #endregion

        #region OutputParam  
        public double Npgub { get; set; }        // Электрическая мощность брутто 
        public double Npgun { get; set; }        // Электрическая мощность нетто  
        public double Nupgub { get; set; }       // Электрический КПД ПГУ брутто 
        public double Nupgun { get; set; }       // Электрический КПД ПГУ брутто
        #endregion


        public FinalPower()
        {
            
        }

        public void updateParam(double N, double Npt, double Npn, double Ndk,double B,double Q )
        {
            Npgub = N + Npt;

            Npgun = Npgub - Npn/1000 - Ndk;

            Nupgub = Npgub * 100 / (B * Q);

            Nupgun = Npgun * 100 / (B * Q);
        }
    }
}
