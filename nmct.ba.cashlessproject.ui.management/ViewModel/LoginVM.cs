using GalaSoft.MvvmLight.Command;
using nmct.ba.cashlessproject.ui.management.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Login"; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        public ICommand LoginCommand
        {
            get { return new RelayCommand<PasswordBox>(Login); }
        }

        private void Login(PasswordBox pb)
        {
            ApplicationVM.token = GetToken(pb);

            if (!ApplicationVM.token.IsError)
            {
                //huidige window verbergen
                App.Current.MainWindow.Hide();

                MainWindow mw = new MainWindow();
                mw.Show();

                //login en password leegmaken, als er opnieuw ingelogd zou worden na het uitloggen
                Username = null;
                pb.Password = null;
            }
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
            }
        }

        private TokenResponse GetToken(PasswordBox pb)
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:55853/token"));
            return client.RequestResourceOwnerPasswordAsync(Username, pb.Password).Result;
        }
    }
}