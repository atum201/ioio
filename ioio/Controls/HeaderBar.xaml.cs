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
using ioio.Windows;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for HeaderPanel.xaml
    /// </summary>
    public partial class HeaderBar : UserControl
    {
        public HeaderBar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.OpenCustomTheme.Execute(null);
            var window = Application.Current.Windows.OfType<Basic>().FirstOrDefault();
            window.Close();
        }
    }
}
