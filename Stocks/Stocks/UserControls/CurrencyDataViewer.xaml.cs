using Stocks.Util;
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

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for CurrencyDataViewer.xaml
    /// </summary>
    public partial class CurrencyDataViewer : UserControl
    {
        private String _id;
        private RealtimeViewer _realtime;
        private CurrencyHistoryTrending _currencyHistoryTrending;

        public CurrencyDataViewer()
        {
            InitializeComponent();
            Id = Configuration.Instance.Symbol;
            Title.Text = Configuration.Instance.FullName;
            //_realtime = new RealtimeViewer();
            _currencyHistoryTrending = new CurrencyHistoryTrending();
        }

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public RealtimeViewer RealTime
        {
            get { return _realtime; }
            set { _realtime = value; }
        }

        public CurrencyHistoryTrending CurrencyHisotryTrendind
        {
            get { return _currencyHistoryTrending; }
            set { _currencyHistoryTrending = value; }
        }
    }
}
