using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ItemSelection.xaml
    /// </summary>
    public partial class ItemSelection : UserControl, INotifyPropertyChanged
    {
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;

                OnPropertyChanged("SearchText");
                OnPropertyChanged("MyFilteredItems");
            }
        }

        public List<string> MyItems { get; set; }

        public IEnumerable<string> MyFilteredItems
        {
            get
            {
                Console.WriteLine(SearchText);
                if (SearchText == null) return MyItems;
                return MyItems.Where(x => x.ToUpper().StartsWith(SearchText.ToUpper()));
            }
        }

        public ItemSelection()
        {
            InitializeComponent();

            MyItems = new List<string>() { "ABC", "DEF", "GHI" };

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    
}
