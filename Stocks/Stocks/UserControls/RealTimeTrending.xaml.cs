using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Stocks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using Stocks.Util;
using Avapi;
using Avapi.AvapiTIME_SERIES_INTRADAY;
using System.Globalization;
using LiveCharts.Defaults;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for RealTimeTrending.xaml
    /// </summary>
    public partial class RealTimeTrending : UserControl, INotifyPropertyChanged
    {
        private double _lastLecture;
        private double _trend;
        private Brush _foregroundColor;
        private double _step;
        private FetchArgs _args;
        private IAvapiConnection _connection = AvapiConnection.Instance;
        private Task _backgroindWork;
        private DispatcherTimer timer;
        


        public RealTimeTrending()
        {
            InitializeComponent();

            var dayConfig = Mappers.Xy<DateTimePoint>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromMilliseconds(1000).Ticks)
                .Y(dayModel => dayModel.Value);

            _connection.Connect("5XQ6Y6JJKEOQ7JRU");
            _args = new FetchArgs { DefaultCurrency = "RSD", Symbol = "MSFT", RefreshRate = 5 };
            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Configuration = dayConfig,
                    AreaLimit = -10,
                    //Values = GetData(),
                    Values = new ChartValues<DateTimePoint>
                    {

                        new DateTimePoint{Value = 1, DateTime = DateTime.Now},
                        new DateTimePoint{Value = 5, DateTime = DateTime.Now.AddSeconds(1)},
                        new DateTimePoint{Value = 3, DateTime = DateTime.Now.AddSeconds(2)},
                        new DateTimePoint{Value = 4, DateTime = DateTime.Now.AddSeconds(3)},
                        new DateTimePoint{Value = 7, DateTime = DateTime.Now.AddSeconds(4)},
                        new DateTimePoint{Value = 2, DateTime = DateTime.Now.AddSeconds(5)},
                        new DateTimePoint{Value = 11, DateTime = DateTime.Now.AddSeconds(6)},
                        new DateTimePoint{Value = 6, DateTime = DateTime.Now.AddSeconds(7)},
                        new DateTimePoint{Value = 9, DateTime = DateTime.Now.AddSeconds(8)},
                        new DateTimePoint{Value = 1, DateTime = DateTime.Now.AddSeconds(9)},
                        new DateTimePoint{Value = 3, DateTime = DateTime.Now.AddSeconds(10)},
                        new DateTimePoint{Value = 9, DateTime = DateTime.Now.AddSeconds(11)},



                    }
                },


            };
            Step = 0;
            LastLecture = 0;

            //timer = new DispatcherTimer();
            //timer.Tick += new EventHandler(SetValues);
            //timer.Interval = new TimeSpan(0, 0, _args.RefreshRate);
            //timer.Start();

            Task.Run(() =>
            {
                ChartValues<DateTimePoint> values = new ChartValues<DateTimePoint>();
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                while (true)
                {

                    Thread.Sleep(4000);
                    _backgroindWork = Task.Factory.StartNew(() =>
                    {
                        values = GetData();
                    }, TaskCreationOptions.LongRunning);

                    //Dispatcher.Invoke(() => {
                    //    if (values.Count != 0)
                    //    {
                    //        LastHourSeries[0].Values = values;
                    //        Step = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value -
                    //                    ((ChartValues<DateModel>)LastHourSeries[0].Values).ElementAt(
                    //                        ((ChartValues<DateModel>)LastHourSeries[0].Values).Count - 2).Value;
                    //        if (Step > 0)
                    //            ForegroundColor = Brushes.Green;
                    //        else if (Step < 0)
                    //            ForegroundColor = Brushes.Red;
                    //        else
                    //            ForegroundColor = Brushes.Black;
                    //        LastLecture = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value;
                    //    }
                    //    MessageBox.Show("Refresh", "Refrash");
                    //});

                    _backgroindWork.ContinueWith(x =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            using (Dispatcher.DisableProcessing())
                            {
                                LastHourSeries[0].Values.Add(values.Last());
                                LastHourSeries[0].Values.RemoveAt(0);
                                //LastHourSeries[0].Values = values;
                            }
                        },DispatcherPriority.ContextIdle);
                        Step = ((ChartValues<DateTimePoint>)LastHourSeries[0].Values).Last().Value -
                                        ((ChartValues<DateTimePoint>)LastHourSeries[0].Values).ElementAt(
                                            ((ChartValues<DateTimePoint>)LastHourSeries[0].Values).Count - 2).Value;
                        if (Step > 0)
                            ForegroundColor = Brushes.Green;
                        else if (Step < 0)
                            ForegroundColor = Brushes.Red;
                        else
                            ForegroundColor = Brushes.Black;
                        LastLecture = ((ChartValues<DateTimePoint>)LastHourSeries[0].Values).Last().Value;


                        //MessageBox.Show("Refresh", "Refrash");
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }





                //while (true)
                //{
                //    Thread.Sleep(5000);
                //    Dispatcher.BeginInvoke(new Action(()=> {
                //        values = GetData();
                //    }), DispatcherPriority.Background);

                //    Application.Current.Dispatcher.Invoke(() => {
                //        if(values.Count != 0)
                //        {
                //            LastHourSeries[0].Values = values;
                //            Step = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value -
                //                        ((ChartValues<DateModel>)LastHourSeries[0].Values).ElementAt(
                //                            ((ChartValues<DateModel>)LastHourSeries[0].Values).Count - 2).Value;
                //            if (Step > 0)
                //                ForegroundColor = Brushes.Green;
                //            else if (Step < 0)
                //                ForegroundColor = Brushes.Red;
                //            else
                //                ForegroundColor = Brushes.Black;
                //            LastLecture = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value;
                //        }

                //    });

                //}

            });

            //Task.Run(() =>
            //{
            //    var r = new Random();
            //    while (true)
            //    {
            //        Thread.Sleep(5000);
            //        //var values = GetData();
            //        //foreach (var temp in GetData())
            //        //{
            //        //    Console.WriteLine(temp.Value + "     " + temp.DateTime.ToLongTimeString());
            //        //}

            //        //foreach( var temp in values)
            //        //{
            //        //    Thread.Sleep(1000);
            //        //    _trend = temp.Value;
            //        //    Application.Current.Dispatcher.Invoke(() =>
            //        //    {
            //        //        if (_trend > ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value)
            //        //            ForegroundColor = Brushes.Green;
            //        //        else if (_trend < ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value)
            //        //            ForegroundColor = Brushes.Red;
            //        //        else
            //        //            ForegroundColor = Brushes.Black;
            //        //        LastHourSeries[0].Values.Add(temp);
            //        //        LastHourSeries[0].Values.RemoveAt(0);
            //        //        SetLecture();
            //        //    });
            //        //}
            //        //Thread.Sleep(1000);
            //        _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);

            //        Application.Current.Dispatcher.Invoke(() =>
            //        {


            //            //LastHourSeries[0].Values.Add(new DateModel { Value = _trend, DateTime = DateTime.Now });
            //            //LastHourSeries[0].Values.RemoveAt(0);
            //            LastHourSeries[0].Values = new ChartValues<DateModel>
            //            {
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5), DateTime = DateTime.Now},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 2 : -2) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(1)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 3 : -3) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(2)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 4 : -4) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(3)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 5 : -5) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(4)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 6 : -6) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(5)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 7 : -7) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(6)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 8 : -8) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(7)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 9 : -9) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(8)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 10 : -10) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(9)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 11 : -11) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(10)},
            //            new DateModel{Value = (r.NextDouble() > 0.3 ? 12 : -12) * r.Next(0, 5), DateTime = DateTime.Now.AddSeconds(11)},
            //            };
            //            Step = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value -
            //                        ((ChartValues<DateModel>)LastHourSeries[0].Values).ElementAt(
            //                            ((ChartValues<DateModel>)LastHourSeries[0].Values).Count - 2).Value;
            //            if (_trend > 0)
            //                ForegroundColor = Brushes.Green;
            //            else if (_trend < 0)
            //                ForegroundColor = Brushes.Red;
            //            else
            //                ForegroundColor = Brushes.Black;
            //            LastLecture = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value;
            //            //SetLecture();
            //        });
            //    }
            //});

            Formatter = value => new DateTime((long)(value * TimeSpan.FromMilliseconds(1000).Ticks)).ToString("T");
            //Formatter = value => new DateTime((long)value).ToString("T");
            DataContext = this;
        }

        private void SetValues(object sender, EventArgs e)
        {
            var values = new ChartValues<DateTimePoint>();

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
                    //MessageBox.Show("Failed to fetch data", "Error");
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
                    int count = 12;
                    foreach (var timeseries in data.TimeSeries)
                    {
                        if (count <= 0)
                            break;
                        Console.WriteLine("open: " + timeseries.open);
                        Console.WriteLine("high: " + timeseries.high);
                        Console.WriteLine("low: " + timeseries.low);
                        Console.WriteLine("close: " + timeseries.close);
                        Console.WriteLine("volume: " + timeseries.volume);
                        Console.WriteLine("DateTime: " + timeseries.DateTime);
                        Console.WriteLine("========================");

                        values.Insert(0, new DateTimePoint
                        {
                            Value = double.Parse(timeseries.close),
                            DateTime = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                        });
                    }
                    LastHourSeries[0].Values = values;

                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Failed to send request", "Error");
            }

        }


        private ChartValues<DateTimePoint> GetData()
        {
            
            var values = new ChartValues<DateTimePoint>();

            //_backgroindWork = Task.Factory.StartNew(() =>
            //    {
            //        Int_TIME_SERIES_INTRADAY time_series_intraday =
            //    _connection.GetQueryObject_TIME_SERIES_INTRADAY();

            //        IAvapiResponse_TIME_SERIES_INTRADAY time_series_intradayResponse =
            //        time_series_intraday.Query(
            //             _args.Symbol,
            //             Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min,
            //             Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_outputsize.compact);

            //        var data = time_series_intradayResponse.Data;
            //        if (data.Error)
            //        {
            //            MessageBox.Show("Failed to fetch data", "Error");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Information: " + data.MetaData.Information);
            //            Console.WriteLine("Symbol: " + data.MetaData.Symbol);
            //            Console.WriteLine("LastRefreshed: " + data.MetaData.LastRefreshed);
            //            Console.WriteLine("Interval: " + data.MetaData.Interval);
            //            Console.WriteLine("OutputSize: " + data.MetaData.OutputSize);
            //            Console.WriteLine("TimeZone: " + data.MetaData.TimeZone);
            //            Console.WriteLine("========================");
            //            Console.WriteLine("========================");
            //            foreach (var timeseries in data.TimeSeries)
            //            {
            //                //Console.WriteLine("open: " + timeseries.open);
            //                //Console.WriteLine("high: " + timeseries.high);
            //                //Console.WriteLine("low: " + timeseries.low);
            //                //Console.WriteLine("close: " + timeseries.close);
            //                //Console.WriteLine("volume: " + timeseries.volume);
            //                //Console.WriteLine("DateTime: " + timeseries.DateTime);
            //                //Console.WriteLine("========================");

            //                values.Insert(0, new DateModel
            //                {
            //                    Value = double.Parse(timeseries.close),
            //                    DateTime = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            //                });
            //            }
            //        }
            //    }
            //    ,TaskCreationOptions.LongRunning
            //);


            //Dispatcher.BeginInvoke(new Action(()=>
            //    {
            //        Int_TIME_SERIES_INTRADAY time_series_intraday =
            //   _connection.GetQueryObject_TIME_SERIES_INTRADAY();

            //        IAvapiResponse_TIME_SERIES_INTRADAY time_series_intradayResponse =
            //        time_series_intraday.Query(
            //             _args.Symbol,
            //             Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min,
            //             Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_outputsize.compact);

            //        var data = time_series_intradayResponse.Data;
            //        if (data.Error)
            //        {
            //            MessageBox.Show("Failed to fetch data", "Error");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Information: " + data.MetaData.Information);
            //            Console.WriteLine("Symbol: " + data.MetaData.Symbol);
            //            Console.WriteLine("LastRefreshed: " + data.MetaData.LastRefreshed);
            //            Console.WriteLine("Interval: " + data.MetaData.Interval);
            //            Console.WriteLine("OutputSize: " + data.MetaData.OutputSize);
            //            Console.WriteLine("TimeZone: " + data.MetaData.TimeZone);
            //            Console.WriteLine("========================");
            //            Console.WriteLine("========================");
            //            foreach (var timeseries in data.TimeSeries)
            //            {
            //                //Console.WriteLine("open: " + timeseries.open);
            //                //Console.WriteLine("high: " + timeseries.high);
            //                //Console.WriteLine("low: " + timeseries.low);
            //                //Console.WriteLine("close: " + timeseries.close);
            //                //Console.WriteLine("volume: " + timeseries.volume);
            //                //Console.WriteLine("DateTime: " + timeseries.DateTime);
            //                //Console.WriteLine("========================");

            //                values.Insert(0, new DateModel
            //                {
            //                    Value = double.Parse(timeseries.close),
            //                    DateTime = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            //                });
            //            }
            //        }
            //    }
            //),DispatcherPriority.Background);

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
                    int count = 12;
                    foreach (var timeseries in data.TimeSeries)
                    {
                        if (count <= 0)
                            break;
                        Console.WriteLine("open: " + timeseries.open);
                        Console.WriteLine("high: " + timeseries.high);
                        Console.WriteLine("low: " + timeseries.low);
                        Console.WriteLine("close: " + timeseries.close);
                        Console.WriteLine("volume: " + timeseries.volume);
                        Console.WriteLine("DateTime: " + timeseries.DateTime);
                        Console.WriteLine("========================");

                        values.Insert(0, new DateTimePoint
                        {
                            Value = double.Parse(timeseries.close),
                            DateTime = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                        });
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Failed to send request", "Error");
            }

            
            return values;
        }

        public Func<double, string> Formatter { get; set; }


        public SeriesCollection LastHourSeries { get; set; }

        public Brush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                OnPropertyChanged("ForegroundColor");
            }
        }

        public string Currency
        {
            get { return _args.DefaultCurrency; }
            
        }

        public double Step
        {
            get { return _step; }
            set
            {
                _step = value;
                OnPropertyChanged("Step");
            }
        }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<DateTimePoint>)LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture) / 4;
            Step = target - _lastLecture;

            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private void UpdateOnclick(object sender, RoutedEventArgs e)
        //{
        //    TimePowerChart.Update(true);
        //}
    }
}
