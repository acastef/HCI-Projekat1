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
    }

    
}
