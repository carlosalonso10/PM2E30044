using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E30044carlos.Controllers
{
    public class MyUtilities : IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource image = null;
            if (value != null)
            {
                byte[] bytes = value as byte[];
                Stream stream = new MemoryStream(bytes);
                image = ImageSource.FromStream(() => stream);
            }
            return image;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
