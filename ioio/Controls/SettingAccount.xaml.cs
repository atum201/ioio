using Amazon.CognitoIdentityProvider.Model;
using ioio.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ioio.Controls
{
    /// <summary>
    /// Interaction logic for SettingAccount.xaml
    /// </summary>
    public partial class SettingAccount : UserControl
    {
        public SettingAccount()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            CognitoHelper cognitoHelper = new CognitoHelper();
            
            ioioSetting.All.cognitoUser = await cognitoHelper.ValidateUser("atumm", "B@y7cgmc");
            Console.WriteLine(ioioSetting.All.cognitoUser.Username);
            string email = string.Empty;
            ioioSetting.All.cognitoUser.Attributes.TryGetValue("email", out email);
            GetUserResponse r = await cognitoHelper.GetUserInfo(ioioSetting.All.cognitoUser);
            Console.WriteLine(email);
        }
    }
}
