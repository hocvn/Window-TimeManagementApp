using System;

namespace TimeManagementApp.Note
{
    public partial class MyNote 
    {
        public MyNote(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public MyNote() {
            Id = "";
            Name = "";
        }

        public String Id { get; set; }
        public String Name { get; set; }
    }
}
