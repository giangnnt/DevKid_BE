using DevKid.src.Domain.IRepository;

namespace DevKid.src.Application.Service
{
    public interface IBoughtCertificateService
    {
        public Task<bool> CheckCertificateAsync(Guid Id, Guid userId);
    }
    public class BoughtCertificateService : IBoughtCertificateService
    {
        private readonly ICourseRepo _courseRepo;
        public BoughtCertificateService(ICourseRepo courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<bool> CheckCertificateAsync(Guid Id, Guid userId)
        {
            var listId = new List<Guid>();
            var boughtCourse = await _courseRepo.GetBoughtCourse(userId);
            listId.AddRange(boughtCourse.Select(x => x.Id));
            listId.AddRange(boughtCourse.SelectMany(c => c.Chapters).Select(ch => ch.Id));
            listId.AddRange(boughtCourse.SelectMany(c => c.Chapters).SelectMany(ch => ch.Lessons).Select(l => l.Id));
            listId.AddRange(boughtCourse.SelectMany(c => c.Chapters).SelectMany(ch => ch.Lessons).SelectMany(l => l.Materials).Select(l => l.Id));
            listId.AddRange(boughtCourse.SelectMany(c => c.Chapters).SelectMany(ch => ch.Lessons).SelectMany(l => l.Quizzes).Select(l => l.Id));
            foreach (var id in listId)
            {
                if (Id == id)
                    return true;
            }
            return false;
        }
    }
}
