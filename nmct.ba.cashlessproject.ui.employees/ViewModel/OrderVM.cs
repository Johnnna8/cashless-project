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
            Sales = new ObservableCollection<Sale>();

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

            Register = new Register()
            {
                ID = 1,
                RegisterName = "testje",
                Device = "IBM"
            };

            Amount = "1 (standaard)";

            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
        }

        private ObservableCollection<Sale> _sales;

        public ObservableCollection<Sale> Sales
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

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        private Sale _selectedSale;

        public Sale SelectedSale
        {
            get { return _selectedSale; }
            set { _selectedSale = value; OnPropertyChanged("SelectedSale"); }
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

        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; OnPropertyChanged("Register"); }
        }

        private string _amount;

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }

        private string _errorProducts;

        public string ErrorProducts
        {
            get { return _errorProducts; }
            set { _errorProducts = value; OnPropertyChanged("ErrorProducts"); }
        }

        private string _errorSales;

        public string ErrorSales
        {
            get { return _errorSales; }
            set { _errorSales = value; OnPropertyChanged("ErrorSales"); }
        }

        private double _totalOrder;

        public double TotalOrder
        {
            get { return _totalOrder; }
            set { _totalOrder = value; OnPropertyChanged("TotalOrder"); }
        }

        public ICommand IncreaseAmountCommand
        {
            get { return new RelayCommand<string>(increaseAmount); }
        }

        public ICommand ResetAmountCommand
        {
            get { return new RelayCommand(resetAmount); }
        }

        public ICommand AddProductCommand
        {
            get { return new RelayCommand(addProduct); }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(deleteProduct); }
        }

        public ICommand CheckOutCommand
        {
            get { return new RelayCommand(checkOut); }
        }

        private void increaseAmount(string input)
        {
            string newAmount;

            //wanneer je een getal toetst en het aantal is standaard 1 --> dit wegnemen en getoetst getal plaatsen
            if (Amount == "1 (standaard)")
            {
                Amount = input;
            }
            //nieuw getal naast huidige getal plaatsen
            else
            {
                newAmount = Amount + input;

                if (Convert.ToInt32(newAmount) <= 99)
                {
                    Amount = newAmount;
                }
            }

            OnPropertyChanged("Amount");
        }

        private void addProduct()
        {
            if (SelectedProduct == null)
            {
                ErrorProducts = "Gelieve een product te selecteren";
                return;
            }
            else
            {
                ErrorProducts = "";
            }

            Sale newSale = new Sale();
            newSale.Register = Register;
            newSale.Customer = Customer;
            newSale.Product = SelectedProduct;
            newSale.Amount = giveAmount();
            newSale.TotalPrice = SelectedProduct.Price * giveAmount();

            Sales.Add(newSale);
            TotalOrder += newSale.TotalPrice;
            resetAmount();
        }

        private void resetAmount()
        {
            Amount = "1 (standaard)";
        }

        private int giveAmount()
        {
            //wanneer je zelf geen aantal kiest --> 1 product toevoegen
            if (Amount == "1 (standaard)") return 1;
            return Convert.ToInt32(Amount);
        }

        private void deleteProduct()
        {
            if (SelectedSale == null)
            {
                ErrorSales = "Gelieve een bestelt product te selecteren";
            }
            else
            {
                TotalOrder -= SelectedSale.TotalPrice;
                Sales.Remove(SelectedSale);
                ErrorSales = "";
            }
        }

        private void checkOut()
        {
            //voor elke sale, record in database plaatsen
            foreach (Sale sale in Sales)
            {
            }

            //bedrag customer verlagen in database
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
    }
}
