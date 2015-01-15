using be.belgium.eid;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;

        public static void getToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:55853/token"));
            string login = ConfigurationManager.AppSettings["Login"];
            string password = ConfigurationManager.AppSettings["Password"];

            token = client.RequestResourceOwnerPasswordAsync(login, password).Result;
        }

        public ApplicationVM()
        {
            Pages.Add(new SignInVM());
            pages.Add(new OrderVM());
            CurrentPage = Pages[0];

            getToken();

            if (token != null)
            {
                getRegister();
                GetOrganisation();
            }
        }

        private IPage currentPage;
        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private ObservableCollection<IPage> pages;
        public ObservableCollection<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new ObservableCollection<IPage>();
                return pages;
            }
            set
            {
                pages = value; OnPropertyChanged("Pages");
            }
        }

        private Organisation _organisation;

        public Organisation Organisation
        {
            get { return _organisation; }
            set { _organisation = value; OnPropertyChanged("Organisation"); }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(Logout); }
        }

        private void Logout()
        {
            ApplicationVM.employee = new Employee();

            changeUntilTime();
            ApplicationVM.registerEmployee = new RegisterEmployee();

            currentPage = Pages[0];
            OnPropertyChanged("CurrentPage");

            BEID_ReaderSet.releaseSDK();
        }

        private async void changeUntilTime()
        {
            ApplicationVM.registerEmployee.UntilTime = DateTime.Now;
            string input = JsonConvert.SerializeObject(registerEmployee);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:55853/api/registeremployee", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
            }
        }

        public void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        public static RegisterEmployee registerEmployee = new RegisterEmployee();
        public static Register register = new Register();
        public static Employee employee = new Employee();

        private async void getRegister()
        {
            int registerID = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterID"]);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/register?id=" + registerID);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    register = JsonConvert.DeserializeObject<Register>(json);
                }
            }
        }

        private async void GetOrganisation()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/organisation");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Organisation = JsonConvert.DeserializeObject<Organisation>(json);
                }
            }
        }
    }
}
