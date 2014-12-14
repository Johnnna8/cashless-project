using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    class SignInVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Aanmelden"; }
        }
    }
}
