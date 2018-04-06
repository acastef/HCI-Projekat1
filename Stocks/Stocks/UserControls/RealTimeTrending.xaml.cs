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


        public RealTimeTrending()
        {
            InitializeComponent();

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromMilliseconds(1000).Ticks)
                .Y(dayModel => dayModel.Value);

            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Configuration = dayConfig,
                    AreaLimit = -10,
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},
                        new DateModel{Value = 0, DateTime = DateTime.Now},



                    }
                },


            };
            _trend = 8;

            Task.Run(() =>
            {
                var r = new Random();
                while (true)
                {
                    Thread.Sleep(1000);
                    _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (_trend > ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value)
                            ForegroundColor = Brushes.Green;
                        else if (_trend < ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value)
                            ForegroundColor = Brushes.Red;
                        else
                            ForegroundColor = Brushes.Black;
                        LastHourSeries[0].Values.Add(new DateModel { Value = _trend, DateTime = DateTime.Now });
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    });
                }
            });
            Formatter = value => new DateTime((long)(value * TimeSpan.FromMilliseconds(1000).Ticks)).ToString("T");

            DataContext = this;
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
            var target = ((ChartValues<DateModel>)LastHourSeries[0].Values).Last().Value;
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

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            TimePowerChart.Update(true);
        }
    }
}
