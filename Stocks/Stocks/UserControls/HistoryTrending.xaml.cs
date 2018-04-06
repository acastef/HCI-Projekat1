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

namespace Stocks.UserControls

{
    /// <summary>
    /// Interaction logic for HistoryTrending.xaml
    /// </summary>
    public partial class HistoryTrending : UserControl, INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;

        public HistoryTrending()
        {
            InitializeComponent();

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = GetData(),
                    Fill = gradientBrush,
                    StrokeThickness = 1,
                    PointGeometrySize = 0
                }
            };

            ZoomingMode = ZoomingOptions.X;

            XFormatter = val => new DateTime((long)val).ToString("dd MMM yyyy");
            YFormatter = val => val.ToString("C");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

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
             // Creating the connection object
            IAvapiConnection connection = AvapiConnection.Instance;

            // Set up the connection and pass the API_KEY provided by alphavantage.co
            connection.Connect("5XQ6Y6JJKEOQ7JRU");

            // Get the TIME_SERIES_DAILY query object
            Int_TIME_SERIES_DAILY time_series_daily =
                connection.GetQueryObject_TIME_SERIES_DAILY();

            // Perform the TIME_SERIES_DAILY request and get the result
            IAvapiResponse_TIME_SERIES_DAILY time_series_dailyResponse =
            time_series_daily.Query(
                 "MSFT",
                 Const_TIME_SERIES_DAILY.TIME_SERIES_DAILY_outputsize.compact);


            // Printout the results
            //Console.WriteLine("******** RAW DATA TIME_SERIES_DAILY ********");
            //Console.WriteLine(time_series_dailyResponse.RawData);

            //Console.WriteLine("******** STRUCTURED DATA TIME_SERIES_DAILY ********");

            var values = new ChartValues<DateTimePoint>();

            var data = time_series_dailyResponse.Data;
            if (data.Error)
            {
                Console.WriteLine(data.ErrorMessage);
            }
            else
            {
                //Console.WriteLine("Information: " + data.MetaData.Information);
                //Console.WriteLine("Symbol: " + data.MetaData.Symbol);
                //Console.WriteLine("LastRefreshed: " + data.MetaData.LastRefreshed);
                //Console.WriteLine("OutputSize: " + data.MetaData.OutputSize);
                //Console.WriteLine("TimeZone: " + data.MetaData.TimeZone);
                //Console.WriteLine("========================");
                //Console.WriteLine("========================");
                foreach (var timeseries in data.TimeSeries)
                {
                    //Console.WriteLine("open: " + timeseries.open);
                    //Console.WriteLine("high: " + timeseries.high);
                    //Console.WriteLine("low: " + timeseries.low);
                    //Console.WriteLine("close: " + timeseries.close);
                    //Console.WriteLine("volume: " + timeseries.volume);
                    //Console.WriteLine("DateTime: " + timeseries.DateTime);
                    //Console.WriteLine("========================");

                    values.Add(new DateTimePoint(DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        double.Parse(timeseries.close)));

                }
            }
            
            

            //for (var i = 0; i < 100; i++)
            //{
            //    var seed = r.NextDouble();
            //    if (seed > .8) trend += seed > .9 ? 50 : -50;
            //    values.Add(new DateTimePoint(DateTime.Now.AddDays(i), trend + r.Next(0, 10)));
            //}

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
