using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class ChangePasswordVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Wachtwoord wijzigen"; }
        }

        private string _currentPassword;
        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { _currentPassword = value; OnPropertyChanged("Username"); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged("Password"); }
        }

        private string _newPasswordAgain;

        public string NewPasswordAgain
        {
            get { return _newPasswordAgain; }
            set { _newPasswordAgain = value; }
        }

        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(ChangePassword); }
        }

        private async void ChangePassword()
        {
            //als het huidige wachtwoord correct is, wachtwoord wijzigen
            if (await passwordCorrect())
            {
                updatePassword();
            }
        }

        private async Task<bool> passwordCorrect()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/organisation?oldpassword=" + CurrentPassword);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Boolean>(json);
                }
                else
                {
                    return false;
                }
            }
        }

        private async void updatePassword()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:55853/api/organisation?newpassword=" + NewPasswordAgain, new StringContent(NewPasswordAgain, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
            }
        }
    }
}
