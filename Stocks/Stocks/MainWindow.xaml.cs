using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Stocks.FileManager;
using Stocks.Model;
using Stocks.UserControls;
using Stocks.Util;


namespace Stocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static AbstractData Stocks;
        private AbstractData Currencies;
        private AbstractData Cryptocurrency;
        private List<CheckBox> StockItems { get; set; }
        private List<CheckBox> DCurrenciesItems { get; set; }
        private List<CheckBox> CCurrenciesItems { get; set; }
        private List<CheckBox> _checkBoxList { get; set; }
        private CompareGraph graph = null;
        private int index = -1; // pozicija u historyTrendingu na kojoj se dodaje grafik

        // search field for stocks
        private string _searchText1;
        public string SearchText1
        {
            get { return _searchText1; }
            set
            {
                _searchText1 = value;

                OnPropertyChanged("SearchText1");
                OnPropertyChanged("FilterStockItems");
            }
        }

        // search field for DCurrencies
        private string _searchText2;
        public string SearchText2
        {
            get { return _searchText2; }
            set
            {
                _searchText2 = value;

                OnPropertyChanged("SearchText2");
                OnPropertyChanged("FilterDCurrenciesItems");
            }
        }

        // search field for CCurrencies
        private string _searchText3;
        public string SearchText3
        {
            get { return _searchText3; }
            set
            {
                _searchText3 = value;

                OnPropertyChanged("SearchText3");
                OnPropertyChanged("FilterCCurrenciesItems");
            }
        }

        // filters Stocks based on searchtext1 input field
        public IEnumerable<CheckBox> FilterStockItems
        {
            get
            {
                if (SearchText1 == null) return StockItems;

                return StockItems.Where(x => ((String)x.Content).ToUpper().StartsWith(SearchText1.ToUpper()));
            }
        }

        // filters DCurrencies based on searchtext2 input field
        public IEnumerable<CheckBox> FilterDCurrenciesItems
        {
            get
            {
                if (SearchText2 == null) return DCurrenciesItems;

                return DCurrenciesItems.Where(x => ((String)x.Content).ToUpper().StartsWith(SearchText2.ToUpper()));
            }
        }

        // filters CCurrencies based on searchtext3 input field
        public IEnumerable<CheckBox> FilterCCurrenciesItems
        {
            get
            {
                if (SearchText3 == null) return CCurrenciesItems;

                return CCurrenciesItems.Where(x => ((String)x.Content).ToUpper().StartsWith(SearchText3.ToUpper()));
            }
        }



        public MainWindow()
        {
            
            Load();
            
            InitializeComponent();

            StockItems = new List<CheckBox>();
            DCurrenciesItems = new List<CheckBox>();
            CCurrenciesItems = new List<CheckBox>();

            // Initialize Stocks
            foreach (ConcreteData stock in Stocks.ConcreteDataCollection)
            {
                //string merged = ;
                CheckBox cb = new CheckBox()
                {
                    Content = stock.Name + "(" + stock.Code + ")"
                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cb_Checked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cb_Unchecked));
                StockItems.Add(cb);
            }


            

            // Initialize Currencies
            foreach (ConcreteData stock in Currencies.ConcreteDataCollection)
            {
                
                CheckBox cb = new CheckBox()
                {
                    Content = stock.Name + "(" + stock.Code + ")"
                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cbCurrChecked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cbCurrUnchecked));
                DCurrenciesItems.Add(cb);
            }

            // Initialize Cryptocurrency
            foreach (ConcreteData stock in Cryptocurrency.ConcreteDataCollection)
            {
                CheckBox cb = new CheckBox()
                {
                    Content = stock.Name + "(" + stock.Code + ")"
                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cb_Checked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cb_Unchecked));
                CCurrenciesItems.Add(cb);
            }

            graph = new CompareGraph();
            graph.Show();
            

            DataContext = this;
        }

        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            Configuration.Instance.Type = TypeSeries.STOCK;
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            Configuration.Instance.Symbol = firstSplit.Substring(0, firstSplit.Length -1);
            Configuration.Instance.FullName = merged.Split('(')[0];
            DataViewer dw = new DataViewer();
            //dw.HistoryTrending.index = -1;
            DataContainer.Children.Add(dw);
        }
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            String symbol = firstSplit.Substring(0, firstSplit.Length - 1);

            foreach(DataViewer dv in DataContainer.Children)
            {
                if (dv.Id == symbol)
                {
                    DataContainer.Children.Remove(dv);
                    dv.HistoryTrending.Remove();
                    break;
                }
            }
        }



        private void cbCurrChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            Configuration.Instance.Type = TypeSeries.STOCK;
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            Configuration.Instance.Symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            Configuration.Instance.FullName = merged.Split('(')[0];
            DataContainer.Children.Add(new DataDigitalViewer(Configuration.Instance.Symbol));
        }
        private void cbCurrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            Configuration.Instance.Type = TypeSeries.STOCK;
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            String symbol = firstSplit.Substring(0, firstSplit.Length - 1);

            foreach (DataDigitalViewer dv in DataContainer.Children)
            {
                if (dv.Id == symbol)
                {
                    //dv
                    DataContainer.Children.Remove(dv);
                    break;
                }
            }
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
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    
}
