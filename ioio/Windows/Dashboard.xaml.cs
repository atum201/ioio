using ioio.Common;
using ioio.ViewModels;
using System;

namespace ioio.Windows
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard
    {
        public Dashboard()
        {
            DataContext = new MainApplication();
            
            
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowUtil.SetVolumeDown(10);
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowUtil.SetVolumeUp(10);
        }

        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowUtil.SetVolume(80);
        }
    }
}
