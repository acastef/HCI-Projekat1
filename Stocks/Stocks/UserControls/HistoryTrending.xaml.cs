using Avapi;
using Avapi.AvapiTIME_SERIES_DAILY;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
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
using Stocks.Util;
using Avapi.AvapiTIME_SERIES_MONTHLY_ADJUSTED;
using Avapi.AvapiTIME_SERIES_DAILY_ADJUSTED;
using Avapi.AvapiTIME_SERIES_WEEKLY_ADJUSTED;
using Avapi.AvapiCURRENCY_EXCHANGE_RATE;
using System.Windows.Controls.Primitives;

namespace Stocks.UserControls

{
    /// <summary>
    /// Interaction logic for HistoryTrending.xaml
    /// </summary>
    public partial class HistoryTrending : UserControl, INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;
        private string _title;
        private FetchArgs _args;
        private IAvapiConnection _connection = AvapiConnection.Instance;
        private double _exchangeRate = 1;
        private CompareGraph graph = Application.Current.Windows[1] as CompareGraph;
        private LineSeries temp;
        private Popup msg;
        private bool _added;

        public HistoryTrending()
        {
            InitializeComponent();
            temp = new LineSeries();

            _added = false;

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(64, 224, 208), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            _connection.Connect("5XQ6Y6JJKEOQ7JRU");
            _args = new FetchArgs
            {
                DefaultCurrency = Configuration.Instance.DefaultCurrency,
                Symbol = Configuration.Instance.Symbol,
                FullName = Configuration.Instance.FullName,
                RefreshRate = Configuration.Instance.RefreshRate * 60
            };

            XFormatter = val => new DateTime((long)val).ToString("dd MMM yyyy");
            YFormatter = val => val.ToString("0.##") + " " + _args.DefaultCurrency;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = GetData(),
                    Fill = gradientBrush,
                    StrokeThickness = 1,
                    PointGeometrySize = 0,
                    Title = _args.FullName,

                }
            };

            ZoomingMode = ZoomingOptions.Xy;

            Title = _args.FullName;
            
            DataContext = this;
        }


        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public String Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        


        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                OnPropertyChanged();
            }
        }

        private void ToogleZoomingMode(object sender, RoutedEventArgs e)
        {
            switch (ZoomingMode)
            {
                case ZoomingOptions.None:
                    ZoomingMode = ZoomingOptions.X;
                    break;
                case ZoomingOptions.X:
                    ZoomingMode = ZoomingOptions.Y;
                    break;
                case ZoomingOptions.Y:
                    ZoomingMode = ZoomingOptions.Xy;
                    break;
                case ZoomingOptions.Xy:
                    ZoomingMode = ZoomingOptions.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

       

        private ChartValues<DateTimePoint> GetData()
        {
                        
            Int_TIME_SERIES_DAILY time_series_daily =
                _connection.GetQueryObject_TIME_SERIES_DAILY();

            
            IAvapiResponse_TIME_SERIES_DAILY time_series_dailyResponse =
            time_series_daily.Query(
                 _args.Symbol,
                 Const_TIME_SERIES_DAILY.TIME_SERIES_DAILY_outputsize.compact);
            var values = new ChartValues<DateTimePoint>();

            var data = time_series_dailyResponse.Data;
            if (data.Error)
            {
                MessageBox.Show("Failed to fetch data", "Error");
            }
            else
            {

                if (_args.DefaultCurrency != "USD")
                {
                    try
                    {


                        Int_CURRENCY_EXCHANGE_RATE currency_exchange_rate = _connection.GetQueryObject_CURRENCY_EXCHANGE_RATE();
                        IAvapiResponse_CURRENCY_EXCHANGE_RATE currency_exchange_rateResponse =
                        currency_exchange_rate.QueryPrimitive("USD", _args.DefaultCurrency);
                        var data2 = currency_exchange_rateResponse.Data;
                        if (data2.Error)
                            MessageBox.Show("Failed to fetch data", "Error");
                        else
                        {
                            _exchangeRate = double.Parse(data2.ExchangeRate);

                            foreach (var timeseries in data.TimeSeries)
                            {

                                values.Add(new DateTimePoint(DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                    double.Parse(timeseries.close) * _exchangeRate));


                            }
                        }
                    }
                    catch (NullReferenceException)
                    {

                        MessageBox.Show("Failed to fetch currency exchange rate for chosen currency. Values will be show in USD", "Error");
                        _exchangeRate = 1;
                        foreach (var timeseries in data.TimeSeries)
                        {

                            values.Add(new DateTimePoint(DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                    double.Parse(timeseries.close) * _exchangeRate));


                        }
                        YFormatter = val => "$" + val.ToString("0.##");
                    }
                }
                else
                {
                    _exchangeRate = 1;
                    foreach (var timeseries in data.TimeSeries)
                    {

                        values.Add(new DateTimePoint(DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                double.Parse(timeseries.close) * _exchangeRate));


                    }
                }

               
            }

            return values;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            //Use the axis MinValue/MaxValue properties to specify the values to display.
            //use double.Nan to clear it.

            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }


        private async void MinHistory(object sender, RoutedEventArgs e)
        {
            
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_MONTHLY_ADJUSTED time_series_montly_adjusted = _connection.GetQueryObject_TIME_SERIES_MONTHLY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_MONTHLY_ADJUSTED time_series_weekly_adjustedResponse =
                await time_series_montly_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_weekly_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data","Error");
            else
            {
                foreach (var timeseries in data.TimeSeries)
                {
                    values.Add(new DateTimePoint(DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        double.Parse(timeseries.adjustedclose) * _exchangeRate));
                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);
        }

        private async void History5y(object sender, RoutedEventArgs e)
        {
            
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_MONTHLY_ADJUSTED time_series_montly_adjusted = _connection.GetQueryObject_TIME_SERIES_MONTHLY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_MONTHLY_ADJUSTED time_series_weekly_adjustedResponse =
                await time_series_montly_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_weekly_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
                DateTime offset = new DateTime(DateTime.Today.Year - 5, DateTime.Today.Month, DateTime.Today.Day);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if(temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }
                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);

        }

        private async void History2y(object sender, RoutedEventArgs e)
        {
           
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_WEEKLY_ADJUSTED time_series_weekly_adjusted = _connection.GetQueryObject_TIME_SERIES_WEEKLY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_WEEKLY_ADJUSTED time_series_weekly_adjustedResponse =
                await time_series_weekly_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_weekly_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
                DateTime offset = new DateTime(DateTime.Today.Year - 2, DateTime.Today.Month, DateTime.Today.Day);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }

                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);

        }

        private async void History1y(object sender, RoutedEventArgs e)
        {
            
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_WEEKLY_ADJUSTED time_series_weekly_adjusted = _connection.GetQueryObject_TIME_SERIES_WEEKLY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_WEEKLY_ADJUSTED time_series_weekly_adjustedResponse =
                await time_series_weekly_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_weekly_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
                DateTime offset = new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }

                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);
        }

        private async void History3m(object sender, RoutedEventArgs e)
        {
           
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjusted = _connection.GetQueryObject_TIME_SERIES_DAILY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjustedResponse =
                await time_series_daily_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_daily_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
                

                DateTime offset = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 3, DateTime.Today.Day);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }

                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);
        }

        private async void History1m(object sender, RoutedEventArgs e)
        {
            
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjusted = _connection.GetQueryObject_TIME_SERIES_DAILY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjustedResponse =
                await time_series_daily_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_daily_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
               

                DateTime offset = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }

                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);
        }

        private async void History10d(object sender, RoutedEventArgs e)
        {
           
            var values = new ChartValues<DateTimePoint>();
            Int_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjusted = _connection.GetQueryObject_TIME_SERIES_DAILY_ADJUSTED();
            IAvapiResponse_TIME_SERIES_DAILY_ADJUSTED time_series_daily_adjustedResponse =
                await time_series_daily_adjusted.QueryPrimitiveAsync(_args.Symbol);
            var data = time_series_daily_adjustedResponse.Data;
            if (data.Error)
                MessageBox.Show("Failed to fetch data", "Error");
            else
            {
                DateTime offset = DateTime.Now.AddDays(-10);
                DateTime temp;
                foreach (var timeseries in data.TimeSeries)
                {
                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (temp > offset)
                    {
                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.adjustedclose) * _exchangeRate));
                    }

                }
            }
            SeriesCollection[0].Values = values;
            ResetZoomOnClick(sender, e);
        }

        private void Add(object sender, RoutedEventArgs e)
        {

            if (_added)
            {
                msg = new Popup();
                TextBlock popupText = new TextBlock
                {
                    Text = "\n  Compare graph already cointains this item!   \n",
                    Background = Brushes.Turquoise,
                    Foreground = Brushes.Black,
                };

                msg.Child = popupText;
                
                msg.PlacementTarget = RemoveButton;
                msg.IsOpen = true;
                msg.StaysOpen = false;
            }
            else
            {
                if (_exchangeRate != 1)
                {
                    var values = new ChartValues<DateTimePoint>();
                    foreach (var value in SeriesCollection[0].Values)
                    {
                        var cast = (DateTimePoint)value;
                        values.Add(new DateTimePoint { Value = cast.Value / _exchangeRate, DateTime = cast.DateTime });
                    }
                    temp.Values = values;
                    temp.Title = SeriesCollection[0].Title;
                    graph.SeriesCollection.Add(temp);
                }
                else
                {
                    temp.Values = SeriesCollection[0].Values;
                    temp.Title = SeriesCollection[0].Title;
                    graph.SeriesCollection.Add(temp);
                }

                _added = true;
            }
            
        }

        public void Remove()
        {
            foreach(var item in graph.SeriesCollection)
            {
                var ls = item as LineSeries;
                if(ls.Title == Title)
                {
                    graph.SeriesCollection.Remove(item);
                    break;
                }
            }
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (!_added)
            {
                msg = new Popup();
                TextBlock popupText = new TextBlock
                {
                    Text = "\n  Compare graph does not cointain this item!  \n",
                    Background = Brushes.Turquoise,
                    Foreground = Brushes.Black,
                };

                msg.Child = popupText;

                msg.PlacementTarget = RemoveButton;
                msg.IsOpen = true;
                msg.StaysOpen = false;
            }
            else
            {
                Remove();
                _added = false;
            }
           
            
        }
        
    }

    public class ZoomingModeCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ZoomingOptions)value)
            {
                case ZoomingOptions.None:
                    return "None";
                case ZoomingOptions.X:
                    return "X";
                case ZoomingOptions.Y:
                    return "Y";
                case ZoomingOptions.Xy:
                    return "XY";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
