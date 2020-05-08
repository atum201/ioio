using System;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace ioio.Views
{
    public class ThemeColor
    {
        public ThemeColor(ITheme theme, string name)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            Name = name;
        }

        public Color SampleColor => Theme.PrimaryMid.Color;

        public string Name { get; }

        public ITheme Theme { get; }
    }
}
