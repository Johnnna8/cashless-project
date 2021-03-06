﻿using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.management.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    public class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;

        public ApplicationVM()
        {
            Pages.Add(new EmployeeVM());
            Pages.Add(new CustomerVM());
            Pages.Add(new ProductVM());
            Pages.Add(new RegisterVM());
            Pages.Add(new StatisticsVM());
            Pages.Add(new ChangePasswordVM());

            CurrentPage = Pages[0];

            if (token != null)
            {
                GetOrganisation();
            }
        }

        private IPage currentPage;
        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private ObservableCollection<IPage> _pages;
        public ObservableCollection<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new ObservableCollection<IPage>();
                return _pages;
            }
            set
            {
                _pages = value; OnPropertyChanged("Pages");
            }
        }

        private Organisation _organisation;

        public Organisation Organisation
        {
            get { return _organisation; }
            set { _organisation = value; OnPropertyChanged("Organisation"); }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        public void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(Logout); }
        }

        private void Logout()
        {

            if (!ApplicationVM.token.IsError)
            {
                //huidige windows sluiten en loginpagina openen
                Login login = new Login();
                login.Show();
                App.Current.MainWindow.Close();
                App.Current.MainWindow = login;

                token = null;
            }
        }

        private async void GetOrganisation()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/organisation");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Organisation = JsonConvert.DeserializeObject<Organisation>(json);
                }
            }
        }
    }
}
