using OKAWSP6A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dip3
{
    abstract class DataModel<T> where T : Data
    {
        public abstract (double,double)[] GetData(params double[] p);

        public abstract bool HasParam();

    }
    class ParameterlessDataModel : DataModel<ParameterlessData>
    {
        private readonly ParameterlessData[] modelData;
        public ParameterlessDataModel(ParameterlessData[] modelData)
        {
            this.modelData = modelData;
        }

        public override (double, double)[] GetData(params double[] p)
        {
            return modelData.Select(x => (x.data.Item1, x.data.Item2)).ToArray();
        }

        public override bool HasParam()
        {
            return false;
        }

    }

    class OneParametrDataModel : DataModel<OneParametrData>
    {
        private readonly OneParametrData[] modelData;
        public OneParametrDataModel(OneParametrData[] modelData)
        {
            this.modelData = modelData;
        }

        public override (double, double)[] GetData(params double[] p)
        {
            return modelData.Select(x => (x.data.Item1, x.data.Item2)).ToArray();
        }

        public override bool HasParam()
        {
            return false;
        }
    }

    class TwoParametrsDataModel : DataModel<TwoParametrsData>
    {
        private readonly TwoParametrsData[] modelData;
        public TwoParametrsDataModel(TwoParametrsData[] modelData)
        {
            this.modelData = modelData;
        }

        public override (double, double)[] GetData(params double[] p)
        {
            return modelData.Select(x => (x.data.Item1, x.data.Item2)).ToArray();
        }

        public override bool HasParam()
        {
            return false;
        }
    }

}
