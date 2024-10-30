using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace TimeManagementApp.Note
{
    public partial class MyNote : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public String Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
