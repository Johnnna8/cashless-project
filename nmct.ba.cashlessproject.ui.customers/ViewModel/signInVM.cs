using be.belgium.eid;
using GalaSoft.MvvmLight.Command;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.customers.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.customers.ViewModel
{
    class SignInVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Aanmelden"; }
        }

        public ICommand IdentificeerCommand
        {
            get { return new RelayCommand(Identificeer); }
        }

        private void Identificeer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new RegisterVM());

            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderContext Reader = BEID_ReaderSet.instance().getReader();
                BEID_EIDCard card = Reader.getEIDCard();
                BEID_EId data = card.getID();

                int rijksregisternummer = Convert.ToInt32(data.getNationalNumber());

                /*using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/product");
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                    }
                }*/

            }

            catch (BEID_ExParamRange ex)
            {
                //MessageBox.Show("Error");
            }
            catch (BEID_Exception ex)
            {
                //MessageBox.Show("Sluit de idreader aan en steek de kaart er correct in");
            }

        }
    }
}
