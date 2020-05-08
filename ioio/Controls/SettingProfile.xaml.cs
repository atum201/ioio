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
using XamlReader = System.Windows.Markup.XamlReader;
using System.IO;
using ioio.Models;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for SettingProfile.xaml
    /// </summary>
    public partial class SettingProfile : UserControl
    {
        public SettingProfile()
        {
            InitializeComponent();
            //var local = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "table.CT");
            //using (var fs = new System.IO.FileStream(local, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    try
            //    {
            //        //Read in ResourceDictionary File
            //        //object o = (CheatTable)XamlReader.Load(fs);
            //        //Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject(o));
                    
            //    }
            //    //catch (XamlParseException xx)
            //    //{
            //    //    if (xx.InnerException is XamlObjectWriterException inner && trial < 5)
            //    //        return LoadOrDefault(path, trial + 1, inner);

            //    //    resource = new ResourceDictionary();
            //    //}
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }
    }
}
