using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LiveCharts.Wpf;
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
        public CompareGraph graph = null;
        private SettingsWindow settings = null;
        private LoadData Data = new LoadData();
        private Help helpWindow;
        private Configuration statusMessage;
        

        // message in status bar
        


        // search field for stocks
        private string _searchText1;
        public string SearchText1
        {
            get { return _searchText1; }
            set
            {
                _searchText1 = value;
                //StatusMessage = "Stocks filtered";
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
                Configuration.Instance.MessageText = "Digital currencies filtered";
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
                Configuration.Instance.MessageText = "Currencies filtered";
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
            statusMessage = Configuration.Instance;

            // Initialize Stocks
            foreach (String stockCode in Stocks.ConcreteDataCollection.Keys)
            {
                CheckBox cb = new CheckBox()
                {
                    Content = Stocks.ConcreteDataCollection[stockCode] + "(" + stockCode + ")",
                    Style = (Style)Application.Current.FindResource("CustomCheckBox")

                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cb_Checked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cb_Unchecked));
                StockItems.Add(cb);
            }




            // Initialize Currencies
            foreach (String currencyCode in Currencies.ConcreteDataCollection.Keys)
            {

                CheckBox cb = new CheckBox()
                {

                    Content = Currencies.ConcreteDataCollection[currencyCode] + "(" + currencyCode + ")",
                    Style = (Style)Application.Current.FindResource("CustomCheckBox")
                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cbDigCurrChecked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cbDigCurrUnchecked));
                DCurrenciesItems.Add(cb);
            }

            // Initialize Cryptocurrency
            foreach (String digCurrencyCode in Cryptocurrency.ConcreteDataCollection.Keys)
            {
                CheckBox cb = new CheckBox()
                {
                    Content = Cryptocurrency.ConcreteDataCollection[digCurrencyCode] + "(" + digCurrencyCode + ")",
                    Style = (Style)Application.Current.FindResource("CustomCheckBox")
                };

                cb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(cbCurrChecked));
                cb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(cbCurrUnchecked));
                CCurrenciesItems.Add(cb);
            }

            graph = new CompareGraph();
            //graph.Show();
            //Load_Help();
            //if (!Configuration.Instance.Help)
            //{
            //    helpWindow = new Help();
            //    helpWindow.Show();
            //}

            DataContext = this;
        }

        private void cbCurrChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            Configuration.Instance.Type = TypeSeries.CURRENCY;
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            Configuration.Instance.Symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            Configuration.Instance.FullName = merged.Split('(')[0];
            DataContainer.Children.Add(new CurrencyDataViewer());
            Configuration.Instance.MessageText = Configuration.Instance.FullName + " fetched!";
        }

        private void cbCurrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;


            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            String fullName = merged.Split('(')[0];
            String symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            CurrencyDataViewer temp;
            Configuration.Instance.MessageText = fullName + " removed!";


            foreach (var dv in DataContainer.Children)
            {
                try
                {
                    temp = (CurrencyDataViewer)dv;
                }
                catch (Exception)
                {
                    continue;
                }

                if (temp.Id == symbol)
                {

                    foreach (var item in graph.SeriesCollection)
                    {
                        var ls = item as LineSeries;
                        if (ls.Title == fullName)
                        {
                            graph.SeriesCollection.Remove(item);
                            break;
                        }
                    }

                    //temp.CurrencyHisotryTrendind.Remove();
                    DataContainer.Children.Remove(temp);
                    break;
                }
            }
        }


        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            Configuration.Instance.Symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            Configuration.Instance.FullName = merged.Split('(')[0];
            Configuration.Instance.Type = TypeSeries.STOCK;
            DataContainer.Children.Add(new DataViewer());
            Configuration.Instance.MessageText = Configuration.Instance.FullName + " fetched!";
        }
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;


            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            String fullName = merged.Split('(')[0];
            String symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            DataViewer temp;
            Configuration.Instance.MessageText = fullName + " removed!";
            foreach (var dv in DataContainer.Children)
            {
                try
                {
                    temp = (DataViewer)dv;
                }
                catch (Exception)
                {
                    continue;
                }

                if (temp.Id == symbol)
                {
                    foreach (var item in graph.SeriesCollection)
                    {
                        var ls = item as LineSeries;
                        if (ls.Title == fullName)
                        {
                            graph.SeriesCollection.Remove(item);
                            break;
                        }
                    }

                    //temp.HistoryTrending.Remove();
                    DataContainer.Children.Remove(temp);
                    break;
                }
            }
        }



        private void cbDigCurrChecked(object sender, RoutedEventArgs e)
        {
            
            CheckBox cb = (CheckBox)sender;

            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            Configuration.Instance.Symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            Configuration.Instance.FullName = merged.Split('(')[0];
            Configuration.Instance.Type = TypeSeries.DIGITAL_CURRENCY;
            DataContainer.Children.Add(new DataDigitalViewer());
            Configuration.Instance.MessageText = Configuration.Instance.FullName + " fetched!";
        }
        private void cbDigCurrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            Configuration.Instance.Type = TypeSeries.DIGITAL_CURRENCY;
            String merged = (String)cb.Content;
            String firstSplit = merged.Split('(')[1];
            String fullName = merged.Split('(')[0];
            String symbol = firstSplit.Substring(0, firstSplit.Length - 1);
            Configuration.Instance.MessageText = fullName + " removed!";
            DataDigitalViewer temp;
            foreach (var dv in DataContainer.Children)
            {
                try
                {
                    temp = (DataDigitalViewer)dv;
                }
                catch (Exception)
                {
                    continue;
                }

                if (temp.Id == symbol)
                {
                    foreach (var item in graph.SeriesCollection)
                    {
                        var ls = item as LineSeries;
                        if (ls.Title == fullName)
                        {
                            graph.SeriesCollection.Remove(item);
                            break;
                        }
                    }

                    //temp.DigitalCurrencyHisotryTrendind.Remove();
                    DataContainer.Children.Remove(temp);
                    break;
                }
            }
        }

        public void Load()
        {

            try
            {
                Stocks = Data.ReadStocks();
                Cryptocurrency = Data.ReadCriptoCurrencies();
                Currencies = Data.ReadCurrencies();

                //read metadata
                int[] meta = Data.ReadMetaData("meta_data.txt");
                Configuration.Instance.DefaultCurrencyIndex = meta[0];


                //read all deafult currencies in application
                Configuration.Instance.DefaultCurrenciesList = Data.ReadDefaultCurrencies("default_currencies.txt");
                //find which of default currencies is the default one (via index)
                Configuration.Instance.DefaultCurrency = Configuration.Instance.DefaultCurrenciesList[meta[0]];
                //get full name od default currency from all currencies in the app
                //Configuration.Instance.FullName = Currencies.ConcreteDataCollection[Configuration.Instance.Symbol]; 
                // read all real time parameters in application
                
                // you should have learned the pattern by now
                Configuration.Instance.RefreshRateList = Data.ReadRefreshRates("refresh_rate_list.txt");
                Configuration.Instance.RefreshRateIndex = meta[1];
                Configuration.Instance.RefreshRate = Configuration.Instance.RefreshRateList[meta[1]];

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

        // scrolling on mouse wheel
        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }


        // Action on "SettingsButton" click
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //StatusMessage = "Settings opened";
            settings = new SettingsWindow();
            settings.Show();
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            //StatusMessage = "Help window opened";
            helpWindow = new Help();
            helpWindow.Show();
        }

        private void ShowGlobalGraph(object sender, RoutedEventArgs e)
        {
            //StatusMessage = "Compare graph opened";
            this.graph.Show();
        }

    }


}
