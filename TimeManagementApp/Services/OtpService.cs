﻿using System;

namespace TimeManagementApp.Services
{
    /// <summary>
    /// This class supports generate OTP code
    /// </summary>
    /// 
    public class OtpService
    {
        public static string GenerateOtp(int length = 6)
        {
            var random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10);
            }
            return otp;
        }
    }
}
