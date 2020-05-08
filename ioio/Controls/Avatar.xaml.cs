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

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for Avatar.xaml
    /// </summary>
    public partial class Avatar : UserControl
    {
        public static readonly DependencyProperty UserIDProperty =
                DependencyProperty.Register
                (
                    "UserID", typeof(string), typeof(Avatar),
                    new UIPropertyMetadata(string.Empty)
                );

        public string UserID
        {
            get
            {
                return (string)GetValue(UserIDProperty);
            }
            set
            {
                SetValue(UserIDProperty, value);
            }
        }
        public Avatar()
        {
            InitializeComponent();
        }
    }
}
