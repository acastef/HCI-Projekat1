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

namespace Stocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private AbstractData Stocks;
        private AbstractData Currencies;
        private AbstractData Cryptocurrency;

        private List<CheckBox> checkBoxList = new List<CheckBox>();

        public MainWindow()
        {
            
            Load();
            
            InitializeComponent();

            List<ConcreteData> items = new List<ConcreteData>();

            foreach(ConcreteData data in Stocks.ConcreteDataCollection)
            {
                CheckBox cb = new CheckBox();
                cb.Content = data.Name;
                checkBoxList.Add(cb);
                items.Add(data);
            }

            foreach(CheckBox chBox in checkBoxList)
            {
                StockListContainer.Children.Add(chBox);
            }
            

            StockItemSel.MyItems = checkBoxList;


            var temp = new DataViewer();
            DataContainer.Children.Add(temp);
            DataContext = this;
        }

        public void Load()
        {
            LoadData Data = new LoadData();
            try
            {
                Stocks = Data.ReadStocks();
                Cryptocurrency = Data.ReadCriptoCurrencies();
                Currencies = Data.ReadCurrencies();
            }
            catch (Exception)
            {
                Console.WriteLine("Neuspesno ucitavanje fajla");
            }
        }
    }

    
}
