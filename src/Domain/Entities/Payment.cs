namespace DevKid.src.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; } = null!; 
        public int Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string PaymentMethod { set; get; } = null!;
        public enum StatusEnum
        {
            Pending,
            Completed,
            Failed,
            Cancelled
        }
        public StatusEnum Status;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
