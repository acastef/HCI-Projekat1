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
using Stocks.Util;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for DataDigitalViewer.xaml
    /// </summary>
    public partial class DataDigitalViewer : UserControl
    {
        private String _id;
        private RealtimeViewer _realtime;
        private DigitalCurrencyHisotryTrendind _digitalCurrencyHisotryTrendind;

        public DataDigitalViewer()
        {
            
            InitializeComponent();
            Id = Configuration.Instance.Symbol;
            Title.Text = Configuration.Instance.FullName;
            _realtime = new RealtimeViewer();
            _digitalCurrencyHisotryTrendind = new DigitalCurrencyHisotryTrendind();

        }

        public String Id
        {
            get { return _id; }
            set {_id = value;}
        }

        public DigitalCurrencyHisotryTrendind DigitalCurrencyHisotryTrendind
        {
            get { return _digitalCurrencyHisotryTrendind; }
            set { _digitalCurrencyHisotryTrendind = value; }
        }


        public RealtimeViewer RealTime
        {
            get { return _realtime; }
            set { _realtime = value; }
        }


       
    }
}
