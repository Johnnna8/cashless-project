using GalaSoft.MvvmLight.Command;
using nmct.ba.cashlessproject.ui.management.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            get { return new RelayCommand<MainWindow>(Logout); }
        }

        private void Logout(MainWindow mw)
        {

            if (!ApplicationVM.token.IsError)
            {
                mw.Hide();

                //login tonen
                App.Current.MainWindow.Show();

                token = null;
            }
        }
    }
}
