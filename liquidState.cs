using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OKAWSP6A;
using SharpFluids;
using UnitsNet;

namespace dip3
{
    class LiquidState
    {
        public LiquidState()
        {
            WSPCalculator a = new WSPCalculator();
            var b = a.wspTSP(0);
            //Water.UpdatePT(Pressure.FromBars(1.013), Temperature.FromDegreesCelsius(13));
        }
    }
}
