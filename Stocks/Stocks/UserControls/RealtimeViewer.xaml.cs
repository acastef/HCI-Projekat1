using Avapi;
using Avapi.AvapiTIME_SERIES_INTRADAY;
using Stocks.Model;
using Stocks.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private double _trend;
        private FetchArgs _args;
        private IAvapiConnection _connection = AvapiConnection.Instance;
        private Task _backgroindWork;

        public RealtimeViewer()
        {
            InitializeComponent();

            _connection.Connect("5XQ6Y6JJKEOQ7JRU");

            _args = new FetchArgs
            {
                DefaultCurrency = Configuration.Instance.DefaultCurrency,
                Symbol = Configuration.Instance.Symbol,
                FullName = Configuration.Instance.FullName,
                RefreshRate = Configuration.Instance.RefreshRate
            };

            Task.Run(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                while (true)
                {
                    RealTimeData value = new RealTimeData();
                    
                    _backgroindWork = Task.Factory.StartNew(() =>
                    {
                        value = GetData();
                    }, TaskCreationOptions.LongRunning);

                  

                    _backgroindWork.ContinueWith(x =>
                    {
                        CurrentValue = value.Value;
                        Trend = value.Trend;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    Thread.Sleep(_args.RefreshRate * 10000);
                }
                
            });


            DataContext = this;
        }

        private RealTimeData GetData()
        {

            
            
            Int_TIME_SERIES_INTRADAY time_series_intraday =
                _connection.GetQueryObject_TIME_SERIES_INTRADAY();

            try
            {
                IAvapiResponse_TIME_SERIES_INTRADAY time_series_intradayResponse =
            time_series_intraday.Query(
                 _args.Symbol,
                 Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min,
                 Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_outputsize.compact);

                var data = time_series_intradayResponse.Data;
                if (data.Error)
                {
                    MessageBox.Show("Failed to fetch data", "Error");
                }
                else
                {
                    Console.WriteLine("Information: " + data.MetaData.Information);
                    Console.WriteLine("Symbol: " + data.MetaData.Symbol);
                    Console.WriteLine("LastRefreshed: " + data.MetaData.LastRefreshed);
                    Console.WriteLine("Interval: " + data.MetaData.Interval);
                    Console.WriteLine("OutputSize: " + data.MetaData.OutputSize);
                    Console.WriteLine("TimeZone: " + data.MetaData.TimeZone);
                    Console.WriteLine("========================");
                    Console.WriteLine("========================");
                    return new RealTimeData
                    {
                        Value = double.Parse(data.TimeSeries.First().close),
                        Date = DateTime.ParseExact(data.TimeSeries.First().DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        Trend = 75.2
                    };
                   
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Failed to send request", "Error");
                Console.WriteLine("Desila je greska!");
            }


            return new RealTimeData();
        }

        public double CurrentValue
        {
            get { return _currentValue; }
            set {
                _currentValue = value;
                OnPropertyChanged("CurrentValue");
                }
        }


        public double Trend
        {
            get { return _trend; }
            set {
                _trend = value;
                OnPropertyChanged("Trend");
                }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
