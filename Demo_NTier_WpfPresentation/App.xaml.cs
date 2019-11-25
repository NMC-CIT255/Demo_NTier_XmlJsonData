using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Demo_NTier_WpfPresentation.ViewModels;
using Demo_NTier_XmlJsonData.BusinessLayer;

namespace Demo_NTier_WpfPresentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FlintstoneCharacterBusiness fcBusiness = new FlintstoneCharacterBusiness();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(fcBusiness);

            MainWindowView mainWindowView = new MainWindowView();
            mainWindowView.DataContext = mainWindowViewModel;
            mainWindowView.Show();
        }
    }
}
