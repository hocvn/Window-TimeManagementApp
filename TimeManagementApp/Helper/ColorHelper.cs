using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace TimeManagementApp.Helper
{
    public static class ColorHelper
    {
        public static Color FromArgb(string colorString)
        {
            byte a = Convert.ToByte(colorString.Substring(1, 2), 16);
            byte r = Convert.ToByte(colorString.Substring(3, 2), 16);
            byte g = Convert.ToByte(colorString.Substring(5, 2), 16);
            byte b = Convert.ToByte(colorString.Substring(7, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }
    }

}
