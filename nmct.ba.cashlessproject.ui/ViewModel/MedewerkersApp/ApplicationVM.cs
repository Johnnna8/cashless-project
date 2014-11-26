using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel.MedewerkersApp
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages.Add(new AanmeldenVM());
            pages.Add(new BestellenVM());
            CurrentPage = Pages[0];
        }

        private IPage currentPage;
        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private ObservableCollection<IPage> pages;
        public ObservableCollection<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new ObservableCollection<IPage>();
                return pages;
            }
            set
            {
                pages = value; OnPropertyChanged("Pages");
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }
    }
}
