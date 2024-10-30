using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Note;

namespace TimeManagementApp.Dao
{
    interface IDao
    {
        ObservableCollection<MyNote> GetAllNote();
        MyNote GetNote();

        void SaveRtf(RichEditBox editor, MyNote note);

        void OpenRtf(RichEditBox editor, MyNote note);

    }
}
