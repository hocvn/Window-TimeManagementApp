using System;

namespace TimeManagementApp.Timer
{
    public class FocusSession
    {
        public int Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Timestamp { get; set; }
        public string Tag { get; set; }
    }
}
