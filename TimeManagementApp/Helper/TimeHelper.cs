using System;

namespace TimeManagementApp.Helper
{
    class TimeHelper
    {
        public static string GetTimeString()
        {
            return DateTime.Now.ToString("yyyy MM dd HH mm ss");
        }

        public static string GetTimesOfDay()
        {
            var currentHour = DateTime.Now.Hour;

            if (currentHour >= 5 && currentHour < 12)
            {
                return "Morning";
            }
            else if (currentHour >= 12 && currentHour < 18)
            {
                return "Afternoon";
            }
            else
            {
                return "Evening";
            }
        }

        public static DateTime GetToday()
        {
            return DateTime.Now.Date;
        }
    }
}
