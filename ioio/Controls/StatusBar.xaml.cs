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
using ioio.Common;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for StatusPanel.xaml
    /// </summary>
    public partial class StatusBar : UserControl
    {
        private int mouseSpeed = 10;
        private int volume = 50;
        public StatusBar()
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

        #region Event
        private void PoiterSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int.TryParse(e.NewValue.ToString(), out mouseSpeed);
            WindowUtil.SetMouseSpeed(mouseSpeed);
        }

        private void PoiterSpeed_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                PoiterSpeed.Value++;
            }

            else if (e.Delta < 0)
            {
                PoiterSpeed.Value--;
            }
        }

        private void PoiterVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int.TryParse(e.NewValue.ToString(), out volume);
            WindowUtil.SetVolume(volume);
        }

        private async void PoiterVoice_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                PoiterVoice.Value+=8;
            }

            else if (e.Delta < 0)
            {
                PoiterVoice.Value-=8;
            }
        }
        #endregion
    }
}
