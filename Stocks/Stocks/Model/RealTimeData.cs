using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Model
{
    public class RealTimeData
    {
        public RealTimeData()
        {

        }

        public double Value { get; set; }
        public DateTime Date { get; set; }
        public double Trend { get; set; }
    }
}
