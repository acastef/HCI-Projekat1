using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Stocks.Util;
using Stocks.UserControls;

namespace Stocks
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    MainWindow mainWindow = new MainWindow();
        //    mainWindow.Hide();
        //    mainWindow.graph.Hide();
        //    Load_Help();
        //    if (!Configuration.Instance.Help)
        //    {

        //    }
        //    else
        //    {
        //        mainWindow.Show();
        //    }
           
        //}

        private void Load_Help()
        {
            string currentPaht = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = currentPaht + "\\Files\\help.txt";
            try
            {

                using (StreamReader reader = new StreamReader(path))
                {
                    Configuration.Instance.Help = bool.Parse(reader.ReadLine());
                }

            }
            catch (Exception)
            {

            }

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Load_Help();
            if (!Configuration.Instance.Help)
            {
                var help = new Help(mainWindow);
                help.Show();
            }
            else
            {
                Current.MainWindow = mainWindow;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
            }
        }
    }
}
