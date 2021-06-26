using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nir2.DataModels
{
    class SimpleFunction : DataModel
    {
        private double[,] data;
        public double[,] GetData(double? param)
        {
            return data;
        }
        public bool HasParam()
        {
            return false;
        }

        public SimpleFunction(double[,] data)
        {
            this.data = data;
        }
    }
}
