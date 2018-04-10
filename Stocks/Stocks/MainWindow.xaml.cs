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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stocks.FileManager;
using Stocks.Model;
using Stocks.UserControls;
using Stocks.Util;

namespace Stocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Load();
            
            InitializeComponent();
        }

        public void Load()
        {
            LoadData Data = new LoadData();
            try
            {
                AbstractData Stocks = Data.ReadStocks();
                AbstractData CriptroCurrencies = Data.ReadCriptoCurrencies();
                AbstractData Currencies = Data.ReadCurrencies();
            }
            catch (Exception)
            {
                Console.WriteLine("Neuspesno ucitavanje fajla");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.STOCK;
            Configuration.Instance.Symbol = "MSFT";
            Configuration.Instance.FullName = "Microsoft";
            dataContainer.Children.Add(new DataViewer());
            CompareGraph graph = new CompareGraph();
            
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.DIGITAL_CURRENCY;
            Configuration.Instance.Symbol = "BTC";
            Configuration.Instance.FullName = "Bitcoin";
            dataContainer.Children.Add(new DataDigitalViewer());
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.STOCK;
            Configuration.Instance.Symbol = "AAPL";
            Configuration.Instance.FullName = "Apple";
            dataContainer.Children.Add(new DataViewer());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.STOCK;
            Configuration.Instance.Symbol = "FB";
            Configuration.Instance.FullName = "Facebook";
            dataContainer.Children.Add(new DataViewer());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.DIGITAL_CURRENCY;
            Configuration.Instance.Symbol = "ETH";
            Configuration.Instance.FullName = "Ethereum";
            dataContainer.Children.Add(new DataDigitalViewer());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.Type = TypeSeries.DIGITAL_CURRENCY;
            Configuration.Instance.Symbol = "XZC";
            Configuration.Instance.FullName = "Zcoin";
            dataContainer.Children.Add(new DataDigitalViewer());
        }
    }

    
}
