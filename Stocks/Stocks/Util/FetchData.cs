using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;


namespace Stocks.Util
{
    

    public class FetchArgs
    {
        public string Symbol { get; set; }
        public string DefaultCurrency { get; set; }
        public string FullName { get; set; }
        public int RefreshRate { get; set; }
        public TypeSeries Type { get; set; }
    }

    public enum TypeSeries
    {
        STOCK,CURRENCY, DIGITAL_CURRENCY
    }

}
