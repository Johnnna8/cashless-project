using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    class SignInVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Aanmelden"; }
        }

        public SignInVM()
        {
            if (ApplicationVM.token != null)
            {
                getEmployees();
            }
        }

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private string _pincode;

        public string Pincode
        {
            get { return _pincode; }
            set { _pincode = value; OnPropertyChanged("Pincode"); }
        }

        public static Register register = null;

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        public ICommand InputPincodeCommand
        {
            get { return new RelayCommand<string>(inputPincode); }
        }

        public ICommand ClearPincodeCommand
        {
            get { return new RelayCommand(clearPincode); }
        }

        public ICommand IdentifyEmployeeCommand
        {
            get { return new RelayCommand(IdentifyEmployee); }
        }

        private void inputPincode(string input)
        {
            if (Pincode == null)
            {
                Pincode = input;

            //als de lengte bv. 3 is wordt er nog 1 keer een cijfertje achter gezet
            } else if (Pincode.Length < 4)
            {
                Pincode += input;
            }
        }

        private void clearPincode()
        {
            if (Pincode != null)
            {
                Pincode = null;
            }
        }

        private void IdentifyEmployee()
        {
            if (SelectedEmployee.Pincode == Pincode)
            {
                Error = "";
                ApplicationVM.employee = SelectedEmployee;
                createRegisterEmployee();

                ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                appvm.ChangePage(new OrderVM());
            }
            else
            {
                Error = "Uw ingevoerde pincode klopt niet";
            }
        }

        private async void createRegisterEmployee()
        {
            RegisterEmployee registerEmployee = new RegisterEmployee();
            registerEmployee.Register = ApplicationVM.register;
            registerEmployee.Employee = SelectedEmployee;
            registerEmployee.FromTime = DateTime.Now;
            registerEmployee.UntilTime = DateTime.Now;

            registerEmployee.ID = await addRegisterEmployee(registerEmployee);
            ApplicationVM.registerEmployee = registerEmployee;
        }

        private async void getEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        private async Task<int> addRegisterEmployee(RegisterEmployee registerEmployee)
        {
            string input = JsonConvert.SerializeObject(registerEmployee);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/registeremployee", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    return Int32.Parse(output);
                }
                else
                {
                    Console.WriteLine("error");
                    return 0;
                }
            }
        }
    }
	
}
