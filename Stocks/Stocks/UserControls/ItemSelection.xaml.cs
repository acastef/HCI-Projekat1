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
using Stocks.Model;

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

        public List<CheckBox> MyItems { get; set; }

        public List<CheckBox> MyFilteredItems
        {
            get;
            set;
            
        }

        public IEnumerable<CheckBox> getItems()
        {
            List<CheckBox> newItems = new List<CheckBox>();

            if (SearchText == null) return MyItems;

            //foreach (CheckBox cb in MyItems)
            //{
            //    String name = (String)cb.Content;
            //    //String a = "AB";
            //    if (name.ToUpper().StartsWith(SearchText.ToUpper()))
            //    {
            //        newItems.Add(cb);
            //    }
            //}

            //return newItems;

            return MyItems.Where(x => ((String)x.Content).ToUpper().StartsWith(SearchText.ToUpper()));
        }

        public ItemSelection()
        {
            InitializeComponent();

            //MyItems = new List<string>() { "ABC", "DEF", "GHI" };

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    
}
