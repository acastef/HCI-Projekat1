using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Util;

namespace Stocks.Util
{
    class Configuration
    {
        private static  Configuration _instance;
        private Dictionary<String, Boolean> _realTimeParameters;

        private int _defaultCurrencyIndex;
        private List<String> _defaultCurrenciesList = new List<String>();

        private int _refreshRateIndex;
        private List<int> _refreshRateList = new List<int>();


        public int RefreshRateIndex
        {
            get { return _refreshRateIndex; }
            set { _refreshRateIndex = value; }
        }

        public int RefreshRate
        {
            get { return _refreshRateList[_refreshRateIndex]; }
            set { _refreshRateIndex = _refreshRateList.IndexOf(value); }
        }

        public List<int> RefreshRateList
        {
            get { return _refreshRateList; }
            set { _refreshRateList = value; }
        }

        public int DefaultCurrencyIndex
        {
            get { return _defaultCurrencyIndex; }
            set { _defaultCurrencyIndex = value; }
        }

        public List<String> DefaultCurrenciesList
        {
            get { return _defaultCurrenciesList; }
            set { _defaultCurrenciesList = value; }
        }


        public Dictionary<String, Boolean> RealTimeParameters
        {
            get { return _realTimeParameters; }
            set { _realTimeParameters = value; }
        }

        

        public string DefaultCurrency
        {
            get { return _defaultCurrenciesList[_defaultCurrencyIndex]; }
            set { _defaultCurrencyIndex = _defaultCurrenciesList.IndexOf(value); }
        }


        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private bool _help;

        public bool Help
        {
            get { return _help; }
            set { _help = value; }
        }



        private TypeSeries typeSeries;

        public TypeSeries Type
        {
            get { return typeSeries; }
            set { typeSeries = value; }
        }

        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }





        private Configuration() { }

        public static Configuration Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Configuration {};
                }
                return _instance;
            }
        }
    }
}
