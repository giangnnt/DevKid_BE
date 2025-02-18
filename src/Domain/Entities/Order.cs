namespace DevKid.src.Domain.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public int Price { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public Payment? Payment { get; set; }
        public enum StatusEnum
        {
            Pending,
            Completed,
            Failed,
            Cancelled
        }
        public StatusEnum Status;
    }
}
