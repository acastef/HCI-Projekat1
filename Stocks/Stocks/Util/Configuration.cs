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
        private string _defaultCurrency;
        

        public string DefaultCurrency
        {
            get { return _defaultCurrency; }
            set { _defaultCurrency = value; }
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

        private int _refreshRate;

        public int RefreshRate
        {
            get { return _refreshRate; }
            set { _refreshRate = value; }
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
                    _instance = new Configuration {
                        DefaultCurrency = "USD",
                        RefreshRate = 1
                    };
                }
                return _instance;
            }
        }
    }
}
