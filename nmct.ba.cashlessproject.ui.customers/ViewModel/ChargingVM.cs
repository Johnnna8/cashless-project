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
    class ChargingVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Opladen"; }
        }

        public ChargingVM()
        {
            if (ApplicationVM.customer != null && ApplicationVM.token != null)
            {
                GetCustomer();
            }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private string _foutmelding;

        public string Foutmelding
        {
            get { return _foutmelding; }
            set { _foutmelding = value; OnPropertyChanged("Foutmelding"); }
        }

        public ICommand AddFiveCommand
        {
            get { return new RelayCommand(AddFive); }
        }

        public ICommand AddTenCommand
        {
            get { return new RelayCommand(AddTen); }
        }

        public ICommand AddTwentyCommand
        {
            get { return new RelayCommand(AddTwenty); }
        }

        public ICommand AddFiftyCommand
        {
            get { return new RelayCommand(AddFifty); }
        }

        private void AddFive()
        {
            AddWithAmount(5);
        }

        private void AddTen()
        {
            AddWithAmount(10);
        }

        private void AddTwenty()
        {
            AddWithAmount(20);
        }
        private void AddFifty()
        {
            AddWithAmount(50);
        }

        private void AddWithAmount(int amount)
        {
            double newAmount = Customer.Balance + amount;

            if (newAmount <= 100)
            {
                Customer.Balance += amount;
                OnPropertyChanged("Customer");
                AddAmountDatabase();
                Foutmelding = "";
            } else {
                Foutmelding = "U kunt maximaal 100 euro opladen";
            }
        }

        private async void AddAmountDatabase()
        {
            string input = JsonConvert.SerializeObject(Customer);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:55853/api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
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
    }
}
