namespace API.DTO
{
    public class MessageDTO
    {

        public int MessageId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Recipient { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
