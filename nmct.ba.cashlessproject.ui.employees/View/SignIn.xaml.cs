using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nmct.ba.cashlessproject.ui.employees.View
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }

        //mvvm patroon doorbreken door gebruik van passwordbox

        private void txtPincode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Pincode = ((PasswordBox)sender).Password;
            }
        }

        private void addNumber(string n)
        {
            string pincode = txtPincode.Password;
            if (pincode.Length < 4) txtPincode.Password += n;
        }

        #region clicks
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            addNumber("1");
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            addNumber("2");
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            addNumber("3");
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            addNumber("4");
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            addNumber("5");
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            addNumber("6");
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            addNumber("7");
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            addNumber("8");
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            addNumber("9");
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            addNumber("0");
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtPincode.Password = "";
        }

        #endregion
    }
}
