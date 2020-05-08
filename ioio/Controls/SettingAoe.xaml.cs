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
using ioio.Common;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for SettingAoe.xaml
    /// </summary>
    public partial class SettingAoe : UserControl
    {
        private int mouseSpeed = 10;
        private int volume = 50;
        public SettingAoe()
        {
            InitializeComponent();
            int.TryParse(WindowUtil.GetMouseSpeed().ToString(), out mouseSpeed);
            PoiterSpeed.Value = mouseSpeed;
            PoiterSpeed.Maximum = 20;
            PoiterSpeed.BeginInit();

            PoiterVoice.Maximum = 100;
            PoiterVoice.BeginInit();
            PoiterVoice.Value = WindowUtil.GetVolume();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(App.MainViewModel.Match.Victory);
            App.MainViewModel.Status = (int)Cat_AoeStatus.None;
        }

        private void PoiterSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void PoiterVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void PoiterAoeVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void PoiterLight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void startgame_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
