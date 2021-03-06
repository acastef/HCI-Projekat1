﻿using Avapi;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Stocks.Model;
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
    /// Interaction logic for CurrencyHistoryTrending.xaml
    /// </summary>
    public partial class CurrencyHistoryTrending : UserControl, INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;
        private string _title;
        private FetchArgs _args;
        private double _exchangeRate = 1;
        private CompareGraph graph = Application.Current.Windows[1] as CompareGraph;
        private LineSeries temp;
        private bool _added;
        private Popup msg;

        public CurrencyHistoryTrending()
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

            //var dayConfig = Mappers.Xy<DateModel>()
            //    .X(dayModel => (double)dayModel.DateTime.Ticks)
            //    .Y(dayModel => dayModel.Value);

            _args = new FetchArgs
            {
                DefaultCurrency = Configuration.Instance.DefaultCurrency,
                Symbol = Configuration.Instance.Symbol,
                FullName = Configuration.Instance.FullName,
                RefreshRate = Configuration.Instance.RefreshRate
            };

            XFormatter = value => new DateTime((long)value).ToString("dd.MM.yyyy. HH:mm:ss");
           // XFormatter = value => new DateTime(Math.Max(0,((long)value / TimeSpan.FromSeconds(1).Ticks))).ToString("dd.MM.yyyy. HH:mm:ss");

            //XFormatter = val => new DateTime((long)Math.Max(0, val)).ToString("dd MM yyyy HH:mm:ss");
            YFormatter = val => val.ToString("0.##") + " " + _args.DefaultCurrency;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                   // Configuration = dayConfig,
                    //AreaLimit = -10,
                    Values = GetData(),
                    Fill = gradientBrush,
                    StrokeThickness = 1,
                    PointGeometrySize = 10,
                    Title = _args.FullName,

                }
            };

            ZoomingMode = ZoomingOptions.Xy;





            Title = _args.FullName;

            DataContext = this;
        }


    //    new DateTimePoint{Value = 1, DateTime = DateTime.Now
    //},
    //                    new DateTimePoint{Value = 5, DateTime = DateTime.Now.AddSeconds(1)},
    //                    new DateTimePoint{Value = 3, DateTime = DateTime.Now.AddSeconds(2)},
    //                    new DateTimePoint{Value = 4, DateTime = DateTime.Now.AddSeconds(3)},
    //                    new DateTimePoint{Value = 7, DateTime = DateTime.Now.AddSeconds(4)},
    //                    new DateTimePoint{Value = 2, DateTime = DateTime.Now.AddSeconds(5)},
    //                    new DateTimePoint{Value = 11, DateTime = DateTime.Now.AddSeconds(6)},
    //                    new DateTimePoint{Value = 6, DateTime = DateTime.Now.AddSeconds(7)},
    //                    new DateTimePoint{Value = 9, DateTime = DateTime.Now.AddSeconds(8)},
    //                    new DateTimePoint{Value = 1, DateTime = DateTime.Now.AddSeconds(9)},
    //                    new DateTimePoint{Value = 3, DateTime = DateTime.Now.AddSeconds(10)},
    //                    new DateTimePoint{Value = 9, DateTime = DateTime.Now.AddSeconds(11)},


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
            return Read();

        }

        private ChartValues<DateTimePoint> Read()
        {
            
            var temp = new ChartValues<DateTimePoint>
            {
                new DateTimePoint { Value = 0, DateTime = DateTime.Now },
                new DateTimePoint { Value = 0.2, DateTime = DateTime.Now.AddSeconds(10) }
            };
            try
            {

                using (StreamReader reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                    "\\Files\\Currencies\\" + _args.Symbol + ".csv"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "")
                        {
                            continue;
                        }
                        string[] token = line.Split(',');
                        var date = DateTime.ParseExact(token[0], "dd-MM-yyyy HH:MM:ss", CultureInfo.InvariantCulture);
                        temp.Add(new DateTimePoint
                        {
                            Value = double.Parse(token[1]) * _exchangeRate,
                            DateTime = date
                        });
                       
                       
                       

                    }
                }

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
                    var values = new ChartValues<LiveCharts.Defaults.DateTimePoint>();
                    foreach (var value in SeriesCollection[0].Values)
                    {
                        var cast = (LiveCharts.Defaults.DateTimePoint)value;
                        values.Add(new LiveCharts.Defaults.DateTimePoint { Value = cast.Value / _exchangeRate, DateTime = cast.DateTime });
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

        private void Refresh(object sender, RoutedEventArgs e)
        {
            SeriesCollection[0].Values = Read();
        }
    }
}
