using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Helper
{
    class TimeHelper
    {
        public static string GetTime()
        {
            return DateTime.Now.ToString("yyyy MM dd HH mm ss");
        }
    }
}
