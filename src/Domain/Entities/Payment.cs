namespace DevKid.src.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!; 
        public float Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string PaymentMethod { set; get; } = "VNPay";
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
