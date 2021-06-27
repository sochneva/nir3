using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nir2.DataModels
{
    class ParamFunction : DataModel
    {
        private Dictionary<double, double[,]> data;
        public double[,] GetData(double? param)
        {
            return data[param ?? 0];
        }
        public bool HasParam()
        {
            return true;
        }
        public ParamFunction(Dictionary<double, double[,]> data)
        {
            this.data = data;
        }
    }
}
