namespace GuestBookTest.Models
{
    public class Message
    {
        public int messageId { get; set; }
        public string messageText { get; set; }
        public string reply { get; set; }
        public int userId { get; set; }
        public int writerId { get; set; }
    }
}
