using be.belgium.eid;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.customers.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.customers.ViewModel
{
    class SignInVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Aanmelden"; }
        }

        public SignInVM()
        {
            if (ApplicationVM.customer != null)
            {
                Customer = ApplicationVM.customer;
            }
        }
        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        public ICommand ScanCustomerCommand
        {
            get { return new RelayCommand(ScanCustomer); }
        }

        private async void ScanCustomer()
        {
            BEID_EIDCard card = IDReader.getData();

            if (card == null)
            {
                MessageBox.Show("Sluit de id-reader aan en steek de kaart er correct in", "Niet correct aangesloten");
            }
            else
            {
                if (!addCustomer(card))
                {
                    MessageBox.Show("Sluit de id-reader aan en steek de kaart er correct in", "Niet correct aangesloten");
                    return;
                }

                ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                if (await checkCustomerExists())
                {
                    appvm.ChangePage(new ChargingVM());
                }
                else
                {
                    appvm.ChangePage(new RegisterVM());
                }
            }
        }

        private async Task<bool> checkCustomerExists()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/customer?nationalnumber=" + ApplicationVM.customer.NationalNumber);
                //HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/customer?nationalnumber=45"); heeft altijd true
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

                ApplicationVM.customer = new Customer()
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
    }
}
