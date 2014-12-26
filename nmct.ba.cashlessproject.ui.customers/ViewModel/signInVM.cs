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

        public ICommand IdentificeerCommand
        {
            get { return new RelayCommand(Identificeer); }
        }

        private async void Identificeer()
        {
            BEID_EIDCard card = getData();

            if (card == null)
            {
                MessageBox.Show("Sluit de idreader aan en steek de kaart er correct in");
            }
            else
            {
                addCustomer(card);

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

        public static BEID_EIDCard getData()
        {
            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderContext Reader = BEID_ReaderSet.instance().getReader();

                if (Reader.isCardPresent())
                {
                    BEID_EIDCard card = Reader.getEIDCard();

                    if (card.isTestCard())
                    {
                        card.setAllowTestCard(true);
                    }

                    return card;
                }
                else
                {
                    return null;
                }
            }

            catch (BEID_Exception)
            {
                BEID_ReaderSet.releaseSDK();
                return null;
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

        private void addCustomer(BEID_EIDCard card)
        {
            byte[] bytesPicture = card.getPicture().getData().GetBytes();

            BEID_EId data = card.getID();
            string  nationalNumber = data.getNationalNumber();
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

        }
    }
}
