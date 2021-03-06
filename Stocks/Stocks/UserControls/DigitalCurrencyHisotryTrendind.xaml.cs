﻿using Avapi;
using Avapi.AvapiCURRENCY_EXCHANGE_RATE;
using Avapi.AvapiDIGITAL_CURRENCY_DAILY;
using Avapi.AvapiTIME_SERIES_DAILY;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Stocks.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for DigitalCurrencyHisotryTrendind.xaml
    /// </summary>
    public partial class DigitalCurrencyHisotryTrendind : UserControl, INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;
        private string _title;
        private FetchArgs _args;
        private double _exchangeRate = 1;
        private IAvapiConnection _connection = AvapiConnection.Instance;
        private CompareGraph graph = Application.Current.Windows[1] as CompareGraph;
        private LineSeries temp;
        private bool _added;
        private Popup msg;

        public DigitalCurrencyHisotryTrendind()
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
                RefreshRate = Configuration.Instance.RefreshRate
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

        private ChartValues<DateTimePoint> GetData()
        {
            var values = new ChartValues<DateTimePoint>();


            Int_DIGITAL_CURRENCY_DAILY digital_currency_daily =
                _connection.GetQueryObject_DIGITAL_CURRENCY_DAILY();
            try
            {
                IAvapiResponse_DIGITAL_CURRENCY_DAILY digital_currency_dailyResponse =
           digital_currency_daily.QueryPrimitive(_args.Symbol, _args.DefaultCurrency);

                var data = digital_currency_dailyResponse.Data;
                if (data.Error && _args.DefaultCurrency == "USD")
                {
                    MessageBox.Show("Failed to fetch data", "Error");
                    Read("\\" + _args.Symbol + ".csv");
                    try
                    {
                        return (ChartValues<DateTimePoint>)SeriesCollection[0].Values;
                    }
                    catch
                    {

                    }

                }
                else if (data.Error)
                {

                    digital_currency_dailyResponse = digital_currency_daily.QueryPrimitive(_args.Symbol, "USD");
                    var data1 = digital_currency_dailyResponse.Data;
                    if (data1.Error)
                    {
                        MessageBox.Show("Failed to fetch data", "Error");
                        values = Read("\\" + _args.Symbol + ".csv");
                        try
                        {
                            return (ChartValues<DateTimePoint>)SeriesCollection[0].Values;
                        }
                        catch (Exception)
                        {

                        }

                    }
                    else
                    {
                        try
                        {
                            Int_CURRENCY_EXCHANGE_RATE currency_exchange_rate = _connection.GetQueryObject_CURRENCY_EXCHANGE_RATE();
                            IAvapiResponse_CURRENCY_EXCHANGE_RATE currency_exchange_rateResponse =
                            currency_exchange_rate.QueryPrimitive("USD", _args.DefaultCurrency);
                            var data2 = currency_exchange_rateResponse.Data;
                            if (data2.Error)
                            {
                                MessageBox.Show("Failed to fetch exchange rate data for " + _args, "Error");
                            }

                            else
                            {
                                _exchangeRate = double.Parse(data2.ExchangeRate);
                                DateTime offset = DateTime.Now.AddYears(-1);
                                DateTime temp;
                                foreach (var timeseries in data1.TimeSeries)
                                {
                                    temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    if (temp > offset)
                                    {
                                        values.Add(new DateTimePoint(temp, double.Parse(timeseries.Close) * _exchangeRate));
                                    }

                                }
                                Write("\\" + _args.Symbol + ".csv", values);
                            }
                        }
                        catch (NullReferenceException)
                        {
                            MessageBox.Show("Failed to fetch currency exchange rate for " + _args.DefaultCurrency + " currency. Values will be show in USD", "Error");
                            DateTime offset = DateTime.Now.AddYears(-1);
                            DateTime temp;
                            _exchangeRate = 1;
                            foreach (var timeseries in data1.TimeSeries)
                            {
                                temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                if (temp > offset)
                                {
                                    values.Add(new DateTimePoint(temp, double.Parse(timeseries.Close)));
                                }

                            }
                            YFormatter = val => "$" + val.ToString("0.##");
                            Write("\\" + _args.Symbol + ".csv", values);
                        }


                    }
                }
                else
                {
                    DateTime offset = DateTime.Now.AddYears(-1);
                    DateTime temp;
                    _exchangeRate = 1;
                    foreach (var timeseries in data.TimeSeries)
                    {
                        temp = DateTime.ParseExact(timeseries.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        if (temp > offset)
                        {
                            values.Add(new DateTimePoint(temp, double.Parse(timeseries.Close)));
                        }

                    }
                    Write("\\" + _args.Symbol + ".csv", values);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                values = Read("\\" + _args.Symbol + ".csv");
                
            }
           
            //SeriesCollection[0].Title = data.MetaData.DigitalCurrencyName;
            return values;
        }


        private void Write(string pathFile, ChartValues<DateTimePoint> values)
        {
            string currentPaht = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = currentPaht + "\\Files\\" + _args.Symbol;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += pathFile;

            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var line in values)
                {
                    writer.WriteLine(line.DateTime.ToShortDateString() + "," + line.Value / _exchangeRate);
                }
            }
        }

        private ChartValues<DateTimePoint> Read(string filePath)
        {
            var temp = new ChartValues<DateTimePoint>();
            try
            {

                using (StreamReader reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                    "\\Files\\" + _args.Symbol + filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] token = line.Split(',');
                        temp.Add(new DateTimePoint
                        {
                            Value = double.Parse(token[1]) * _exchangeRate,
                            DateTime = DateTime.ParseExact(token[0], "dd-MMM-yy", CultureInfo.InvariantCulture)
                        });

                    }
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("Can not get data for " + _args.FullName + "!", "Error");
            }
            return temp;

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
            foreach (var item in graph.SeriesCollection)
            {
                var ls = item as LineSeries;
                if (ls.Title == Title)
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
}
