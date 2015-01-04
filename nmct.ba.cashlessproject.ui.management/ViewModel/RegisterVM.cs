using Newtonsoft.Json;
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
    class RegisterVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Kassa"; }
        }

        public RegisterVM()
        {
            Registers = new ObservableCollection<Register>();

            if (ApplicationVM.token != null)
            {
                GetRegisters();
            }
        }

        private ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private Register _selectedRegister;

        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); GetEmployeesPerRegister(SelectedRegister.ID); }
        }

        private ObservableCollection<RegisterEmployee> _registerEmployee;

        public ObservableCollection<RegisterEmployee> RegisterEmployee
        {
            get { return _registerEmployee; }
            set { _registerEmployee = value; OnPropertyChanged("RegisterEmployee"); }
        }

        private RegisterEmployee _selectedEmployee;

        public RegisterEmployee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private async void GetRegisters()
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

        private async void GetEmployeesPerRegister(int registerid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/registeremployee?registerid=" + registerid);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    RegisterEmployee = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);
                }
            }
        }

        /*private async void GetEmployeesRegister(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/register?registerid=" + id);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        private async void GetEmployeesRegisters(int registerid, int employeeid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/register?registerid=" +  registerid + "&employeeid=" + employeeid);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    RegisterEmployee = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json)[0];
                }
            }
        }*/
    }
}
