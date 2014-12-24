using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.helper
{
    public class BytesToImageConverter
    {
        //gekopieerd van https://social.msdn.microsoft.com/Forums/vstudio/en-US/8327dd31-2db1-4daa-a81c-aff60b63fee6/converting-an-imagebitmapimage-object-into-byte-array-and-vice-versa?forum=wpf
        public static BitmapImage BytesToImage(byte[] bytes)
        {
            if (bytes != null)
            {
                MemoryStream stream = new MemoryStream(bytes);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
            else
            {
                return null;
            }
        }
    }
}
