using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Services
{
    public class UserSingleton
    {
        private static UserSingleton _instance = null;
        private static readonly object _padlock = new object();
        private UserSingleton()
        {
        }
        public static UserSingleton Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserSingleton();
                    }
                    return _instance;
                }
            }
        }

        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
        public string Email { get; set; }
    }
}
