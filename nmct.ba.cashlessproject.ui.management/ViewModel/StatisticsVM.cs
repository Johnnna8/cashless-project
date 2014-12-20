using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class StatisticsVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Statistieken"; }
        }
    }
}
