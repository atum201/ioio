using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Media;
using ioio.Windows;
using Theme = MaterialDesignThemes.Wpf.Theme;
using System.Windows.Controls;

using System.Runtime.CompilerServices;
using System.IO;
using XamlReader = System.Windows.Markup.XamlReader;
using XamlWriter = System.Windows.Markup.XamlWriter;
using System.Xml;
using System.Text;



namespace ioio.Views
{
    internal class MainApplication : ViewModelBase
    {
        #region Varriable
        private readonly PaletteHelper _PaletteHelper = new PaletteHelper();
        public List<ThemeColor> ThemeColors { get; } = new List<ThemeColor>();
        private static ResourceDictionary _local;
        private static readonly ResourceDictionary Default;
        private bool _IsLightTheme = true;
        private Models.Match match;
        private int status = 0;
        public List<Models.Match> Matches = new List<Models.Match>();
        private string userId;
        #endregion
        #region Properties
        public int Status
        {
            get => status;
            set { Set(ref status, value); }
        }

        public Models.Match Match
        {
            get => match;
            set { Set(ref match, value); }
        }
        
        public bool IsLightTheme
        {
            get => _IsLightTheme;
            set
            {
                if (Set(ref _IsLightTheme, value))
                {
                    ITheme theme = _PaletteHelper.GetTheme();
                    theme.SetBaseTheme(value ? Theme.Light : Theme.Dark);
                    _PaletteHelper.SetTheme(theme);
                }
            }
        }
        public string UserID
        {
            get => userId;
            set { Set(ref userId, value); }
        }
        #region Command
        public ICommand SetThemeCommand { get; }
        public ICommand OpenDashboard { get; private set; }
        public ICommand OpenDashboardSetting { get; private set; }
        public ICommand OpenCustomTheme { get; private set; }
        public ICommand OpenPlayer { get; private set; }

        public ICommand OpenHome { get; private set; }
        public ICommand OpenProfile { get; private set; }
        public ICommand OpenMatch { get; private set; }
        public ICommand OpenNews { get; private set; }
        public ICommand OpenComponent { get; private set; }
        #endregion
        //The starting value of this must match which resource dictionary we loaded in the App.xaml, in this case the light MDIX theme
        #endregion
        #region Constructure
        public MainApplication()
        {
            ThemeColors.AddRange(Enum.GetNames(typeof(MaterialDesignColor))
                .Where(x => Enum.TryParse<MaterialDesignColor>($"{x}Secondary", out _))
                .Select(x =>
                {
                    var primary = (MaterialDesignColor)Enum.Parse(typeof(MaterialDesignColor), x);
                    var secondary = (MaterialDesignColor)Enum.Parse(typeof(MaterialDesignColor), $"{x}Secondary");
                    Theme theme = Theme.Create(Theme.Light,
                        SwatchHelper.Lookup[primary],
                        SwatchHelper.Lookup[secondary]);
                    return new ThemeColor(theme, x);
                }));

            ThemeColors.Add(new ThemeColor(GetThemeFromCode(), "Fire and Ice"));
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/ioio;component/Resources/CustomXamlTheme.xaml")
            };
            IThemeManager themeManager = System.Windows.Application.Current.Resources.GetThemeManager();
            //themeManager.ThemeChanged += ThemeManagerOnThemeChanged;

            ThemeColors.Add(new ThemeColor(GetThemeFromDictionary(rd), "Jungle"));


            SetThemeCommand = new RelayCommand<ThemeColor>(OnSetTheme);

            OpenDashboard = new RelayCommand(openDashboard);
            OpenDashboardSetting = new RelayCommand<object>(openDashboardContent);
            OpenCustomTheme = new RelayCommand(openCustomTheme);
            OpenPlayer = new RelayCommand(openPlayer);

            OpenHome = new RelayCommand(openHome);
            OpenProfile = new RelayCommand<string>(openProfile);
            OpenMatch = new RelayCommand<string>(openMatch);
            OpenNews = new RelayCommand(openNews);
            OpenComponent = new RelayCommand(openComponent);

            Match = new Models.Match();
        }
        #endregion
        #region Method
        public void LoadMap()
        {
            // for all type of resource, get code, AoBScan to get Resource. then add Resource to Match.Map
            Dictionary<string, string> resource = new Dictionary<string, string> ();
        }

        public void StoreAndRefreshMatch()
        {
            if (Matches == null)
                Matches = new List<Models.Match>();
            Matches.Add(Match);
            Match = new Models.Match();
        }

        #region Command

