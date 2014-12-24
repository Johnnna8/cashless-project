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

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class ProductVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Producten"; }
        }

        public ProductVM()
        {
            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
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
            get {
                return _selectedProduct;
            }
            set {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");

                if (SelectedProduct != null)
                {
                    EnableDisableButtons = true;
                }
                else
                {
                    EnableDisableButtons = false;
                }
            }
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

        public ICommand NewProductCommand
        {
            get { return new RelayCommand(NewProduct);  }
        }

        public ICommand SaveProductCommand
        {

            get {
                return new RelayCommand(SaveProduct);
            }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }


        private Boolean _enableDisableButtons;

        public Boolean EnableDisableButtons
        {
            get { return _enableDisableButtons; }
            set { _enableDisableButtons = value; OnPropertyChanged("EnableDisableButtons"); }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private void NewProduct()
        {
            Product p = new Product();
            Products.Add(p);
            SelectedProduct = p;
        }

        private async void SaveProduct()
        { 
            if (SelectedProduct == null || !SelectedProduct.IsValid())
            {
                Error = "Product niet toegevoegd of gewijzigd, houd rekening met de meldingen.";
                return;
            }
            else
            {
                Error = "";
            }

            string input = JsonConvert.SerializeObject(SelectedProduct);

            // check insert (no ID assigned) or update (already an ID assigned)
            if (SelectedProduct.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/product", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedProduct.ID = Int32.Parse(output);
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PutAsync("http://localhost:55853/api/product", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteProduct()
        {
            //opvangen dat je op verwijderen klikt zonder een product te selecteren
            if (SelectedProduct == null)
            {
                return;
            }
            else
            {
                Error = "";
            }

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:55853/api/product/" + SelectedProduct.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Products.Remove(SelectedProduct);
                }
            }
        }
    }
}