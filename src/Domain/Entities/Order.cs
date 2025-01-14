namespace DevKid.src.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public float Price { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}
