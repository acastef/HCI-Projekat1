using Avapi;
using Avapi.AvapiCURRENCY_EXCHANGE_RATE;
using Avapi.AvapiDIGITAL_CURRENCY_INTRADAY;
using Avapi.AvapiTIME_SERIES_INTRADAY;
using Stocks.Model;
using Stocks.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for RealtimeViewer.xaml
    /// </summary>
    public partial class RealtimeViewer : UserControl, INotifyPropertyChanged
    {
        private double _currentValue;
        private double _lastValue;
        private String _trendPergentage;
        private String _trend;
        private Brush _color;
        private FetchArgs _args;
        private IAvapiConnection _connection = AvapiConnection.Instance;
        private Task _backgroindWork;
        private double _exchangeRate = 1;

        public RealtimeViewer()
        {
            InitializeComponent();
            addRefreshRates();
            _connection.Connect("5XQ6Y6JJKEOQ7JRU");

            _args = new FetchArgs
            {
                DefaultCurrency = Configuration.Instance.DefaultCurrency,
                Symbol = Configuration.Instance.Symbol,
                FullName = Configuration.Instance.FullName,
                RefreshRate = Configuration.Instance.RefreshRate,
                Type = Configuration.Instance.Type

            };


            Init();
            DataContext = this;
        }

        private void addRefreshRates()
        {
            List<ComboBoxItem> comboBoxes = new List<ComboBoxItem>();

            foreach (int i in Configuration.Instance.RefreshRateList)
            {
                ComboBoxItem cb = new ComboBoxItem();
                cb.Content = i + " minute(s)";
                comboBoxes.Add(cb);
            }

            RefreshRate.ItemsSource = comboBoxes;
        }

        private async void Init()
        {
            RealTimeData value;
            await Task.Run(() =>
            {
                while (true)
                {
                    value = GetData();
                    if (CurrentValue != value.Value || CurrentValue != 0)
                    {
                        LastValue = CurrentValue;
                        CurrentValue = value.Value;
                        if (LastValue == 0)
                        {
                            LastValue = CurrentValue;
                        }
                        Console.WriteLine(_args.FullName + "  " + _currentValue);
                        double pom1 = CurrentValue - LastValue;
                        if (pom1 > 0)
                        {
                            Color = Brushes.Green;
                        }
                        else if (pom1 < 0)
                        {
                            Color = Brushes.Red;
                        }
                        else
                        {
                            Color = Brushes.WhiteSmoke;
                        }
                        pom1 = Math.Truncate(pom1 * 100) / 100;
                        Trend = string.Format("{0:N2}", pom1);
                        if (LastValue != 0)
                            pom1 = Math.Truncate(pom1 / LastValue * 10000) / 100;
                        else
                            pom1 = 99999999;
                        TrendPercentage = string.Format("{0:N2}%", pom1);
                    }
                    if (CurrentValue == 0 && LastValue != 0)
                    {
                        CurrentValue = LastValue;
                        Trend = "0.00";
                        TrendPercentage = "0.00%";
                        Color = Brushes.WhiteSmoke;
                    }
                    
                    Thread.Sleep(_args.RefreshRate * 60000);
                }

            });
        }

        private RealTimeData GetData()
        {

            if (_args.DefaultCurrency != "USD" && _exchangeRate == 1 && _args.Type != TypeSeries.CURRENCY)
            {
                try
                {
                    Int_CURRENCY_EXCHANGE_RATE currency_exchange_rate = _connection.GetQueryObject_CURRENCY_EXCHANGE_RATE();
                    IAvapiResponse_CURRENCY_EXCHANGE_RATE currency_exchange_rateResponse =
                    currency_exchange_rate.QueryPrimitive("USD", _args.DefaultCurrency);
                    var data2 = currency_exchange_rateResponse.Data;
                    if (data2.Error)
                        MessageBox.Show("Failed to fetch data for exchage rate", "Error");
                    else
                    {
                        _exchangeRate = double.Parse(data2.ExchangeRate);
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Failed to fetch currency exchange rate for chosen currency. Values will be show in USD", "Error");
                }

            }

            if (_args.Type == TypeSeries.STOCK)
            {
                Int_TIME_SERIES_INTRADAY time_series_intraday =
                _connection.GetQueryObject_TIME_SERIES_INTRADAY();

                try
                {
                    //Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min;
                    //if (Refreshrate == "1 minute(s)") 
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min;
                    //}
                    //else if (Refreshrate == "5 minutes(s)")
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_5min;
                    //}
                    //else if (Refreshrate == "15 minute(s)")
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_15min;
                    //}
                    //else if (Refreshrate == "30 minute(s)")
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_30min;
                    //}
                    //else if (Refreshrate == "60 minute(s)")
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_60min;
                    //}
                    //else
                    //{
                    //    cst = Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.none;
                    //}
                    IAvapiResponse_TIME_SERIES_INTRADAY time_series_intradayResponse =
                    time_series_intraday.Query(
                     _args.Symbol,
                     Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min,
                     Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_outputsize.compact);

                    var data = time_series_intradayResponse.Data;
                    if (data.Error)
                    {
                        MessageBox.Show("Failed to fetch data for " + _args.Symbol, "Error");
                    }
                    else
                    {


                        return new RealTimeData
                        {
                            Value = double.Parse(data.TimeSeries.First().close) * _exchangeRate,
                            Date = DateTime.ParseExact(data.TimeSeries.First().DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            Trend = 0
                        };
                    }
                }
                catch (Exception)
                {
                    //MessageBox.Show("Failed to send request", "Error");
                    Console.WriteLine("Desila je greska za deonice!");
                }
            }
            else if (_args.Type == TypeSeries.DIGITAL_CURRENCY)
            {
                Int_DIGITAL_CURRENCY_INTRADAY digital_currency_intraday =
               _connection.GetQueryObject_DIGITAL_CURRENCY_INTRADAY();
                try
                {
                    IAvapiResponse_DIGITAL_CURRENCY_INTRADAY digital_currency_intradayResponse =
               digital_currency_intraday.QueryPrimitive(_args.Symbol, "USD");

                    var data = digital_currency_intradayResponse.Data;
                    if (data.Error)
                    {
                        MessageBox.Show("Failed to fetch data for " + _args.FullName);
                    }
                    else
                    {
                        return new RealTimeData
                        {
                            Value = double.Parse(data.TimeSeries.First().Price) * _exchangeRate,
                            Date = DateTime.ParseExact(data.TimeSeries.First().DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            Trend = 0
                        };
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Desila je greska za kripto valute!");
                }

            }
            else if (_args.Type == TypeSeries.CURRENCY)
            {
                try
                {
                    Int_CURRENCY_EXCHANGE_RATE currency_exchange_rate = _connection.GetQueryObject_CURRENCY_EXCHANGE_RATE();
                    IAvapiResponse_CURRENCY_EXCHANGE_RATE currency_exchange_rateResponse =
                    currency_exchange_rate.QueryPrimitive(_args.Symbol, _args.DefaultCurrency);
                    var data2 = currency_exchange_rateResponse.Data;
                    if (data2.Error)
                        MessageBox.Show("Failed to fetch data for exchage rate for " + _args.Symbol, "Error");
                    else
                    {

                        var temp = new RealTimeData
                        {
                            Value = double.Parse(data2.ExchangeRate),
                            Date = DateTime.Now,
                            Trend = 0
                        };
                        Write(temp);
                        return temp;

                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Failed to fetch data for exchage rate for " + _args.Symbol, "Error");
                }
            }

            return new RealTimeData();
        }

        private void Write(RealTimeData value)
        {
            string currentPaht = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = currentPaht + "\\Files\\Currencies\\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += _args.Symbol + ".csv";

                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(value.Date.ToString("dd-MM-yyyy HH:MM:ss") + "," + value.Value / _exchangeRate);

                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

        }

        public double CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        public double LastValue
        {
            get { return _lastValue; }
            set
            {
                _lastValue = value;
                OnPropertyChanged("LastValue");
            }
        }


        public String Trend
        {
            get { return _trend; }
            set
            {
                _trend = value;
                OnPropertyChanged("Trend");
            }
        }

        public String TrendPercentage
        {
            get { return _trendPergentage; }
            set
            {
                _trendPergentage = value;
                OnPropertyChanged("TrendPercentage");
            }
        }

        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
