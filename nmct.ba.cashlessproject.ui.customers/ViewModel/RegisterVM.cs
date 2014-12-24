using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.customers.ViewModel
{
    class RegisterVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Registreer"; }
        }

        public RegisterVM()
        {
            if (ApplicationVM.customer != null)
            {
                GetCustomer();
            }
        }

        private async void GetCustomer()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/customer?cnationalnumber=" + ApplicationVM.customer.NationalNumber);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<Customer>(json);
                }
            }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private double _amount;

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public ICommand RegisterCustomerCommand
        {
            get { return new RelayCommand(RegisterCustomer); }
        }

        private async void RegisterCustomer()
        {
            string input = JsonConvert.SerializeObject(Customer);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();

                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    appvm.ChangePage(new ChargingVM());
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }

    }
}
