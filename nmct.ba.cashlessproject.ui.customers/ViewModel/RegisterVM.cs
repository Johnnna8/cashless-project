using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.customers.ViewModel
{
    class RegisterVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Registreer"; }
        }
    }
}
