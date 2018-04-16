using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;
using Stocks.Util;

namespace Stocks.UserControls
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        private MainWindow _mainWindow;

        private int _index = 0;



        public Help()
        {
            InitializeComponent();
            base.Closing += SetConfiguration;
            CheckBox.IsChecked = Configuration.Instance.Help;
            PngBitmapDecoder dekoder = new PngBitmapDecoder(Images.bitmapImages.ElementAt(_index), BitmapCreateOptions.PreservePixelFormat,
              BitmapCacheOption.Default);
            BitmapSource source = dekoder.Frames[0];
            help_Image.Source = source;
            help_Image.Stretch = Stretch.Uniform;
            if(Images.bitmapImages.Count == 1)
            {
                Next.IsEnabled = false;
            }
            Previous.IsEnabled = false;

        }

        public Help(MainWindow mainWindow)
        {
            InitializeComponent();
            base.Closing += SetConfiguration;
            CheckBox.IsChecked = Configuration.Instance.Help;
            _mainWindow = mainWindow;
            PngBitmapDecoder dekoder = new PngBitmapDecoder(Images.bitmapImages.ElementAt(_index), BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            BitmapSource source = dekoder.Frames[0];
            help_Image.Source = source;
            help_Image.Stretch = Stretch.Uniform;
            if (Images.bitmapImages.Count == 1)
            {
                Next.IsEnabled = false;
            }
            Previous.IsEnabled = false;

        }

        void SetConfiguration(object sender, CancelEventArgs e)
        {
            Write();
            if(_mainWindow != null)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.MainWindow = _mainWindow;
                _mainWindow.Show();
                
            }
            
        }

      

        private void Write()
        {
            string currentPaht = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = currentPaht + "\\Files\\help.txt";
            

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(CheckBox.IsChecked.ToString());
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            Configuration.Instance.Help = (bool)checkBox.IsChecked;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ++_index;
            var button = sender as Button;
            if(_index == Images.bitmapImages.Count-1)
            {
                Next.IsEnabled = false; ;
                Previous.IsEnabled = true;
            }
            else
            { 
                Next.IsEnabled = true;
                Previous.IsEnabled = true;
            }
            PngBitmapDecoder dekoder = new PngBitmapDecoder(Images.bitmapImages.ElementAt(_index), BitmapCreateOptions.PreservePixelFormat,
               BitmapCacheOption.Default);
            BitmapSource source = dekoder.Frames[0];
            help_Image.Source = source;
            help_Image.Stretch = Stretch.Uniform;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            --_index;
            if (_index == 0)
            {
                Previous.IsEnabled = false;
                Next.IsEnabled = true;
            }
            else
            {
                Next.IsEnabled = true;
                Previous.IsEnabled = true;
            }
            PngBitmapDecoder dekoder = new PngBitmapDecoder(Images.bitmapImages.ElementAt(_index), BitmapCreateOptions.PreservePixelFormat,
               BitmapCacheOption.Default);
            BitmapSource source = dekoder.Frames[0];
            help_Image.Source = source;
            help_Image.Stretch = Stretch.Uniform;
        }
    }
}
