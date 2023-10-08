
namespace SmartHomeApp.Models
{
    class TogglingResponse
    {
        public bool ison { get; set; }
        public bool has_timer { get; set; }
        public int timer_started { get; set; }
        public int timer_duration { get; set; }
        public int timer_remaining { get; set; }
        public bool overpower { get; set; }
        public string source { get; set; }
    }
}
