using System;

namespace TimeManagementApp.Note
{
    public partial class MyNote 
    {
        public MyNote(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public MyNote() {
            Id = 0;
            Name = "";
        }

        public int Id { get; set; }
        public String Name { get; set; }
    }
}
