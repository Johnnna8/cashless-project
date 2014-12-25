using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    class OrderVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Bestellen"; }
        }

        public OrderVM()
        {
            Customer = new Customer()
            {
                ID = 2,
                Firstname = "Jonathan Dries",
                Lastname = "Houck",
                Balance = 50
            };

            Employee = new Employee()
            {
                ID = 1,
                Firstname = "Jos",
                Lastname = "Houck"
            };

            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
        }

        private ObservableCollection<Sales> _sales;

        public ObservableCollection<Sales> Sales
        {
            get { return _sales; }
            set { _sales = value; OnPropertyChanged("Sales"); }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; OnPropertyChanged("Customer"); }
        }

        private int _aantal;

        public int Aantal
        {
            get { return _aantal; }
            set { _aantal = value; OnPropertyChanged("Aantal"); }
        }
        

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
            }
        }

        public ICommand VerhoogAantalCommand
        {
            get { return new RelayCommand<string>(verhoogAantal); }
        }

        private void verhoogAantal(string input)
        {
            //cijfers worden niet opgeteld maar naast elkaar geplaatst
            string aantal = Aantal + input;

            if (Convert.ToInt32(aantal) <= 99)
            {
                Aantal = Convert.ToInt32(aantal);
            }

            OnPropertyChanged("Aantal");
        }
    }
}