        private void openDashboard()
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                window.Closed += (sender, args) => { CloseOrNot(); };

                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
        }
        private void openDashboardContent(object c)
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                Grid content = window.FindName("Content") as Grid;
                if (c.GetType() == typeof(object[]))
                {
                    object[] cc = c as object[];
                    UIElement control = cc[0] as UIElement;
                    Grid.SetColumn(control, cc.Length > 1 ? (int)cc[1] : 0);
                    Grid.SetRow(control, cc.Length > 2 ? (int)cc[2] : 0);
                    Grid.SetColumnSpan(control, cc.Length > 3 ? (int)cc[3] : 3);
                    Grid.SetRowSpan(control, cc.Length > 4 ? (int)cc[4] : 3);

                    content.Children.Add(control);
                }
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
        }
        private void openCustomTheme()
        {
            var window = Application.Current.Windows.OfType<CustomTheme>().FirstOrDefault();
            if (window == null)
            {
                window = new CustomTheme();
                window.Closed += (sender, args) => { CloseOrNot(); };

                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
        }
        private void openPlayer()
        {
            var window = Application.Current.Windows.OfType<Player>().FirstOrDefault();
            if (window == null)
            {
                window = new Player();
                window.Closed += (sender, args) => { CloseOrNot(); };

                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
        }
        private void OnSetTheme(ThemeColor themeVm)
        {
            if (themeVm == null) throw new ArgumentNullException(nameof(themeVm));

            ITheme theme = themeVm.Theme;
            theme.SetBaseTheme(IsLightTheme ? Theme.Light : Theme.Dark);
            _PaletteHelper.SetTheme(theme);
        }

        private void openHome()
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
            Grid content = window.FindName("Content") as Grid;
            content.Children.Clear();
            ioio.Component.Home.Home home = new Component.Home.Home();
            Grid.SetColumn(home, 0);
            Grid.SetRow(home, 0);
            Grid.SetColumnSpan(home, 3);
            Grid.SetRowSpan(home, 3);
            content.Children.Add(home);
        }
        private void openProfile(string id)
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
            Grid content = window.FindName("Content") as Grid;
            content.Children.Clear();
            ioio.Component.Profile.Profile profile = new Component.Profile.Profile(id);
            Grid.SetColumn(profile, 0);
            Grid.SetRow(profile, 0);
            Grid.SetColumnSpan(profile, 3);
            Grid.SetRowSpan(profile, 3);
            content.Children.Add(profile);
        }

        private void openMatch(string id)
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
            Grid content = window.FindName("Content") as Grid;
            content.Children.Clear();
            ioio.Component.Match.Match match = new Component.Match.Match(id);
            Grid.SetColumn(match, 0);
            Grid.SetRow(match, 0);
            Grid.SetColumnSpan(match, 3);
            Grid.SetRowSpan(match, 3);
            content.Children.Add(match);
        }

        private void openNews()
        {
            var window = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (window == null)
            {
                window = new Dashboard();
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
            Grid content = window.FindName("Content") as Grid;
            content.Children.Clear();
            ioio.Component.News.News news = new Component.News.News();
            Grid.SetColumn(news, 0);
            Grid.SetRow(news, 0);
            Grid.SetColumnSpan(news, 3);
            Grid.SetRowSpan(news, 3);
            content.Children.Add(news);
        }
        private void openComponent()
        {
            var window = Application.Current.Windows.OfType<Basic>().FirstOrDefault();
            if (window == null)
            {
                window = new Basic();
                window.Closed += (sender, args) => { CloseOrNot(); };
                window.Show();
            }
            else
            {
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
            }
            Grid content = window.FindName("Content") as Grid;
            content.Children.Clear();
            ioio.Component.Component control = new Component.Component();
            Grid.SetColumn(control, 0);
            Grid.SetRow(control, 0);
            Grid.SetColumnSpan(control, 3);
            Grid.SetRowSpan(control, 3);
            content.Children.Add(control);
        }
        #endregion
        #region Local ResourceDictionary
        public static void Save()
        {
            //Only writes if there's something changed. Should not write the default dictionary.
            if (_local == null)
                return;

            //Filename: Local or AppData.
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.xaml");

            #region Create folder

            var folder = Path.GetDirectoryName(filename);

            if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            #endregion

            try
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    CheckCharacters = true,
                    CloseOutput = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    Encoding = Encoding.UTF8,
                };

                using (var writer = XmlWriter.Create(filename, settings))
                    XamlWriter.Save(_local, writer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static object GetValue([CallerMemberName] string key = "", object defaultValue = null)
        {
            if (Default == null)
                return defaultValue;

            if (Application.Current == null || Application.Current.Resources == null)
                return Default[key];

            if (Application.Current.Resources.Contains(key))
                return Application.Current.Resources[key];

            return Default[key] ?? defaultValue;

        }

        private static void SetValue(object value, [CallerMemberName] string key = "")
        {
            //Updates or inserts the value to the Local resource.
            if (_local != null)
            {
                if (_local.Contains(key))
                {
                    _local[key] = value;

                    //If the value is being set to null, remove it.
                    if (value == null && (!Default.Contains(key) || Default[key] == null))
                        _local.Remove(key);
                }
                else
                {
                    _local.Add(key, value);
                }
            }


            //Updates/Adds the current value of the resource.
            if (Application.Current.Resources.Contains(key))
                Application.Current.Resources[key] = value;
            else
                Application.Current.Resources.Add(key, value);

        }
        #endregion
        #region Theme Window
        private void ThemeManagerOnThemeChanged(object sender, ThemeChangedEventArgs e)
        {

        }

        

        private static ITheme GetThemeFromDictionary(ResourceDictionary resourceDictionary)
        {
            var theme = new Theme();
            theme.SetBaseTheme(Theme.Light);

            theme.PrimaryLight = new ColorPair(GetColor("PrimaryHueLight"), GetColor("PrimaryHueLightForeground"));
            theme.PrimaryMid = new ColorPair(GetColor("PrimaryHueMid"), GetColor("PrimaryHueMidForeground"));
            theme.PrimaryDark = new ColorPair(GetColor("PrimaryHueDark"), GetColor("PrimaryHueDarkForeground"));
            
            theme.SecondaryLight = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));
            theme.SecondaryMid = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));
            theme.SecondaryDark = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));

            

            return theme;

            //return new Theme(name)
            //{
            //    NB: The names of these hues need to be set to these values to match what is in the MDIX themes
            //    In addition these names are also used in the App.xaml to map to the MahApps brushes
            //    PrimaryLightHue = new Hue("Primary200", GetColor("PrimaryHueLight"), GetColor("PrimaryHueLightForeground")),
            //    PrimaryMidHue = new Hue("Primary500", GetColor("PrimaryHueMid"), GetColor("PrimaryHueMidForeground")),
            //    PrimaryDarkHue = new Hue("Primary700", GetColor("PrimaryHueDark"), GetColor("PrimaryHueDarkForeground")),
            //    SecondaryAccentHue = new Hue("Accent700", GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground")),
            //};

            Color GetColor(string key)
            {
                var rv = resourceDictionary[key];
                if (rv == null) throw new InvalidOperationException($"Could not find '{key}' in resource dictionary");
                if (rv is Color color)
                {
                    return color;
                }

                throw new InvalidOperationException($"'{key}' is not a {nameof(Color)}");
            }
        }

        private static ITheme GetThemeFromCode()
        {
            var theme = new Theme();
            theme.SetBaseTheme(Theme.Light);

            theme.PrimaryLight = new ColorPair(Colors.LightSalmon);
            theme.PrimaryMid = new ColorPair(Colors.Orange);
            theme.PrimaryDark = new ColorPair(Colors.Red);

            theme.SecondaryLight = new ColorPair(Colors.DeepSkyBlue, Colors.Green);
            theme.SecondaryMid = new ColorPair(Colors.DeepSkyBlue, Colors.Green);
            theme.SecondaryDark = new ColorPair(Colors.DeepSkyBlue, Colors.Green);

            theme.Background = Colors.Red;
            theme.Body = Colors.Blue;
            
            return theme;
            
            //return new Theme("Fire and Ice")
            //{
            //    //NB: The names of these hues need to be set to these values to match what is in the MDIX themes
            //    //In addition these names are also used in the App.xaml to map to the MahApps brushes
            //    PrimaryLightHue = new Hue("Primary200", Colors.LightSalmon, Colors.Black),
            //    PrimaryMidHue = new Hue("Primary500", Colors.Orange, Colors.Black),
            //    PrimaryDarkHue = new Hue("Primary700", Colors.Red, Colors.White),
            //    SecondaryAccentHue = new Hue("Accent700", Colors.DeepSkyBlue, Colors.Green)
            //};
        }

        
        private void CloseOrNot()
        {
            //When closed, check if it's the last window, then close if it's the configured behavior.
            //if (!UserSettings.All.ShowNotificationIcon || !UserSettings.All.KeepOpen)
            //{
            //We only need to check loaded windows that have content
            if (Application.Current.Windows.Cast<Window>().Count(window => window.HasContent) == 0)
                Application.Current.Shutdown(2);
            //}
        }
        #endregion
        #endregion
    }
}
