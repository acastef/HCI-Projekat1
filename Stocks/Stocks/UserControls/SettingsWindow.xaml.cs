using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Stocks.FileManager;
using Stocks.Util;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private List<CheckBox> listOfCheckBoxes { get; set; }
        private LoadData data = new LoadData();

        public SettingsWindow()
        {
           
            DataContext = this;
            InitializeComponent();
            
            addDefaultCurrencies();
            addRefreshRates();
            DefaultCurrencies.SelectedIndex = Configuration.Instance.DefaultCurrencyIndex;
            RefreshRates.SelectedIndex = Configuration.Instance.RefreshRateIndex;
        }

        private void addRefreshRates()
        {
            List<ComboBoxItem> comboBoxes = new List<ComboBoxItem>();

            foreach (int i in Configuration.Instance.RefreshRateList)
            {
                ComboBoxItem cb = new ComboBoxItem();
                cb.Content = i + " minute(s)";
                comboBoxes.Add(cb);
            }

            RefreshRates.ItemsSource = comboBoxes;
        }

        private void addDefaultCurrencies()
        {
            List<ComboBoxItem> comboBoxes = new List<ComboBoxItem>();

            foreach (String s in Configuration.Instance.DefaultCurrenciesList)
            {
                ComboBoxItem cb = new ComboBoxItem();
                cb.Content = s;
                comboBoxes.Add(cb);
            }

            DefaultCurrencies.ItemsSource = comboBoxes;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            // gets default currency index
            Configuration.Instance.DefaultCurrencyIndex = DefaultCurrencies.SelectedIndex;
            // gets refresh rate index
            Configuration.Instance.RefreshRateIndex = RefreshRates.SelectedIndex;

            String returnText;
            if (data.saveSettings() == true)
                returnText = "Successfully saved";
            else
                returnText = "Something went wrong, your data can not be saved!\n Contact us for more information";

           
            TextBlock popupText = new TextBlock
            {
                Text = returnText,
                Background = Brushes.Turquoise,
                Foreground = Brushes.Black,
            };
            Popup msg = new Popup()
            {
                PlacementTarget = SaveButton,
                Placement = PlacementMode.Top,
                IsOpen = true,
                StaysOpen = false,
                HorizontalOffset = 5,
                VerticalOffset = -5,
                Width = SaveButton.Width * 2,

                Child = popupText
            };
        }
    }

   
}
