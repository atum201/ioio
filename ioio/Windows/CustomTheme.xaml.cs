using ioio.Controls;
using ioio.ViewModels;

namespace ioio.Windows
{
    /// <summary>
    /// Interaction logic for CustomTheme.xaml
    /// </summary>
    public partial class CustomTheme 
    {
        public CustomTheme()
        {
            DataContext = new MainApplication();
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.MainViewModel.OpenPlayer.Execute(null);
            this.Close();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] p = new object[] { };
            App.MainViewModel.OpenDashboardSetting.Execute(new object[] { new SettingAccount(),0,0,1,1});
            
            this.Close();
        }
    }
}
