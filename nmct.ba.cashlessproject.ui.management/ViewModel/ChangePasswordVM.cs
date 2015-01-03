using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
            set { _currentPassword = value; OnPropertyChanged("CurrentPassword"); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged("NewPassword"); }
        }

        private string _newPasswordAgain;

        public string NewPasswordAgain
        {
            get { return _newPasswordAgain; }
            set { _newPasswordAgain = value; OnPropertyChanged("NewPasswordAgain"); }
        }

        private string _errorSuccesfull;

        public string ErrorSuccesfull
        {
            get { return _errorSuccesfull; }
            set { _errorSuccesfull = value; OnPropertyChanged("ErrorSuccesfull"); }
        }

        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(ChangePassword); }
        }

        private async void ChangePassword()
        {
            if (CurrentPassword == null || NewPassword == null || NewPasswordAgain == null)
            {
                ErrorSuccesfull = "Gelieve alle velden in te vullen";
            } else if (NewPassword != NewPasswordAgain) {
                ErrorSuccesfull = "De twee velden voor het nieuwe wachtwoord zijn niet gelijk aan elkaar";

            //als het huidige wachtwoord correct is, wachtwoord wijzigen
            } else if (await passwordCorrect())
            {
                updatePassword();
                ErrorSuccesfull = "Wachtwoord gewijzigd";
            }
            else
            {
                ErrorSuccesfull = "Gelieve een correct huidig wachtwoord in te geven.";
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
