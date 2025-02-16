namespace DevKid.src.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Objective { get; set; }
        public string? ImageUrl { get; set; }
        public int Price { get; set; }  
        public List<Chapter> Chapters { get; set; } = new();
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<StudentCourse> StudentCourses { get; set; } = null!;
    }
}
