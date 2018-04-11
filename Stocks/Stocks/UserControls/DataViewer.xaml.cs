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
using System.ComponentModel;
using Stocks.Util;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for DataViewer.xaml
    /// </summary>
    public partial class DataViewer : UserControl
    {
        private String _id;
        private HistoryTrending _historyTrending;
        private RealtimeViewer _realtime;

        public String Id
        {
            get { return _id; }
            set {_id = value;}
        }

        public DataViewer()
        {
            
            InitializeComponent();
            Id = Configuration.Instance.Symbol;
            Title.Text = Configuration.Instance.FullName;
            _historyTrending = new HistoryTrending();
            _realtime = new RealtimeViewer();
        }

        
        public HistoryTrending HistoryTrending
        {
            get { return _historyTrending; }
            set { _historyTrending = value; }
        }



        public RealtimeViewer RealTime
        {
            get { return _realtime; }
            set { _realtime = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
