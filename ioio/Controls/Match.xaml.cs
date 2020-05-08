using ioio.ViewModels;
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

using ioio.Models;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for Match.xaml
    /// </summary>
    public partial class Match : UserControl
    {
        public Match()
        {
            InitializeComponent();
            
            Models.Match m = ((MainApplication)FindResource("AppViewModel")).Match;
            Console.WriteLine(m.Status);
            DataContext = m;
        }
    }
}
