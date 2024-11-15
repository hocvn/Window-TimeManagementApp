using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Global
{
    public class ViewModelContainer
    {
        // contain all global ViewModel that need to use on all pages
        public BackgroundViewModel BackgroundViewModel { get; set; } = new BackgroundViewModel();
    }
}
