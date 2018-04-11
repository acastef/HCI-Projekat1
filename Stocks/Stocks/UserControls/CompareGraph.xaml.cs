using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for CompareGraph.xaml
    /// </summary>
    public partial class CompareGraph : Window
    {

       
       

        public CompareGraph()
        {
            InitializeComponent();

            XFormatter = val => new DateTime((long)val).ToString("dd MMM yyyy");
            YFormatter = val => val.ToString("C");

            SeriesCollection = new SeriesCollection();

            DataContext = this;
        }


        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        
    }
}
