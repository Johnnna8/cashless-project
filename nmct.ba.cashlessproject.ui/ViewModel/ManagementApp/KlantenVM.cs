using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.ViewModel.ManagementApp
{
    class KlantenVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Klanten"; }
        }
    }
}
