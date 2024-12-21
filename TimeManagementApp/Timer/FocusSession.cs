using System;

namespace TimeManagementApp.Timer
{
    public class FocusSession
    {
        public int Id { get; set; }
        public int Duration { get; set; } // Store duration as seconds
        public DateTime Timestamp { get; set; }
        public string Tag { get; set; }
    }
}
