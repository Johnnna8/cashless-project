using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.be.cashlessproject.ui.ViewModel.ManagementApp
{
    class BeheerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Beheer"; }
        }
    }
}
