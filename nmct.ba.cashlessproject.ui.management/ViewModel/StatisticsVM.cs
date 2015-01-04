using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class StatisticsVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Statistieken"; }
        }

        public StatisticsVM()
        {
            FromDate = DateTime.Now.AddDays(-6);
            UntilDate = DateTime.Now.AddDays(1);

            if (ApplicationVM.token != null)
            {
                getProducts();
                getRegisters();

                getSales();
            }
        }

        private DateTime _fromDate;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; OnPropertyChanged("FromDate"); if (Sales != null) UpdateSales(); }
        }

        private DateTime _untilDate;

        public DateTime UntilDate
        {
            get { return _untilDate; }
            set { _untilDate = value; OnPropertyChanged("UntilDate"); if (Sales != null) UpdateSales(); }
        }

        private ObservableCollection<Sale> salesPerRegister;

        public ObservableCollection<Sale> SalesPerRegister
        {
            get { return salesPerRegister; }
            set { salesPerRegister = value; OnPropertyChanged("SalesPerRegister"); }
        }

        private ObservableCollection<Sale> salesPerProduct;

        public ObservableCollection<Sale> SalesPerProduct
        {
            get { return salesPerProduct; }
            set { salesPerProduct = value; OnPropertyChanged("SalesPerProduct"); }
        }

        private ObservableCollection<Sale> _sales;

        public ObservableCollection<Sale> Sales
        {
            get { return _sales; }
            set { _sales = value; OnPropertyChanged("Sales"); }
        }

        private double _totalSales;

        public double TotalSales
        {
            get { return _totalSales; }
            set { _totalSales = value; OnPropertyChanged("TotalSales"); }
        }

        private ObservableCollection<Register> _registers;

        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private async void getProducts()
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

        private async void getRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/register");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                }
            }
        }

        private async void getSales()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/sale");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Sales = JsonConvert.DeserializeObject<ObservableCollection<Sale>>(json);
                }
            }

            TotalSales = Sales.Sum(s => s.TotalPrice);

            if (Sales != null) UpdateSales();
        }

        private void UpdateSales()
        {
            //alle sales ophalen die tussen de geselecteerde van en tot datum ligt
            //met group by unieke productnamen selecteren
            //per productnaam een nieuwe sale aanmaken die je selecteert: deze bevat 2 namen
            //- de som van alle verkopen ophalen die bij een bepaalde productnaam horen
            //- die bepaalde productnaam ophalen
            //bij deze code hulp gehad van klasgenoot

            SalesPerProduct = new ObservableCollection<Sale>(Sales
            .Where(s => s.Timestamp >= FromDate.ToUnixTimestamp() && s.Timestamp <= UntilDate.ToUnixTimestamp())
            .GroupBy(p => p.Product.ProductName)
            .Select(Group => new Sale {
                TotalPrice = Group.Sum(total => total.TotalPrice),
                Product = new Product() { ProductName = Group.Key }
            }));

            SalesPerRegister = new ObservableCollection<Sale>(Sales
            .Where(s => s.Timestamp >= FromDate.ToUnixTimestamp() && s.Timestamp <= UntilDate.ToUnixTimestamp())
            .GroupBy(p => p.Register.RegisterName)
            .Select(Group => new Sale { 
                TotalPrice = Group.Sum(total => total.TotalPrice), 
                Register = new Register() { RegisterName = Group.Key } 
            }));
        }
    }
}
