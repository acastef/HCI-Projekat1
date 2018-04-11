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
            set { _id = value; }
        }

        public DataViewer(String symbol)
        {
            Id = symbol;
            InitializeComponent();
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


    }
}
