using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    abstract class Data
    {
        //public (double, double) data;
        public abstract (double, double) GetData();

        public abstract double[] GetParams();

    }
    class ParameterlessData : Data
    {
        public new(double, double) data;
        public ParameterlessData(double x, double y)
        {
            data = (x, y);
        }
        public override (double, double) GetData() {
            return data;
        }
        
        public override double[] GetParams()
        {
            return null;
        }
    }

    class OneParametrData : Data
    {
        public new(double, double, double) data;
        public OneParametrData(double x, double y, double param1)
        {
            data = (x, y, param1);
        }
        public override (double, double) GetData()
        {
            return (data.Item1, data.Item2);
        }

        public override double[] GetParams()
        {
            return new double[] { data.Item3 };
        }
    }

    class TwoParametrsData : Data
    {
        public new(double, double, double, double) data;
        public TwoParametrsData(double x, double y, double param1, double param2)
        {
            data = (x, y, param1, param2);
        }
        public override (double, double) GetData()
        {
            return (data.Item1, data.Item2);
        }
        public override double[] GetParams()
        {
            return new double[] { data.Item3, data.Item4 };
        }
    }

}
