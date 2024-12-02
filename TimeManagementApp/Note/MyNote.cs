using System;

namespace TimeManagementApp.Note
{
    public partial class MyNote 
    {
        public MyNote(int id, string name)
        {
            Id = id;
            Name = name;
            Content = "";
        }

        public MyNote() {
            Id = 0;
            Name = "";
            Content = "";
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }
    }
}
