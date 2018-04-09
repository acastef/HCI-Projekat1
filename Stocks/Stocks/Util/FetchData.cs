using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;


namespace Stocks.Util
{
    

    public abstract class FetchArgs
    {
        public string Symbol { get; set; }
        public string DefaultCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }

    public enum HistoryRate
    {
        MIN, YEARS5, YEARS2, YEARS1, MONTS3, MONTS1, DAYS10, NONE
    }

    public class RealTimeArgs : FetchArgs
    {
        public int RefreshRate { get; set; }
        
    }

    public class HistoryArgs : FetchArgs
    {
        public HistoryRate DatePoint { get; set; }
    }


}
