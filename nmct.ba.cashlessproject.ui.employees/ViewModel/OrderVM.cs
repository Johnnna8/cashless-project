using be.belgium.eid;
using GalaSoft.MvvmLight.Command;
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
using System.Windows;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    class OrderVM : ObservableObject, IPage
    {
        #region ipage

        public string Name
        {
            get { return "Bestellen"; }
        }

        #endregion

        #region constructor

        public OrderVM()
        {
            Amount = "1 (standaard)";

            Register = ApplicationVM.register;
            Employee = ApplicationVM.employee;

            Sales = new ObservableCollection<Sale>();

            if (ApplicationVM.token != null)
            {
                getProducts();
            }
        }

        #endregion

        #region properties

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
            set {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");

                if (SelectedProduct != null)
                {
                    EnableDisableAdd = true;
                }
                else
                {
                    EnableDisableAdd = false;
                }
            }
        }

        private Sale _selectedSale;

        public Sale SelectedSale
        {
            get { return _selectedSale; }
            set {
                _selectedSale = value;
                OnPropertyChanged("SelectedSale");

                if (SelectedSale != null)
                {
                    EnableDisableDelete = true;
                }
                else
                {
                    EnableDisableDelete = false;
                }
            }
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

        private double _totalOrder;

        public double TotalOrder
        {
            get { return _totalOrder; }
            set { _totalOrder = value; OnPropertyChanged("TotalOrder"); }
        }

        private string _warningBalance;

        public string WarningBalance
        {
            get { return _warningBalance; }
            set { _warningBalance = value; OnPropertyChanged("WarningBalance"); }
        }

        #endregion

        #region enable disable properties

        private Boolean _enableDisableRegister;

        public Boolean EnableDisableRegister
        {
            get { return _enableDisableRegister; }
            set { _enableDisableRegister = value; OnPropertyChanged("EnableDisableRegister"); }
        }

        private Boolean _enableDisableZero;

        public Boolean EnableDisableZero
        {
            get { return _enableDisableZero; }
            set { _enableDisableZero = value; OnPropertyChanged("EnableDisableZero"); }
        }

        private Boolean _enableDisableAdd;

        public Boolean EnableDisableAdd
        {
            get { return _enableDisableAdd; }
            set { _enableDisableAdd = value; OnPropertyChanged("EnableDisableAdd"); }
        }

        private Boolean _enableDisableDelete;

        public Boolean EnableDisableDelete
        {
            get { return _enableDisableDelete; }
            set { _enableDisableDelete = value; OnPropertyChanged("EnableDisableDelete"); }
        }

        private Boolean _enableDisableCancelOrder;

        public Boolean EnableDisableCancelOrder
        {
            get { return _enableDisableCancelOrder; }
            set { _enableDisableCancelOrder = value; OnPropertyChanged("EnableDisableCancelOrder"); }
        }

        private Boolean _enableDisableCheckOut;

        public Boolean EnableDisableCheckOut
        {
            get { return _enableDisableCheckOut; }
            set { _enableDisableCheckOut = value; OnPropertyChanged("EnableDisableCheckOut"); }
        }

        #endregion

        #region commands

        public ICommand ScanCustomerCommand
        {
            get { return new RelayCommand(ScanCustomer); }
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

        public ICommand CancelOrderCommand
        {
            get { return new RelayCommand(cancelOrder); }
        }

        public ICommand CancelCustomerCommand
        {
            get { return new RelayCommand(cancelCustomer); }
        }

        #endregion

        #region functies commands

        private async void ScanCustomer()
        {
            cancelCustomer();

            BEID_EIDCard card = IDReader.getData();

            if (card == null)
            {
                MessageBox.Show("Sluit de idreader aan en steek de kaart er correct in", "Niet correct aangesloten");
            }
            else
            {
                if (!addCustomer(card))
                {
                    MessageBox.Show("Sluit de idreader aan en steek de kaart er correct in", "Niet correct aangesloten");
                    return;
                }

                if (await checkCustomerExists())
                {
                    getCustomer();
                    EnableDisableRegister = true;
                }
                else
                {
                    MessageBox.Show("De klant is nog niet geregistreert.", "Klant niet gevonden");
                    EnableDisableRegister = false;
                    EnableDisableZero = false;
                    EnableDisableCheckOut = false;
                }
            }
        }

        private void increaseAmount(string input)
        {
            string newAmount;

            //wanneer je een getal toetst en het aantal is standaard 1 --> dit wegnemen en getoetst getal plaatsen
            if (Amount == "1 (standaard)")
            {
                //onmogelijk om eerst het getal 0 in te tikken
                EnableDisableZero = true;

                Amount = input;
            }

            //nieuw getal naast huidige getal plaatsen
            else
            {
                //wanneer het aantal niet op standaard 1 staat, disabelen nadat je een toets indrukt
                EnableDisableZero = false;

                newAmount = Amount + input;
                if (Convert.ToInt32(newAmount) <= 99) Amount = newAmount;
            }

            OnPropertyChanged("Amount");
        }

        private void resetAmount()
        {
            Amount = "1 (standaard)";
        }

        private void addProduct()
        {
            if (SelectedProduct == null) return;
                EnableDisableCheckOut = true;

            Sale newSale = new Sale();
            newSale.Timestamp = DateTime.Now.ToUnixTimestamp();
            newSale.Register = Register;
            newSale.Customer = Customer;
            newSale.Product = SelectedProduct;
            newSale.Amount = giveAmount();
            newSale.TotalPrice = SelectedProduct.Price * giveAmount();
            Sales.Add(newSale);

            TotalOrder += newSale.TotalPrice;
            resetAmount();

            //controle of klant genoeg geld geeft om bestelling uit te voeren
            checkCheckOutAndCancelOrder();
        }

        private void deleteProduct()
        {
            if (SelectedSale == null) return;

            TotalOrder -= SelectedSale.TotalPrice;
            Sales.Remove(SelectedSale);

            checkCheckOutAndCancelOrder();
        }

        private void checkOut()
        {
            //voor elke sale, record in database plaatsen
            foreach (Sale sale in Sales)
            {
                addSale(sale);
            }

            //bedrag customer verlagen in database en bedrag updaten
            reduceBalanceCustomer();
            OnPropertyChanged("Customer");

            //resetten om nieuwe klant in te scannen
            cancelOrder();
        }

        private void cancelOrder()
        {
            EnableDisableZero = false;

            resetAmount();
            TotalOrder = 0;
            Sales = new ObservableCollection<Sale>();

            if (Customer != null) checkCheckOutAndCancelOrder();
        }

        private void cancelCustomer()
        {
            EnableDisableRegister = false;
            EnableDisableZero = false;
            EnableDisableAdd = false;

            cancelOrder();
            Customer = null;
            BEID_ReaderSet.releaseSDK();
        }

        #endregion

        #region hulpfuncties
        private Boolean addCustomer(BEID_EIDCard card)
        {
            try
            {
                byte[] bytesPicture = card.getPicture().getData().GetBytes();

                BEID_EId data = card.getID();
                string nationalNumber = data.getNationalNumber();
                string firstname = data.getFirstName1().Contains(' ') ? data.getFirstName1().Split(' ')[0] : data.getFirstName1();
                string lastname = data.getSurname();
                string street = data.getStreet();
                string postcode = data.getZipCode();
                string city = data.getMunicipality();

                Customer = new Customer()
                {
                    NationalNumber = nationalNumber,
                    Firstname = firstname,
                    Lastname = lastname,
                    Street = street,
                    Postcode = postcode,
                    City = city,
                    Picture = bytesPicture
                };

                return true;
            }
            catch (BEID_Exception ex)
            {
                IDReader.logError(ex);
                BEID_ReaderSet.releaseSDK();
                return false;
            }
        }

        private int giveAmount()
        {
            //wanneer je zelf geen aantal kiest --> 1 product toevoegen
            if (Amount == "1 (standaard)") return 1;
            return Convert.ToInt32(Amount);
        }

        private void checkCheckOutAndCancelOrder()
        {
            if (Customer.Balance - TotalOrder < 0 || Sales.Count == 0)
            {
                EnableDisableCheckOut = false;
            }
            else
            {
                EnableDisableCheckOut = true;
            }

            if (Customer.Balance - TotalOrder < 0) WarningBalance = "Geen genoeg geld om af te rekenen";
            else WarningBalance = "";

            if (Sales.Count == 0) EnableDisableCancelOrder = false;
            else EnableDisableCancelOrder = true;
        }

        #endregion

        #region database functies
        private async void getCustomer()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/customer?cnationalnumber=" + Customer.NationalNumber);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<Customer>(json);
                }
            }
        }

        private async Task<bool> checkCustomerExists()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/customer?nationalnumber=" + Customer.NationalNumber);
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

        private async void addSale(Sale sale)
        {
            string input = JsonConvert.SerializeObject(sale);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/sale", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    sale.ID = Int32.Parse(output);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }

        private async void reduceBalanceCustomer()
        {
            Customer.Balance -= TotalOrder;
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
    }
        #endregion
}
