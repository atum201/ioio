using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ioio.Common
{
    internal sealed class Store : INotifyPropertyChanged
    {
        #region variables
        private static readonly ResourceDictionary Default; // user for load local setting user
        // Info cache when process
        private static ResourceDictionary _user;// cache user
        private static ResourceDictionary _matchs; // cache match
        private static ResourceDictionary _local;


        public event PropertyChangedEventHandler PropertyChanged;
        public static Store All { get; } = new Store();
        #endregion
        static Store()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
        }

        #region Properties

        #endregion

        #region Method
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

            All.OnPropertyChanged(key);
        }

        private static object GetUserValue([CallerMemberName] string key = "", object defaultValue = null)
        {
            if (Default == null)
                return defaultValue;

            if (Application.Current == null || Application.Current.Resources == null)
                return Default[key];

            if (Application.Current.Resources.Contains(key))
                return Application.Current.Resources[key];

            return Default[key] ?? defaultValue;
        }

        private static void SetUserValue(object value, [CallerMemberName] string key = "")
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

            All.OnPropertyChanged(key);
        }


        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
